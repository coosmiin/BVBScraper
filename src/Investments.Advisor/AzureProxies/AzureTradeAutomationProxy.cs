using Investments.Advisor.Trading;
using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using Investments.Utils.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Investments.Advisor.AzureProxies
{
	public class AzureTradeAutomationProxy : ITradeAutomation
	{
		private const string FUNCTION_URI_FORMAT = "https://tradeautomation.azurewebsites.net/api/getPortfolio?code={0}";
		
		private readonly HttpClient _httpClient;
		private readonly string _functionUri;

		public AzureTradeAutomationProxy(HttpClient httpClient, string functionKey)
		{
			_httpClient = httpClient;
			_functionUri = string.Format(FUNCTION_URI_FORMAT, functionKey);
		}

		public async Task<(Stock[], decimal)> GetPortfolio()
		{
			var result = await _httpClient.GetStringAsync(_functionUri);

			var currentPortfolio = JsonSerializerHelper.Deserialize<TradePortfolio>(result);

			return (currentPortfolio.ExistingStocks.AsStocks(), currentPortfolio.AvailableAmount);
		}

		private class TradePortfolio
		{
			public IDictionary<string, int> ExistingStocks { get; set; } = new Dictionary<string, int>();

			public decimal AvailableAmount { get; set; }
		}
	}
}
