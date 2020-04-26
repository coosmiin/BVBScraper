using Investments.Advisor.Exceptions;
using Investments.Advisor.Models;
using Investments.Advisor.Providers;
using Investments.Domain.Stocks;
using Investments.Utils.Serialization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Investments.Advisor.AzureProxies
{
	public class AzureBVBDataProviderProxy : IBVBDataProvider
	{
		private const string FUNCTION_URI_FORMAT = "api/scrapeBETIndex?code={0}";

		private readonly HttpClient _httpClient;
		private readonly string _functionUri;

		public AzureBVBDataProviderProxy(HttpClient httpClient, string functionKey)
		{
			_httpClient = httpClient;
			_functionUri = string.Format(FUNCTION_URI_FORMAT, functionKey);
		}

		public async Task<Stock[]> GetBETStocksAsync()
		{
			var result = await _httpClient.GetStringAsync(_functionUri);

			var indexStocks = JsonSerializerHelper.Deserialize<BETStock[]>(result);

			ThrowIfInvalid(indexStocks, result);

			return indexStocks.Select(s => new Stock(s.Symbol) { Price = s.Price, Weight = s.Weight }).ToArray();
		}

		private void ThrowIfInvalid(BETStock[] indexStocks, string rawResult)
		{
			if (indexStocks.Length == 0)
				throw new InvalidBETDataException("'ScrapeBETIndex' Azure Function returned 0 results");

			if (indexStocks.Any(s => string.IsNullOrEmpty(s.Symbol))
				|| indexStocks.Any(s => s.Price == 0)
				|| indexStocks.Any(s => s.Weight == 0))
				throw new InvalidBETDataException($"'ScrapeBETIndex' Azure Function returned invalid results or deserialization failed: {rawResult}");
		}
	}
}
