using System.IO;
using Investments.Domain.Portfolios;
using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using Investments.Utils.Serialization;

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
			var bvbStocks = JsonSerializerHelper.Deserialize<Stock[]>(File.ReadAllText(@"TestData/bet-index.json"));
			var currentStocks = JsonSerializerHelper.Deserialize<Stock[]>(File.ReadAllText(@"TestData/portfolio.json"));

			currentStocks = currentStocks.UpdatePrices(bvbStocks.AsStockPrices());

			var currentPortfolio = new Portfolio(currentStocks);

			return (currentStocks, bvbStocks, currentPortfolio.TotalValue);
		}
	}
}
