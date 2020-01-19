using System;
using System.Collections.Generic;
using System.Linq;

namespace Investments.Domain.Stocks.Extensions
{
	public static class StockExtensions
	{
		public static StockWeights AsStockWeights(this IDictionary<string, decimal> weights)
		{
			return new StockWeights(weights);
		}

		public static StockWeights AsStockWeights(this IEnumerable<KeyValuePair<string, decimal>> weights)
		{
			return new StockWeights(weights.ToDictionary(w => w.Key, w => w.Value));
		}

		public static StockWeights AsStockWeights(this IEnumerable<(string Symbol, decimal Weight)> weights)
		{
			return new StockWeights(weights.ToDictionary(w => w.Symbol, w => w.Weight));
		}

		public static StockWeights AsStockWeights(this IEnumerable<Stock> stocks)
		{
			return new StockWeights(stocks.ToDictionary(s => s.Symbol, s => s.Weight));
		}

		public static StockPrices AsStockPrices(this IDictionary<string, decimal> prices)
		{
			return new StockPrices(prices);
		}

		public static Stock[] UpdatePrices(this Stock[] stocks, StockPrices prices)
		{
			if (stocks.Any(s => !prices.ContainsKey(s.Symbol)))
				throw new ArgumentException($"There are stocks without prices. First one: {stocks.First(s => !prices.ContainsKey(s.Symbol)).Symbol}");

			return stocks.Select(s => new Stock(s.Symbol) { Price = prices[s.Symbol], Count = s.Count }).ToArray();
		}
	}
}
