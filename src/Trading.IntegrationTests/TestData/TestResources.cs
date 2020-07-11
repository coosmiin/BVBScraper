using Investments.Domain.Portfolios;
using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using Investments.Utils.Serialization;
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
			var bvbStocks = JsonSerializerHelper.Deserialize<Stock[]>(Resources.bet_index);
			var currentStocks = JsonSerializerHelper.Deserialize<Stock[]>(Resources.portfolio);

			currentStocks = currentStocks.UpdatePrices(bvbStocks.AsStockPrices());

			var currentPortfolio = new Portfolio(currentStocks);

			return (currentStocks, bvbStocks, currentPortfolio.TotalValue);
		}
	}
}
