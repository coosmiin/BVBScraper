using Investments.Domain.Portfolios;
using Investments.Domain.Stocks;
using System.Linq;

namespace Investments.Logic.Portfolios
{
	public static class PortfolioExtensions
	{
		public static Stock[] DeriveToBuyStocks(this Portfolio portfolio, Stock[] stocks)
		{
			var initialStocks = stocks.ToDictionary(s => s.Symbol);

			return portfolio.Select(s => s - SafeGetStock(s.Symbol)).Where(s => s.Count > 0).ToArray();

			Stock? SafeGetStock(string symbol)
			{
				if (initialStocks.ContainsKey(symbol))
					return initialStocks[symbol];

				return null;
			}
		}
	}
}
