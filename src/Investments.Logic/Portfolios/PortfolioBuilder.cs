using System;
using System.Collections.Generic;
using System.Linq;
using Investments.Domain.Models;
using Investments.Logic.Calculus;

namespace Investments.Logic.Portfolios
{
    public class PortfolioBuilder
	{
		private Dictionary<string, decimal> _stockPrices;
		private Dictionary<string, decimal> _targetWeights;
		private decimal _availableAmount;

		public Portfolio Build()
		{
			Validate();
			var portfolio = new Portfolio();
			foreach (var weight in _targetWeights.OrderByDescending(s => s.Value))
			{
				var price = _stockPrices[weight.Key];
				var count = (int)Math.Round(weight.Value * _availableAmount / price, 0);

				TryAddStock(portfolio, new Stock(weight.Key) { Count = count, Price = price }, _availableAmount);
			}

			return portfolio;
		}

        public PortfolioBuilder UsePrices(Dictionary<string, decimal> stockPrices)
		{
			_stockPrices = stockPrices;
			return this;
		}

		public PortfolioBuilder UseWeights(Dictionary<string, decimal> targetWeights)
		{
			_targetWeights = targetWeights;
			return this;
		}

		public PortfolioBuilder UseAmount(decimal availableAmount)
		{
			_availableAmount = availableAmount;
			return this;
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

        private void Validate()
        {
            if (_availableAmount <= 0)
				throw new ArgumentException($"Available amount ({_availableAmount}) needs to be higher than 0");

			if (_targetWeights.Any(w => w.Value > 1))
				throw new ArgumentException($"Target weights cannot be higher than 100% ({_targetWeights.First(w => w.Value > 1).Key} weight: {_targetWeights.First(w => w.Value > 1)})");			

			if (!MathHelper.IsApproxOne(_targetWeights.Sum(w => w.Value)))
				throw new ArgumentException($"Sum of target weights has to be approx 100% ({_targetWeights.Sum(w => w.Value)})");			
        }
	}
}