using System.Threading.Tasks;

namespace Trading.BVBScraper.Console
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var scraper = new StockScraper();
			foreach (var stock in await scraper.ScrapeBETComposition())
			{
				System.Console.WriteLine($"{stock.Symbol}\t: {stock.Price}\t [{stock.Weight}] ({stock.Name})");
			}
		}
	}
}
