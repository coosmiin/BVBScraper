using Investments.Advisor.Providers;
using Investments.Domain.Stocks;
using System;
using System.Threading.Tasks;

namespace Investments.Advisor.Trading
{
	public class TradeSessionOrchestrator : ITradeSessionOrchestrator
	{
		private readonly IBVBDataProvider _bvbDataProvider;
		private readonly ITradeAutomation _tradeAutomation;
		private readonly ITradeAdvisor _tradeAdvisor;

		public TradeSessionOrchestrator(IBVBDataProvider bvbDataProvider, ITradeAutomation tradeAutomation, ITradeAdvisor tradeAdvisor)
		{
			_bvbDataProvider = bvbDataProvider;
			_tradeAutomation = tradeAutomation;
			_tradeAdvisor = tradeAdvisor;
		}

		[Obsolete("To be removed when trade automation func is in place")]
		public async Task<Stock[]> GetToBuyStocks()
		{
			var betStocks = await _bvbDataProvider.GetBETStocksAsync();

			var (existingStocks, availableAmount) = await _tradeAutomation.GetPortfolio();

			return await _tradeAdvisor.CalculateToBuyStocksAsync(availableAmount, existingStocks, betStocks);
		}

		public async Task Run()
		{
			var betStocks = await _bvbDataProvider.GetBETStocksAsync();

			var (existingStocks, availableAmount) = await _tradeAutomation.GetPortfolio();

			availableAmount *= 0.85m; // to overcome order value estimation risk

			var toBuyStocks = 
				await _tradeAdvisor.CalculateToBuyStocksAsync(availableAmount, existingStocks, betStocks);
		}
	}
}
