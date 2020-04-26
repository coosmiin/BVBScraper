using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Trading.BVBScraper;
using System.Linq;

namespace Trading.Functions.BVB
{
	public static class BETIndexFunctions
	{
		[FunctionName(nameof(ScrapeBETIndex))]
		public static async Task<IActionResult> ScrapeBETIndex(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "scrapeBETIndex")] HttpRequest request)
		{
			var scraper = new StockScraper();
			var stocks = (await scraper.ScrapeBETComposition()).ToArray();

			return new OkObjectResult(stocks);
		}
	}
}
