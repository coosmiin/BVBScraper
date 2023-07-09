using System;
using System.Collections.Generic;
using System.Linq;
using Investments.Domain.Portfolios;
using Investments.Domain.Stocks;
using Investments.Logic.Calculus;
using Investments.Logic.Weights;

namespace Investments.Logic.Portfolios
{
	public class PortfolioBuilder
	{
		private IWeightAdjustmentStrategy? _weightStrategy;
		private StockPrices _stockPrices = new();
		private StockWeights _targetWeights = new();
		private Stock[]? _stocks;
		private decimal _toBuyAmount;
		private int _minOrderValue;

		public Portfolio Build()
		{
			Validate();

			var initialPortfolio = GetInitialPortfolio();

			RecalculateWeights(initialPortfolio);

			return  BuildPortfolio(initialPortfolio);

			Portfolio GetInitialPortfolio()
			{
				// Update prices on existing stocks
				foreach (var stock in _stocks ?? Enumerable.Empty<Stock>())
				{
					stock.Price = _stockPrices[stock.Symbol];
				}

				return new Portfolio(_stocks);
			}

			void RecalculateWeights(Portfolio portfolio)
			{
				_weightStrategy ??= new NoWeightAdjustmentStrategy();
				_targetWeights = 
					_weightStrategy.AdjustWeights(
						portfolio.StockWeights, 
						_targetWeights, 
						portfolio.TotalValue / _toBuyAmount);
			}

			Portfolio BuildPortfolio(Portfolio portfolio)
			{
				var availableAmount = portfolio.TotalValue + _toBuyAmount;
				foreach (var weight in _targetWeights.OrderByDescending(s => s.Value))
				{
					var price = _stockPrices[weight.Key];
					var count = (int)Math.Round(weight.Value * _toBuyAmount / price, 0);

					TryAddStock(portfolio, new Stock(weight.Key) { Count = count, Price = price }, availableAmount);
				}

				return portfolio;
			}
		}

		public PortfolioBuilder UsePrices(StockPrices stockPrices)
		{
			_stockPrices = stockPrices;
			return this;
		}

		public PortfolioBuilder UseStocks(Stock[] stocks)
		{
			_stocks = stocks;
			return this;
		}

		public PortfolioBuilder UseTargetWeights(StockWeights targetWeights)
		{
			_targetWeights = targetWeights;
			return this;
		}

		public PortfolioBuilder UseToBuyAmount(decimal toBuyAmount)
		{
			_toBuyAmount = toBuyAmount;
			return this;
		}

		public PortfolioBuilder UseWeightAdjustmentStrategy(IWeightAdjustmentStrategy strategy)
		{
			_weightStrategy = strategy;
			return this;
		}

		public PortfolioBuilder UseMinOrderValue(int value)
		{
			_minOrderValue = value;
			return this;
		}

		private void TryAddStock(Portfolio portfolio, Stock stock, decimal availableAmount)
		{
			if (stock.Count == 0)
				return;

			if (stock.TotalValue < _minOrderValue)
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
			if (_toBuyAmount <= 0)
				throw new ArgumentException($"Available amount ({_toBuyAmount}) needs to be higher than 0");

			var invalidTargetWeight = _targetWeights.FirstOrDefault(w => w.Value > 1);
			if (!invalidTargetWeight.Equals(default(KeyValuePair<string, decimal>)))
				throw new ArgumentException($"Target weights cannot be higher than 100% ({invalidTargetWeight.Key} weight: {invalidTargetWeight.Value * 100}% )");

			if (!MathHelper.IsApproxOne(_targetWeights.Sum(w => w.Value)))
				throw new ArgumentException($"Sum of target weights has to be approx 100% ({_targetWeights.Sum(w => w.Value)})");

			if (_stockPrices == null)
				throw new ArgumentException("Cannot build portfolio without stock prices");

			if (_targetWeights.Any(w => !_stockPrices.ContainsKey(w.Key)))
				throw new ArgumentException($"There is at least one target symbol that doesn't have a price: `{_targetWeights.First(w => !_stockPrices.ContainsKey(w.Key)).Key}`");

			if (_stocks?.Any(s => !_stockPrices.ContainsKey(s.Symbol)) == true)
				throw new ArgumentException($"There is at least one stock that doesn't have equivalent price: `{_stocks.First(s => !_stockPrices.ContainsKey(s.Symbol)).Symbol}`");
		}
	}
}