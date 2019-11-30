using System.Linq;
using System.Threading.Tasks;
using Investments.Logic.Portfolios;
using Trading.BVBScraper;

namespace BVBTrading.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
   			var scraper = new StockScraper();
            var betStocks = await scraper.ScrapeBETComposition();

            var stockPrices = betStocks.ToDictionary(s => s.Symbol, s => s.Price);
            var targetWeights = betStocks.ToDictionary(s => s.Symbol, s => s.Weight);

            var portfolio = new PortfolioBuilder()
                .UsePrices(stockPrices)
                .UseWeights(targetWeights)
                .UseAmount(2000)
                .Build();

			foreach (var stock in portfolio)
			{
				System.Console.WriteLine($"{stock.Symbol} ({stock.Price})\t {stock.Count}\t {stock.Weight} ({targetWeights[stock.Symbol]})");
			}            
        }
    }
}
