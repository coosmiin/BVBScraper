using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Trading.BVBScraper;

namespace Trading.Functions.BVB
{
	public class BvbIndexFunctions
	{
		private readonly StockScraper _stockScraper;

		public BvbIndexFunctions(StockScraper stockScraper)
		{
			_stockScraper = stockScraper ?? throw new ArgumentNullException(nameof(stockScraper));
		}

		[FunctionName(nameof(ScrapeBvbIndex))]
		public Task<IActionResult> ScrapeBvbIndex(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "ScrapeBvbIndex")] HttpRequest request)
		{
			string? indexName = request.Query["index"];

			if (string.IsNullOrEmpty(indexName))
				throw new ArgumentException($"'index' query string param cannot be null or empty");

			return ScrapeBetIndex();

			async Task<IActionResult> ScrapeBetIndex()
			{
				var stocks = (await _stockScraper.ScrapeIndexdComposition(indexName)).ToArray();

				return new OkObjectResult(stocks);
			}
		}
	}
}
