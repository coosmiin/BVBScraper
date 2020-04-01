using Investments.Advisor.Providers;
using Investments.Domain.Stocks;
using System.Threading.Tasks;

namespace Investments.Advisor.Trading
{
	public class TradeSessionOrchestrator : ITradeSessionOrchestrator
	{
		private readonly IBVBDataProvider _bvbDataProvider;

		public TradeSessionOrchestrator(IBVBDataProvider bvbDataProvider)
		{
			_bvbDataProvider = bvbDataProvider;
		}

		public async Task<Stock[]> GetBETStocksAsync()
		{
			return await _bvbDataProvider.GetBETStocksAsync();
		}
	}
}
