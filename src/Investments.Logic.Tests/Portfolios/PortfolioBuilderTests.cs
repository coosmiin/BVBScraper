using Investments.Logic.Portfolios;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Investments.Logic.Tests.Portfolios
{
	public class PortfolioBuilderTests
	{
		[Test]
		public void BuildPortfolio_NotEnoughForAny_PortfolioValueIsZero()
		{
			var prices = new Dictionary<string, decimal>
			{ { "TLV", 10 }, { "FP", 20 } };

			var targetWeights = new Dictionary<string, decimal>
			{ { "TLV", 0.5m }, {  "FP", 0.5m } };

			var portfolio = new PortfolioBuilder()
				.UsePrices(prices)
				.UseWeights(targetWeights)
				.UseAmount(9)
				.Build();

			Assert.Zero(portfolio.TotalValue);
		}

		[Test]
		public void BuildPortfolio_NotEnoughForAll_PortfolioValueIsCorrect()
		{
			var prices = new Dictionary<string, decimal>
			{ { "TLV", 10 }, { "FP", 20 } };

			var targetWeights = new Dictionary<string, decimal>
			{ { "TLV", 0.5m }, {  "FP", 0.5m } };

			var portfolio = new PortfolioBuilder()
				.UsePrices(prices)
				.UseWeights(targetWeights)
				.UseAmount(20)
				.Build();

			Assert.AreEqual(10, portfolio.TotalValue);
		}

		[Test]
		public void BuildPortfolio_AlmostEnoughForAll_SmallestEligibleStockIsDecreased()
		{
			var prices = new Dictionary<string, decimal>
			{ { "TLV", 10 }, { "FP", 20 }, { "EL", 30 } };

			var targetWeights = new Dictionary<string, decimal>
			{ { "TLV", 0.2m }, { "FP", 0.3m }, { "EL", 0.5m } };

			var portfolio = new PortfolioBuilder()
				.UsePrices(prices)
				.UseWeights(targetWeights)
				.UseAmount(119)
				.Build();

			Assert.AreEqual(2, portfolio["EL"].Count);
			Assert.AreEqual(2, portfolio["FP"].Count);
			Assert.AreEqual(1, portfolio["TLV"].Count);

			Assert.AreEqual(110, portfolio.TotalValue);
		}

		[Test]
		public void BuildPortfolio_AtLeastOneTargetWeightAboveAboveOneHundredPercent_ThrowsArgumentException()
		{
			var prices = new Dictionary<string, decimal>
			{ { "TLV", 10 }, { "FP", 20 }, { "EL", 30 } };

			var targetWeights = new Dictionary<string, decimal>
			{ { "TLV", 1.2m }, { "FP", 0.3m }, { "EL", 0.5m } };

			var builder = new PortfolioBuilder()
				.UsePrices(prices)
				.UseWeights(targetWeights)
				.UseAmount(100);

			Assert.Throws<ArgumentException>(() => builder.Build());
		}

		[Test]
		public void BuildPortfolio_SumOfTargetWeightNotApproxEqualWithOneHundredPercent_ThrowsArgumentException()
		{
			var prices = new Dictionary<string, decimal>
			{ { "TLV", 10 }, { "FP", 20 }, { "EL", 30 } };

			var targetWeights = new Dictionary<string, decimal>
			{ { "TLV", 0.3m }, { "FP", 0.3m }, { "EL", 0.5m } };

			var builder = new PortfolioBuilder()
				.UsePrices(prices)
				.UseWeights(targetWeights)
				.UseAmount(100);

			Assert.Throws<ArgumentException>(() => builder.Build());
		}

		[Test]
		public void BuildPortfolio_SumOfTargetWeightApproxEqualWithOneHundredPercent_DoesNotThrow()
		{
			var prices = new Dictionary<string, decimal>
			{ { "TLV", 10 }, { "FP", 20 }, { "EL", 30 } };

			var targetWeights = new Dictionary<string, decimal>
			{ { "TLV", 0.21m }, { "FP", 0.3m }, { "EL", 0.5m } };

			var builder = new PortfolioBuilder()
				.UsePrices(prices)
				.UseWeights(targetWeights)
				.UseAmount(100);

			Assert.DoesNotThrow(() => builder.Build());
		}
	}
}