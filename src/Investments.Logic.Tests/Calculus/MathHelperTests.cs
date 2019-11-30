using Investments.Logic.Calculus;
using NUnit.Framework;

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
    }
}