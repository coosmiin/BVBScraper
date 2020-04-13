using Investments.Domain.Stocks;
using Investments.Logic.Calculus;
using NUnit.Framework;
using System.Linq;

namespace Investments.Logic.Tests.Calculus
{
	public class MathHelperTests
	{
		[Test]
		public void IsApproxOne_CloseEnough_ReturnsTrue()
		{
			Assert.IsTrue(MathHelper.IsApproxOne(1.02m));
			Assert.IsTrue(MathHelper.IsApproxOne(0.98m));
		}

		[Test]
		public void IsApproxOne_NotCloseEnough_ReturnsFalse()
		{
			Assert.IsFalse(MathHelper.IsApproxOne(1.12m));
			Assert.IsFalse(MathHelper.IsApproxOne(0.88m));
		}

		[Test]
		public void Redistribute_TotalWeightAlreadyOne_WeightsRemainUnchanged()
		{
			var weights = new StockWeights
			{
				{ "TLV", 0.3m }, { "FP", 0.6m }, { "EL", 0.1m }
			};

			weights = weights.Redistribute();

			Assert.AreEqual(weights["TLV"], 0.3);
			Assert.AreEqual(weights["FP"], 0.6);
			Assert.AreEqual(weights["EL"], 0.1);
		}

		[Test]
		public void Redistribute_TotalWeightLessThanOne_RedistributedWeightIsAprroxOne()
		{
			var weights = new StockWeights
			{
				{ "TLV", 0.1m }, { "FP", 0.2m }, { "EL", 0.3m }
			};

			weights = weights.Redistribute();

			Assert.IsTrue(weights.Sum(w => w.Value).IsApproxOne());
		}
	}
}