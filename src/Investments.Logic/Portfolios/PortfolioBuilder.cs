using System;
using System.Linq;
using Investments.Domain.Portfolios;
using Investments.Domain.Stocks;
using Investments.Logic.Calculus;
using Investments.Logic.Weights;

namespace Investments.Logic.Portfolios
{
	public class PortfolioBuilder
	{
		private IWeightAdjustmentStrategy _weightStrategy;
		private StockPrices _stockPrices;
		private StockWeights _targetWeights;
		private Stock[] _stocks;
		private decimal _toBuyAmount;

		public Portfolio Build()
		{
			Validate();

			var portfolio = new Portfolio(_stocks);

			_weightStrategy ??= new NoWeightAdjustmentStrategy();
			_targetWeights = _weightStrategy.AdjustWeights(portfolio.StockWeights, _targetWeights, portfolio.TotalValue / _toBuyAmount);

			var availableAmount = portfolio.TotalValue + _toBuyAmount;
			foreach (var weight in _targetWeights.OrderByDescending(s => s.Value))
			{
				var price = _stockPrices[weight.Key];
				var count = (int)Math.Round(weight.Value * _toBuyAmount / price, 0);

				TryAddStock(portfolio, new Stock(weight.Key) { Count = count, Price = price }, availableAmount);
			}

			return portfolio;
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
			if (_toBuyAmount <= 0)
				throw new ArgumentException($"Available amount ({_toBuyAmount}) needs to be higher than 0");

			if (_targetWeights.Any(w => w.Value > 1))
				throw new ArgumentException($"Target weights cannot be higher than 100% ({_targetWeights.First(w => w.Value > 1).Key} weight: {_targetWeights.First(w => w.Value > 1)})");			

			if (!MathHelper.IsApproxOne(_targetWeights.Sum(w => w.Value)))
				throw new ArgumentException($"Sum of target weights has to be approx 100% ({_targetWeights.Sum(w => w.Value)})");			
		}
	}
}