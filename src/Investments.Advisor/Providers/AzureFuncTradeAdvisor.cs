using Investments.Domain.Stocks;
using Investments.Utils.Serialization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Investments.Advisor.Providers
{
	public class AzureFuncTradeAdvisor : ITradeAdvisor
	{
		private const string FUNCTION_URI_FORMAT = "https://tradeorchestration.azurewebsites.net/api/calculateToBuyStocks?code={0}";

		private readonly HttpClient _httpClient;
		private readonly string _functionUri;

		public AzureFuncTradeAdvisor(HttpClient httpClient, string functionKey)
		{
			_httpClient = httpClient;
			_functionUri = string.Format(FUNCTION_URI_FORMAT, functionKey);
		}

		public async Task<Stock[]> CalculateToBuyStocksAsync(decimal toBuyAmount, Stock[] existingStocks, Stock[] betStocks)
		{
			var payload = new
			{
				ToBuyAmount = toBuyAmount,
				ExistingStocks = existingStocks,
				BETStocks = betStocks
			};

			var result = 
				await _httpClient.PostAsync(
					_functionUri, 
					new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

			return JsonSerializerHelper.Deserialize<Stock[]>(await result.Content.ReadAsStringAsync());
		}
	}
}
