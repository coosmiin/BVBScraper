using System.Linq;
using System.Threading.Tasks;
using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using Investments.Logic.Portfolios;
using Investments.Logic.Weights;
using Trading.BVBScraper;

namespace BVBTrading.Console
{
	class Program
	{
		private const int MIN_ORDER_VALUE = 250;

		static async Task Main(string[] args)
		{
			var scraper = new StockScraper();
			var betStocks = await scraper.ScrapeBETComposition();

			var stockPrices = betStocks.ToDictionary(s => s.Symbol, s => s.Price).AsStockPrices();
			var targetWeights = betStocks.ToDictionary(s => s.Symbol, s => s.Weight).AsStockWeights();

			var stocks = new[]
{
				new Stock("TLV") { Count = 313 },
				new Stock("FP") { Count = 665 },
				new Stock("SNP") { Count = 1668 },
				new Stock("SNG") { Count = 12 },
				new Stock("BRD") { Count = 28 },
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

			var toBuyAmount = 2198.93m;
			decimal currentPortfolioValue = stocks.Sum(s => s.TotalValue);

			var strategy = 
				new MinOrderValueCutOffStrategy(
					new FollowTargetAdjustmentStrategy(), MIN_ORDER_VALUE / currentPortfolioValue);

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

			foreach (var stock in portfolio.DeriveAdditionalPortfolio(stocks))
			{
				System.Console.WriteLine($"{stock.Symbol} ({stock.Price})\t {stock.Count}\t ({stock.Count * stock.Price})\t {stock.Weight} ({targetWeights[stock.Symbol]})");
			}
		}
	}
}
