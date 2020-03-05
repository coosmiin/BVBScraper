using Investments.Advisor.Providers;
using Investments.Domain.Stocks;
using System.Threading.Tasks;

namespace Investments.Advisor.Trading
{
	public class TradeSessionOrchestrator : ITradeSessionOrchestrator
	{
		private readonly IBVBDataProvider _bvbRepository;

		public TradeSessionOrchestrator(IBVBDataProvider bvbRepository)
		{
			_bvbRepository = bvbRepository;
		}

		public async Task<Stock[]> GetBETStocksAsync()
		{
			return await _bvbRepository.GetBETStocksAsync();
		}
	}
}
