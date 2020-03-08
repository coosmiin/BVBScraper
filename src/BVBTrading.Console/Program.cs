using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Investments.Advisor.Providers;
using Investments.Advisor.Trading;
using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using Investments.Logic.Portfolios;
using Investments.Logic.Weights;
using SecretStore;

namespace BVBTrading.Console
{
	class Program
	{
		private const int MIN_ORDER_VALUE = 250;

		static async Task Main(string[] args)
		{
			var secretStore = new LocalSecretStore<Program>();

			var repository = new AzureFuncBVBDataProvider(new HttpClient(), secretStore.GetSecret("Azure:TradingFuncKey"));
			// var repository = new StaticDataBVBDataProvider();
			var orchestrator = new TradeSessionOrchestrator(repository);

			var betStocks = await orchestrator.GetBETStocksAsync();

			var stockPrices = betStocks.AsStockPrices();
			var targetWeights = betStocks.AsStockWeights();

			var stocks = new[]
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

			stocks = stocks.UpdatePrices(stockPrices);

			var toBuyAmount = 2393.13m;
			decimal currentPortfolioValue = stocks.Sum(s => s.TotalValue);

			var strategy = 
				new MinOrderValueCutOffStrategy(
					new FollowTargetAdjustmentStrategy(), MIN_ORDER_VALUE / toBuyAmount);

			var portfolio = new PortfolioBuilder()
				.UseStocks(stocks)
				.UsePrices(stockPrices)
				.UseTargetWeights(targetWeights)
				.UseToBuyAmount(toBuyAmount)
				.UseMinOrderValue(MIN_ORDER_VALUE)
				.UseWeightAdjustmentStrategy(strategy)
				.Build();

			foreach (var stock in portfolio)
			{
				System.Console.WriteLine($"{stock.Symbol} ({stock.Price})\t {stock.Count}\t ({stock.Count * stock.Price})\t {stock.Weight} ({targetWeights[stock.Symbol]})");
			}

			System.Console.WriteLine("-------------------------------------");

			System.Console.WriteLine($"Weights check: {portfolio.Sum(s => s.Weight)}");

			System.Console.WriteLine("-------------------------------------");

			var newPortfolio = portfolio.DeriveAdditionalPortfolio(stocks);
			foreach (var stock in newPortfolio)
			{
				System.Console.WriteLine($"{stock.Symbol} ({stock.Price})\t {stock.Count}\t ({stock.Count * stock.Price})\t {stock.Weight} ({targetWeights[stock.Symbol]})");
			}

			System.Console.WriteLine("-------------------------------------");

			System.Console.WriteLine($"Invested sum: {newPortfolio.Sum(s => s.Count * s.Price)}");
		}
	}
}
