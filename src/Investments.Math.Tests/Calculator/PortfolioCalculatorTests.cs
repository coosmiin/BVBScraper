using Investments.Math.Calculator;
using NUnit.Framework;
using System.Collections.Generic;

namespace Investments.Math.Tests.Calculator
{
	public class PortfolioCalculatorTests
	{
		[Test]
		public void BuildPortfolio_NotEnoughForAny_PortfolioValueIsZero()
		{
			var prices = new Dictionary<string, decimal>
			{ { "TLV", 10 }, { "FP", 20 } };

			var targetWeights = new Dictionary<string, decimal>
			{ { "TLV", 0.5m }, {  "FP", 0.5m } };

			var portfolio = new PortfolioCalculator(prices).BuildPortfolio(9, targetWeights);

			Assert.Zero(portfolio.TotalValue);
		}

		[Test]
		public void BuildPortfolio_NotEnoughForAll_PortfolioValueIsCorrect()
		{
			var prices = new Dictionary<string, decimal>
			{ { "TLV", 10 }, { "FP", 20 } };

			var targetWeights = new Dictionary<string, decimal>
			{ { "TLV", 0.5m }, {  "FP", 0.5m } };

			var portfolio = new PortfolioCalculator(prices).BuildPortfolio(20, targetWeights);

			Assert.AreEqual(10, portfolio.TotalValue);
		}

		[Test]
		public void BuildPortfolio_AlmostEnoughForAll_SmallestEligibleStockIsDecreased()
		{
			var prices = new Dictionary<string, decimal>
			{ { "TLV", 10 }, { "FP", 20 }, { "EL", 30 } };

			var targetWeights = new Dictionary<string, decimal>
			{ { "TLV", 0.2m }, {  "FP", 0.3m }, { "EL", 0.5m } };

			var portfolio = new PortfolioCalculator(prices).BuildPortfolio(119, targetWeights);

			Assert.AreEqual(2, portfolio["EL"].Count);
			Assert.AreEqual(2, portfolio["FP"].Count);
			Assert.AreEqual(1, portfolio["TLV"].Count);

			Assert.AreEqual(110, portfolio.TotalValue);
		}
	}
}
