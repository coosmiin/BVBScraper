using AngleSharp;
using AngleSharp.Html.Dom;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BVBScraper
{
	public class StockScraper
	{
		private const string BET_INDEX_COMPOSITION_URL = "http://www.bvb.ro/FinancialInstruments/Indices/IndicesProfiles";

		private readonly IBrowsingContext _browsingContext;

		public StockScraper(IBrowsingContext browsingContext = null)
		{
			_browsingContext = browsingContext ?? BrowsingContext.New(Configuration.Default.WithDefaultLoader());
		}

		public async Task<IEnumerable<BETStock>> ScrapeBETComposition()
		{
			var document = await _browsingContext.OpenAsync(BET_INDEX_COMPOSITION_URL);

			var stockRows = document.QuerySelectorAll("table.generic-table tbody tr");

			var stocks = new List<BETStock>();

			foreach (IHtmlTableRowElement row in stockRows)
			{
				stocks.Add(new BETStock
				{
					Symbol = row.Cells[0].QuerySelector("a").TextContent,
					Name = row.Cells[1].TextContent,
					Price = decimal.Parse(row.Cells[3].TextContent),
					Weight = decimal.Parse(row.Cells[7].TextContent)
				});
			}

			return stocks.ToArray();
		}
	}
}
