using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using System;
using System.Linq;

namespace Investments.Logic.Weights
{
	/// <summary>
	/// Imlements the strategy that tries to bring the final (stocks) weights as close to the given target weights as possible
	/// </summary>
	public class FollowTargetAdjustmentStrategy : IWeightAdjustmentStrategy
	{
		public StockWeights AdjustWeights(StockWeights currentWeights, StockWeights targetWeights, decimal toBuyInverseRatio)
		{
			if (currentWeights.Any(w => !targetWeights.ContainsKey(w.Key)))
				throw new ArgumentException($"Target weights should cover all current weights. Symbol `{currentWeights.First(w => !targetWeights.ContainsKey(w.Key)).Key}` not found in target weights");

			// Applies the following formula: 
			// Starting from: cn * C + bn * B = tn * T
			// , where cn - weight for stock 'n', C - total current stock value
			// , bn - to buy weight for stock 'n', B - to buy available amount
			// , tn - target weight for stock 'n', T - total final stock value
			// We get: bn = tn * (A / B + 1) - an * (A / B), where A / B is the to buy inverse ratio
			return targetWeights
				.Select(t => new { Symbol = t.Key, Weight = t.Value * (toBuyInverseRatio + 1) - SafeGetCurrentWeight(t.Key) * toBuyInverseRatio })
				.ToDictionary(t => t.Symbol, t => t.Weight)
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
