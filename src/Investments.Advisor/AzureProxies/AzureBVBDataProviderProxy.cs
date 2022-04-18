using Investments.Advisor.Exceptions;
using Investments.Advisor.Models;
using Investments.Advisor.Providers;
using Investments.Domain.Stocks;
using Investments.Utils.Linq.Extensions;
using Investments.Utils.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Investments.Advisor.AzureProxies
{
	public class AzureBvbDataProviderProxy : IBvbDataProvider
	{
		private const string FUNCTION_URI_FORMAT = "api/scrapeBvbIndex?code={0}&index=BET";

		private readonly HttpClient _httpClient;
		private readonly string _functionUri;

		public AzureBvbDataProviderProxy(HttpClient httpClient, string functionKey)
		{
			_httpClient = httpClient;
			_functionUri = string.Format(FUNCTION_URI_FORMAT, functionKey);
		}

		public async Task<Stock[]> GetBvbStocksAsync()
		{
			var result = await _httpClient.GetStringAsync(_functionUri);

			var indexStocks = JsonSerializerHelper.Deserialize<BvbStock[]>(result).OrEmpty();

			ThrowIfInvalid(indexStocks, result);

			return indexStocks.Select(stock => new Stock(stock.Symbol) { Price = stock.Price, Weight = stock.Weight }).ToArray();
		}

		private static void ThrowIfInvalid(IEnumerable<BvbStock> indexStocks, string rawResult)
		{
			if (!indexStocks.Any())
				throw new InvalidBvbDataException("'ScrapeBvbIndex' Azure Function returned 0 results");

			if (indexStocks.Any(stock => string.IsNullOrEmpty(stock.Symbol))
				|| indexStocks.Any(stock => stock.Price == 0)
				|| indexStocks.Any(stock => stock.Weight == 0))
				throw new InvalidBvbDataException($"'ScrapeBvbIndex' Azure Function returned invalid results or deserialization failed: {rawResult}");
		}
	}
}
