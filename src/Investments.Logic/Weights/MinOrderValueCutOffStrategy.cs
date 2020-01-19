using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using System;
using System.Linq;

namespace Investments.Logic.Weights
{
	/// <summary>
	/// Given an internal strategy, it applies it sequentially while cutting off all weights below a minimal value. 
	/// The total weight that is cut off is then redistributed to the remaining weights
	/// </summary>
	public class MinOrderValueCutOffStrategy : IWeightAdjustmentStrategy
	{
		private readonly IWeightAdjustmentStrategy _innerStrategy;
		private readonly decimal _minWeight;

		public MinOrderValueCutOffStrategy(IWeightAdjustmentStrategy innerStrategy, decimal minWeight)
		{
			_innerStrategy = innerStrategy ?? throw new ArgumentNullException(nameof(innerStrategy));
			_minWeight = minWeight;
		}

		public StockWeights AdjustWeights(StockWeights currentWeights, StockWeights targetWeights, decimal toBuyInverseRatio)
		{
			// Adjust weights based on initial strategy
			var adjustedWeights = _innerStrategy.AdjustWeights(currentWeights, targetWeights, toBuyInverseRatio);

			// Cut off those that are too small
			targetWeights = adjustedWeights.Where(w => w.Value > _minWeight).AsStockWeights();

			// Calculate total weight to be redistributed
			decimal cutOffTotalWeight = adjustedWeights.Where(w => w.Value <= _minWeight).Sum(w => w.Value);

			// Redistributes deleted weights
			targetWeights = targetWeights.Select(w => (w.Key, w.Value * (1 + cutOffTotalWeight))).AsStockWeights();

			return targetWeights;
		}
	}
}
