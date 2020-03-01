using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Trading.BVBScraper;
using System.Linq;

namespace BVBScraper.Functions
{
	public static class BETIndex
	{
		[FunctionName("ScrapeBETIndex")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
			ILogger log)
		{
			try
			{
				var scraper = new StockScraper();
				var stocks = (await scraper.ScrapeBETComposition()).ToArray();

				log.LogInformation($"Scraped BET BVB Index: {stocks.Count()} stocks");

				return new OkObjectResult(stocks);
			}
			catch (Exception ex)
			{
				log.LogError(ex.Message);
				return null;
			}
		}
	}
}
