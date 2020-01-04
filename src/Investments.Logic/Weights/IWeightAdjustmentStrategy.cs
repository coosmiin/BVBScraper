using Investments.Domain.Stocks;

namespace Investments.Logic.Weights
{
	public interface IWeightAdjustmentStrategy
	{
		/// <summary>
		/// Adjusts weights according to the implementer strategy
		/// </summary>
		/// <param name="currentWeights">Current stock weights</param>
		/// <param name="targetWeights">Target stock weights</param>
		/// <param name="toBuyInverseRatio">The ratio between current stocks total value and to buy available amount</param>
		StockWeights AdjustWeights(StockWeights currentWeights, StockWeights targetWeights, decimal toBuyInverseRatio);
	}
}
