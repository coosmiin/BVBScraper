using Investments.Advisor.Trading;
using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using Investments.Domain.Trading;
using Investments.Utils.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Investments.Advisor.AzureProxies
{
	public class AzureTradeAutomationProxy : ITradeAutomation
	{
		private const string GET_PORTFOLIO_FUNCTION_URI_FORMAT = "api/getPortfolio?code={0}";
		private const string SUBMIT_ORDERS_FUNCTION_URI_FORMAT = "api/submitOrders?code={0}";
		
		private readonly HttpClient _httpClient;
		private readonly string _getPortfoliofunctionUrl;
		private readonly string _submitOrdersFunctionUrl;

		public AzureTradeAutomationProxy(HttpClient httpClient, string functionKey)
		{
			_httpClient = httpClient;
			_getPortfoliofunctionUrl = string.Format(GET_PORTFOLIO_FUNCTION_URI_FORMAT, functionKey);
			_submitOrdersFunctionUrl = string.Format(SUBMIT_ORDERS_FUNCTION_URI_FORMAT, functionKey);
		}

		public async Task<(Stock[], decimal)> GetPortfolio()
		{
			var result = await _httpClient.GetStringAsync(_getPortfoliofunctionUrl);

			var currentPortfolio = JsonSerializerHelper.Deserialize<TradePortfolio>(result);

			return (currentPortfolio.ExistingStocks.AsStocks(), currentPortfolio.AvailableAmount);
		}

		public async Task SubmitOrders(TradeOrder[] orders)
		{
			if (orders == null || !orders.Any())
				return;

			await _httpClient.PostAsync(
				_submitOrdersFunctionUrl, 
				new StringContent(JsonSerializer.Serialize(orders), Encoding.UTF8, "application/json"));
		}

		private class TradePortfolio
		{
			public IDictionary<string, int> ExistingStocks { get; set; } = new Dictionary<string, int>();

			public decimal AvailableAmount { get; set; }
		}
	}
}
