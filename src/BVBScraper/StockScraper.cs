using AngleSharp;
using AngleSharp.Html.Dom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trading.BVBScraper
{
	public class StockScraper
	{
#pragma warning disable S1075 // URIs should not be hardcoded
		private const string INDEX_COMPOSITION_URL_FORMAT = "https://www.bvb.ro/FinancialInstruments/Indices/IndicesProfiles.aspx?i={0}";
#pragma warning restore S1075 // URIs should not be hardcoded

		private readonly IBrowsingContext _browsingContext;

		public StockScraper()
		{
			_browsingContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
		}

		public async Task<IEnumerable<BvbStock>> ScrapeIndexdComposition(string index)
		{
			var indexUrl = string.Format(INDEX_COMPOSITION_URL_FORMAT, index);
			var document = await _browsingContext.OpenAsync(indexUrl);

			var stockRows = document
				.QuerySelectorAll("table#gvC tbody tr")
				.OfType<IHtmlTableRowElement>();

			var stocks = new List<BvbStock>();

#pragma warning disable S3267 // Loops should be simplified with "LINQ" expressions
			foreach (IHtmlTableRowElement row in stockRows)
			{
				var symbol = row.Cells[0].QuerySelector("a")?.TextContent;
				if (symbol == null) continue;

				stocks.Add(new BvbStock
				{
					Symbol = symbol,
					Name = row.Cells[1].TextContent,
					Price = decimal.Parse(row.Cells[3].TextContent),
					Weight = decimal.Parse(row.Cells[7].TextContent) / 100
				});
			}
#pragma warning restore S3267 // Loops should be simplified with "LINQ" expressions

			return stocks.ToArray();
		}
	}
}
