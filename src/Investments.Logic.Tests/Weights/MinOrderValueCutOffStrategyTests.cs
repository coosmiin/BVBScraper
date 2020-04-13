using Investments.Domain.Stocks;
using Investments.Logic.Weights;
using NUnit.Framework;

namespace Investments.Logic.Tests.Weights
{
	public class MinOrderValueCutOffStrategyTests
	{
		[Test]
		public void AdjustWeights_WeightsBellowMinimalWeightAreCutOff_CutOffWeightIsRedistributed()
		{
			var currentWeights = new StockWeights
			{ { "TLV", 0.1m }, { "SNG", 0.1m }, { "FP", 0.2m }, { "EL", 0.6m } };

			var targetWeights = new StockWeights
			{ { "TLV", 0.1m }, { "SNG", 0.1m }, { "FP", 0.2m }, { "EL", 0.6m } };

			var strategy = new MinOrderValueCutOffStrategy(new FollowTargetAdjustmentStrategy(), 0.1m);
			var toBuyWeights = strategy.AdjustWeights(currentWeights, targetWeights, toBuyInverseRatio: 2);

			Assert.IsFalse(toBuyWeights.ContainsKey("TLV"));
			Assert.IsFalse(toBuyWeights.ContainsKey("SNG"));
			Assert.AreEqual(0.25m, toBuyWeights["FP"]);
			Assert.AreEqual(0.75m, toBuyWeights["EL"]);
		}
	}
}
