using Investments.Domain.Stocks;
using Investments.Logic.Weights;
using NUnit.Framework;

namespace Trading.IntegrationTests.Logic
{
	public class WeightAdjustmentStrategyTests
	{
		[Test]
		public void FollowTargetAdjustmentStrategy_AsExpected()
		{
			var strategy = new FollowTargetAdjustmentStrategy();

			var currentWeights = new StockWeights
			{
				{ "TLV", 0.2m }, 
				{ "FP", 0.2m }, 
				{ "EL", 0.6m } 
			};

			var targetWeights = new StockWeights
			{
				{ "TLV", 0.2m },
				{ "FP", 0.3m }, 
				{ "EL", 0.5m }
			};

			currentWeights = strategy.AdjustWeights(currentWeights, targetWeights, 0 / 2000);
		}
	}
}