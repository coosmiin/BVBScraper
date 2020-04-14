using Investments.Domain.Portfolios;
using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using Investments.Utils.Serialization;
using System.Collections.Generic;
using Trading.IntegrationTests.Properties;

namespace Trading.IntegrationTests.TestData
{
	public static class TestResources
	{
		internal static (StockWeights currentWeights, StockWeights targetWeights, decimal portfolioValue) ReadWeights()
		{
			var (currentStocks, bvbStocks, portfolioValue) = ReadStocks();

			return (currentStocks.AsStockWeights(), bvbStocks.AsStockWeights(), portfolioValue);
		}

		internal static (Stock[] currentStocks, Stock[] bvbStocks, decimal portfolioValue) ReadStocks()
		{
			var bvbStocks = JsonSerializerHelper.Deserialize<Stock[]>(Resources.bvb_index);
			var currentStocks = JsonSerializerHelper.Deserialize<Dictionary<string, int>>(Resources.portfolio).AsStocks();

			currentStocks = currentStocks.UpdatePrices(bvbStocks.AsStockPrices());

			var currentPortfolio = new Portfolio(currentStocks);

			return (currentStocks, bvbStocks, currentPortfolio.TotalValue);
		}
	}
}
