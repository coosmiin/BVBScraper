using Investments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Investments.Math.Calculator
{
	public class PortfolioCalculator
	{
		private readonly Dictionary<string, decimal> _stockPrices;

		/// <summary>
		/// Creates a new <see cref="PortfolioCalculator" />
		/// </summary>
		/// <param name="stockPrices">Stock prices snapshot</param>
		public PortfolioCalculator(Dictionary<string, decimal> stockPrices)
		{
			_stockPrices = stockPrices ?? throw new ArgumentNullException(nameof(stockPrices));
		}

		public Portfolio BuildPortfolio(decimal availableAmount, Dictionary<string, decimal> targetWeights)
		{
			var portfolio = new Portfolio();
			foreach (var weight in targetWeights.OrderByDescending(s => s.Value))
			{
				var price = _stockPrices[weight.Key];
				var count = (int)System.Math.Round(weight.Value * availableAmount / price, 0);

				TryAddStock(portfolio, new Stock(weight.Key) { Count = count, Price = price }, availableAmount);
			}

			return portfolio;
		}

		private void TryAddStock(Portfolio portfolio, Stock stock, decimal availableAmount)
		{
			if (stock.Count == 0)
				return;

			var currentAvailableAmount = availableAmount - portfolio.TotalValue - stock.TotalValue;
			if (currentAvailableAmount >= 0)
			{
				portfolio.AddStock(stock);
			}
			else 
			{
				TryAddStock(portfolio, stock--, availableAmount);
			}
		}
	}
}