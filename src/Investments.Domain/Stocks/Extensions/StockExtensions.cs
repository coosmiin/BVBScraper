using System.Collections.Generic;

namespace Investments.Domain.Stocks.Extensions
{
	public static class StockExtensions
	{
		public static StockWeights AsStockWeights(this IDictionary<string, decimal> weights)
		{
			return new StockWeights(weights);
		}

		public static StockPrices AsStockPrices(this IDictionary<string, decimal> weights)
		{
			return new StockPrices(weights);
		}
	}
}
