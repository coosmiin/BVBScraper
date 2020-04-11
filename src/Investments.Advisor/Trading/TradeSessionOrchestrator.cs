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
		public async Task<Stock[]> GetToBuyStocks(decimal toBuyAmount)
		{
			var betStocks = await _bvbDataProvider.GetBETStocksAsync();

			var existingStocks = await _tradeAutomation.GetPortfolio();

			return await _tradeAdvisor.CalculateToBuyStocksAsync(toBuyAmount, existingStocks, betStocks);
		}

		public async Task Run()
		{
			var betStocks = await _bvbDataProvider.GetBETStocksAsync();

			var existingStocks = await _tradeAutomation.GetPortfolio();

			var toBuyStocks = await _tradeAdvisor.CalculateToBuyStocksAsync(2393.13m, existingStocks, betStocks);
		}
	}
}
