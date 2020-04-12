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
			// We get: bn = tn * (C / B + 1) - cn * (C / B), where C / B is the to buy inverse ratio
			// With first constraint that bn >= 0 which results in: cn <= tn * (1 + B / C)
			// With second constraint that b1 + b2 + ... + bn = 1

			// Apply the first contraint and remove the target weights that don't fulfill it
			var allowedWeights = targetWeights
				.Where(w => SafeGetCurrentWeight(w.Key) <= w.Value * (1 + 1 / toBuyInverseRatio))
				.AsStockWeights();

			decimal toRedistributedWeight = targetWeights.Sum(w => w.Value) - allowedWeights.Sum(w => w.Value);

			// Redistribute the removed weights to the other weights
			allowedWeights = allowedWeights.Select(w => (w.Key, w.Value * (1 + toRedistributedWeight))).AsStockWeights();

			var toBuyWeights = allowedWeights
				.Select(w => (w.Key, w.Value * (toBuyInverseRatio + 1) - SafeGetCurrentWeight(w.Key) * toBuyInverseRatio))
				.AsStockWeights();

			// Enforce the second constraint
			return toBuyWeights.Select(w => (w.Key, w.Value / toBuyWeights.Sum(w => w.Value))).AsStockWeights();

			decimal SafeGetCurrentWeight(string symbol)
			{
				if (currentWeights.ContainsKey(symbol))
					return currentWeights[symbol];

				return 0;
			}
		}
	}
}
