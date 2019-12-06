using Investments.Domain.Stocks;

namespace Investments.Logic.Weights
{
	/// <summary>
	/// The "default" implementation of the <see cref="IWeightAdjustmentStrategy"/>. Weights are not touched.
	/// </summary>
	public class NoWeightAdjustmentStrategy : IWeightAdjustmentStrategy
	{
		public StockWeights AdjustWeights(StockWeights currentWeights, StockWeights targetWeights, decimal toBuyInverseRatio)
		{
			return targetWeights;
		}
	}
}
