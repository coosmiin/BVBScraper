using System.IO;
using System.Linq;
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
			var (currentStocks, bvbStocks, portfolioValue) = ReadStocks("BET");

			return (currentStocks.AsStockWeights(), bvbStocks.AsStockWeights(), portfolioValue);
		}

		internal static (Stock[] currentStocks, Stock[] bvbStocks, decimal portfolioValue) ReadStocks(string index)
		{
			var bvbStocks = JsonSerializerHelper.Deserialize<Stock[]>(File.ReadAllText($@"TestData/{index}-index.json"));
			var currentStocks = JsonSerializerHelper.Deserialize<Stock[]>(File.ReadAllText(@"TestData/portfolio.json"));

			var bvbStocksSet = bvbStocks.Select(stock => stock.Symbol).ToHashSet();

			currentStocks = currentStocks
				.Where(stock => bvbStocksSet.Contains(stock.Symbol))
				.ToArray()
				.UpdatePrices(bvbStocks.AsStockPrices());

			var currentPortfolio = new Portfolio(currentStocks);

			return (currentStocks, bvbStocks, currentPortfolio.TotalValue);
		}
	}
}
