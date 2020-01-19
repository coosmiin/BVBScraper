using Investments.Domain.Portfolios;
using Investments.Domain.Stocks;
using System.Linq;

namespace Investments.Logic.Portfolios
{
	public static class PortfolioExtensions
	{
		public static Portfolio DeriveAdditionalPortfolio(this Portfolio portfolio, Stock[] stocks)
		{
			var initialStocks = stocks.ToDictionary(s => s.Symbol);

			return new Portfolio(portfolio.Select(s => s - SafeGetStock(s.Symbol)).Where(s => s.Count > 0).ToArray());

			Stock SafeGetStock(string symbol)
			{
				if (initialStocks.ContainsKey(symbol))
					return initialStocks[symbol];

				return null;
			}
		}
	}
}
