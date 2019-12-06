using Investments.Domain.Stocks;
using NUnit.Framework;
using System;

namespace Investments.Domain.Tests.Stocks
{
	public class StockTests
	{
		[Test]
		public void Addition_DifferentSymbols_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>(() => { _ = new Stock("TLV") + new Stock("FP"); });
		}

		[Test]
		public void Addition_StockCountsAreCorrectlyAdded()
		{
			Assert.AreEqual(7, (new Stock("FP") { Count = 5 } + new Stock("FP") { Count = 2 }).Count);
		}

		[Test]
		public void Addition_PricesAreDifferent_SecondPriceIsLeading()
		{
			Assert.AreEqual(2, (new Stock("FP") { Price = 5 } + new Stock("FP") { Price = 2 }).Price);
		}

		[Test]
		public void Addition_WeightIsReset()
		{
			Assert.AreEqual(0, (new Stock("FP") { Weight = 0.2m } + new Stock("FP") { Weight = 0.3m }).Weight);
		}

		[Test]
		public void UnarySubstraction_StockCountIsDecreasedByOne()
		{
			var stock = new Stock("FP") { Count = 3 };
			Assert.AreEqual(2, --stock.Count);
		}

	}
}
