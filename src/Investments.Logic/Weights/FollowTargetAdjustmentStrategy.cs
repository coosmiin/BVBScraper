using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using System.Linq;

namespace Investments.Logic.Weights
{
	/// <summary>
	/// Implements the strategy that tries to bring the final (stocks) weights as close to the given target weights as possible
	/// </summary>
	public class FollowTargetAdjustmentStrategy : IWeightAdjustmentStrategy
	{
		public StockWeights AdjustWeights(StockWeights currentWeights, StockWeights targetWeights, decimal toBuyInverseRatio)
		{
			// Starting from: cn * C + bn * B = tn * T
			// , where cn - weight for stock 'n', C - total current stock value
			// , bn - to buy weight for stock 'n', B - to buy available amount
			// , tn - target weight for stock 'n', T - total final stock value
			// We get: bn = tn * (C / B + 1) - an * (C / B), where C / B is the to buy inverse ratio
			// With the constraint that bn >= 0 which results in: cn <= tn * (1 + B / C)

			// Apply the constraint first and remove the target weights that don't fulfill it
			var allowedWeights = targetWeights
				.Where(w => SafeGetCurrentWeight(w.Key) <= w.Value * (1 + 1 / toBuyInverseRatio))
				.AsStockWeights();

			decimal toRedistributedWeight = targetWeights.Sum(w => w.Value) - allowedWeights.Sum(w => w.Value);

			// Redistribute the removed weights to the other weights
			allowedWeights = allowedWeights.Select(w => (w.Key, w.Value * (1 + toRedistributedWeight))).AsStockWeights();

			return allowedWeights
				.Select(w => (w.Key, w.Value * (toBuyInverseRatio + 1) - SafeGetCurrentWeight(w.Key) * toBuyInverseRatio))
				.AsStockWeights();

			decimal SafeGetCurrentWeight(string symbol)
			{
				if (currentWeights.ContainsKey(symbol))
					return currentWeights[symbol];

				return 0;
			}
		}
	}
}
