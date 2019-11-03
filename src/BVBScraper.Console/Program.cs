using System.Threading.Tasks;

namespace BVBScraper.Console
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var scraper = new StockScraper();
			foreach (var stock in await scraper.ScrapeBETComposition())
			{
				System.Console.WriteLine($"{stock.Symbol} ({stock.Name}): {stock.Price} [{stock.Weight}]");
			}
		}
	}
}
