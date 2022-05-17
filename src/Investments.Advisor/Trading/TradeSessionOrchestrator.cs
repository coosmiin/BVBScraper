using System.Linq;
using System.Threading.Tasks;
using Investments.Advisor.Providers;
using Investments.Domain.Stocks;
using Investments.Domain.Trading;
using Investments.Utils.Serialization;
using Microsoft.Extensions.Logging;

namespace Investments.Advisor.Trading
{
	public class TradeSessionOrchestrator : ITradeSessionOrchestrator
	{
		private readonly IBvbDataProvider _bvbDataProvider;
		private readonly ITradeAutomation _tradeAutomation;
		private readonly ITradeAdvisor _tradeAdvisor;
		private readonly ILogger _logger;

		public TradeSessionOrchestrator(
			IBvbDataProvider bvbDataProvider,
			ITradeAutomation tradeAutomation,
			ITradeAdvisor tradeAdvisor,
			ILogger<TradeSessionOrchestrator> logger)
		{
			_bvbDataProvider = bvbDataProvider;
			_tradeAutomation = tradeAutomation;
			_tradeAdvisor = tradeAdvisor;
			_logger = logger;
		}

		public async Task Run()
		{
			_logger.LogInformation("Start transaction session");
			
			var getBetStocksTask = GetBvbStocks("BET");
			var getBetAeroStocksTask = GetBvbStocks("BETAeRO");

			Task.WaitAll(getBetStocksTask, getBetAeroStocksTask);

			var betStocks = getBetStocksTask.Result;
			var betAeroStocks = getBetAeroStocksTask.Result;

			// Aprox. target ratio: 2/3 BET + 1/3 BETAeRO
			// Doing buys sequentialy since the estimation risk is elimineted after first buy 

			// First, buy BET index
			// Ratio is less than 2/3 to overcome the order value estimation risk as well
			await BuyIndex(0.6m, betStocks, "REGS");

			// Second, buy BETAeRO index
			// Ratio should compensate the order value estimation risk
			await BuyIndex(0.75m, betAeroStocks, "XRS1");

			_logger.LogInformation("Session completed!");

			async Task BuyIndex(decimal ratio, Stock[] indexStocks, string market)
			{
				var (existingStocks, availableAmount) = await _tradeAutomation.GetPortfolio();

				var toBuyAmount = availableAmount * ratio;

				_logger.LogInformation(
					"Retrieved current Portfolio: ({availableAmount} * {ratio} = {toBuyAmount}) {currentStocks}",
					availableAmount,
					ratio,
					toBuyAmount,
					JsonSerializerHelper.Serialize(existingStocks.Select(s => new { s.Symbol, s.Count }).ToArray()));

				var indexStocksSet = indexStocks.Select(stock => stock.Symbol).ToHashSet();

				// Keep only the stocks from the current index
				existingStocks = existingStocks
					.Where(stock => indexStocksSet.Contains(stock.Symbol))
					.ToArray();

				var toBuyStocks =
					await _tradeAdvisor.CalculateToBuyStocksAsync(toBuyAmount, existingStocks, indexStocks);
				
				if (!toBuyStocks.Any())
				{
					_logger.LogWarning("Advisor could not find any stocks to buy!");
					return;
				}

				_logger.LogInformation(
					"Calculated to buy stocks: {toBuyStocks}",
					JsonSerializerHelper.Serialize(
						toBuyStocks.Select(toBuyStock =>
							new
							{
								toBuyStock.Symbol,
								toBuyStock.Count,
								Value = indexStocks.Single(stock => stock.Symbol == toBuyStock.Symbol).Price * toBuyStock.Count,
								market
							})
						.ToArray()));

				var tradeOrders =
					toBuyStocks
						.Select(stock => new TradeOrder 
						{ 
							Symbol = stock.Symbol, 
							Count = stock.Count, 
							OperationType = OperationType.Buy,
							Market = market
						})
						.ToArray();

				await _tradeAutomation.SubmitOrders(tradeOrders);

				_logger.LogInformation("Orders submitted");

				await _tradeAutomation.SignOrders();

				_logger.LogInformation("Orders signed");

			}

			async Task<Stock[]> GetBvbStocks(string index)
			{
				var bvbStocks = await _bvbDataProvider.GetBvbStocksAsync(index);

				_logger.LogInformation(
					"Retrieved {index}: {bvbStocks}",
					index,
					JsonSerializerHelper.Serialize(bvbStocks.Select(stock => new { stock.Symbol, stock.Price, stock.Weight }).ToArray()));

				return bvbStocks;
			}
		}
	}
}
