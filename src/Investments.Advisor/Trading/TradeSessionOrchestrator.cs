using Investments.Advisor.Providers;
using Investments.Domain.Stocks;
using System;
using System.Threading.Tasks;

namespace Investments.Advisor.Trading
{
	public class TradeSessionOrchestrator : ITradeSessionOrchestrator
	{
		private readonly IBVBDataProvider _bvbDataProvider;
		private readonly ITradeAdvisor _tradeAdvisor;

		public TradeSessionOrchestrator(IBVBDataProvider bvbDataProvider, ITradeAdvisor tradeAdvisor)
		{
			_bvbDataProvider = bvbDataProvider;
			_tradeAdvisor = tradeAdvisor;
		}

		[Obsolete("To be removed when trade automation func is in place")]
		public async Task<Stock[]> GetToBuyStocks()
		{
			var betStocks = await _bvbDataProvider.GetBETStocksAsync();

			var existingStocks = new[]
{
				new Stock("TLV") { Count = 492 },
				new Stock("FP") { Count = 1081 },
				new Stock("SNP") { Count = 2676 },
				new Stock("SNG") { Count = 12 },
				new Stock("BRD") { Count = 28 },
				new Stock("TGN") { Count = 1 },
				new Stock("EL") { Count = 14 },
				new Stock("DIGI") { Count = 4 },
				new Stock("SNN") { Count = 6 },
				new Stock("TEL") { Count = 2 },
				new Stock("ALR") { Count = 21 },
				new Stock("M") { Count = 2 },
				new Stock("WINE") { Count = 2 },
				new Stock("SFG") { Count = 2 }
			};

			return await _tradeAdvisor.CalculateToBuyStocksAsync(2393.13m, existingStocks, betStocks);
		}

		public async Task Run()
		{
			var betStocks = await _bvbDataProvider.GetBETStocksAsync();

			var existingStocks = new[]
{
				new Stock("TLV") { Count = 492 },
				new Stock("FP") { Count = 1081 },
				new Stock("SNP") { Count = 2676 },
				new Stock("SNG") { Count = 12 },
				new Stock("BRD") { Count = 28 },
				new Stock("TGN") { Count = 1 },
				new Stock("EL") { Count = 14 },
				new Stock("DIGI") { Count = 4 },
				new Stock("SNN") { Count = 6 },
				new Stock("TEL") { Count = 2 },
				new Stock("ALR") { Count = 21 },
				new Stock("M") { Count = 2 },
				new Stock("WINE") { Count = 2 },
				new Stock("SFG") { Count = 2 }
			};

			var toBuyStocks = await _tradeAdvisor.CalculateToBuyStocksAsync(2393.13m, existingStocks, betStocks);
		}
	}
}
