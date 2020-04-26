using Investments.Advisor.Providers;
using Investments.Domain.Trading;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Investments.Advisor.Trading
{
	public class TradeSessionOrchestrator : ITradeSessionOrchestrator
	{
		private readonly IBVBDataProvider _bvbDataProvider;
		private readonly ITradeAutomation _tradeAutomation;
		private readonly ITradeAdvisor _tradeAdvisor;
		private readonly ILogger _logger;

		public TradeSessionOrchestrator(
			IBVBDataProvider bvbDataProvider,
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

			var betStocks = await _bvbDataProvider.GetBETStocksAsync();

			_logger.LogInformation(
				"Retrieved BET: {betStocks}", 
				JsonSerializer.Serialize(betStocks.Select(s => new { s.Symbol, s.Price, s.Weight}).ToArray()));

			var (existingStocks, availableAmount) = await _tradeAutomation.GetPortfolio();

			var toBuyAmount = availableAmount * 0.85m; // to overcome order value estimation risk

			_logger.LogInformation(
				"Retrieved current Portfolio: ({availableAmount} * 0.85 = {toBuyAmount}) {currentStocks}", 
				availableAmount, 
				toBuyAmount,
				JsonSerializer.Serialize(existingStocks.Select(s => new { s.Symbol, s.Count }).ToArray()));

			var toBuyStocks = 
				await _tradeAdvisor.CalculateToBuyStocksAsync(toBuyAmount, existingStocks, betStocks);

			_logger.LogInformation(
				"Calculated to buy stocks: {toBuyStocks}",
				JsonSerializer.Serialize(
					toBuyStocks.Select(s => 
						new 
						{ 
							s.Symbol, 
							s.Count, 
							Value = betStocks.Where(bs => bs.Symbol == s.Symbol).Single().Price * s.Count
						})
					.ToArray()));

			var tradeOrders =
				toBuyStocks
					.Select(s => new TradeOrder { Symbol = s.Symbol, Count = s.Count, OperationType = OperationType.Buy })
					.ToArray();

			await _tradeAutomation.SubmitOrders(tradeOrders);

			_logger.LogInformation("Orders submitted. Transaction session completed!");
		}
	}
}
