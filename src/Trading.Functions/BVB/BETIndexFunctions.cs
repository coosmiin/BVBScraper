using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Trading.BVBScraper;
using Trading.Functions.Environments;

namespace Trading.Functions.BVB
{
	public class BETIndexFunctions
	{
		private readonly StockScraper _stockScraper;

		public BETIndexFunctions(StockScraper stockScraper, IEnvironment environment)
		{
			_stockScraper = stockScraper ?? throw new System.ArgumentNullException(nameof(stockScraper));
		}

		[FunctionName(nameof(ScrapeBETIndex))]
		public async Task<IActionResult> ScrapeBETIndex(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "scrapeBETIndex")] HttpRequest request)
		{
			var scraper = new StockScraper();
			var stocks = (await scraper.ScrapeBETComposition()).ToArray();

			return new OkObjectResult(stocks);
		}
	}
}
