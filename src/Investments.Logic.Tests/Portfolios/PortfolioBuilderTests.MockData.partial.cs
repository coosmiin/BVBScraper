using Investments.Domain.Stocks;
using Investments.Logic.Portfolios;
using Investments.Logic.Weights;
using NUnit.Framework;
using System;

namespace Investments.Logic.Tests.Portfolios
{
	public partial class PortfolioBuilderTests
	{
		[Test]
		public void BuildPortfolio_NotEnoughForAny_PortfolioValueIsZero()
		{
			var prices = new StockPrices
			{ { "TLV", 10 }, { "FP", 20 } };

			var targetWeights = new StockWeights
			{ { "TLV", 0.5m }, {  "FP", 0.5m } };

			var portfolio = new PortfolioBuilder()
				.UsePrices(prices)
				.UseTargetWeights(targetWeights)
				.UseToBuyAmount(9)
				.Build();

			Assert.Zero(portfolio.TotalValue);
		}

		[Test]
		public void BuildPortfolio_NotEnoughForAll_PortfolioValueIsCorrect()
		{
			var prices = new StockPrices
			{ { "TLV", 10 }, { "FP", 20 } };

			var targetWeights = new StockWeights
			{ { "TLV", 0.5m }, {  "FP", 0.5m } };

			var portfolio = new PortfolioBuilder()
				.UsePrices(prices)
				.UseTargetWeights(targetWeights)
				.UseToBuyAmount(20)
				.Build();

			Assert.AreEqual(10, portfolio.TotalValue);
		}

		[Test]
		public void BuildPortfolio_AlmostEnoughForAll_SmallestEligibleStockIsDecreased()
		{
			var prices = new StockPrices
			{ { "TLV", 10 }, { "FP", 20 }, { "EL", 30 } };

			var targetWeights = new StockWeights
			{ { "TLV", 0.2m }, { "FP", 0.3m }, { "EL", 0.5m } };

			var portfolio = new PortfolioBuilder()
				.UsePrices(prices)
				.UseTargetWeights(targetWeights)
				.UseToBuyAmount(119)
				.Build();

			Assert.AreEqual(2, portfolio["EL"].Count);
			Assert.AreEqual(2, portfolio["FP"].Count);
			Assert.AreEqual(1, portfolio["TLV"].Count);

			Assert.AreEqual(110, portfolio.TotalValue);
		}

		[Test]
		public void BuildPortfolio_AtLeastOneTargetWeightAboveAboveOneHundredPercent_ThrowsArgumentException()
		{
			var prices = new StockPrices
			{ { "TLV", 10 }, { "FP", 20 }, { "EL", 30 } };

			var targetWeights = new StockWeights
			{ { "TLV", 1.2m }, { "FP", 0.3m }, { "EL", 0.5m } };

			var builder = new PortfolioBuilder()
				.UsePrices(prices)
				.UseTargetWeights(targetWeights)
				.UseToBuyAmount(100);

			Assert.Throws<ArgumentException>(() => builder.Build());
		}

		[Test]
		public void BuildPortfolio_SumOfTargetWeightNotApproxEqualWithOneHundredPercent_ThrowsArgumentException()
		{
			var prices = new StockPrices
			{ { "TLV", 10 }, { "FP", 20 }, { "EL", 30 } };

			var targetWeights = new StockWeights
			{ { "TLV", 0.3m }, { "FP", 0.3m }, { "EL", 0.5m } };

			var builder = new PortfolioBuilder()
				.UsePrices(prices)
				.UseTargetWeights(targetWeights)
				.UseToBuyAmount(100);

			Assert.Throws<ArgumentException>(() => builder.Build());
		}

		[Test]
		public void BuildPortfolio_SumOfTargetWeightApproxEqualWithOneHundredPercent_DoesNotThrow()
		{
			var prices = new StockPrices
			{ { "TLV", 10 }, { "FP", 20 }, { "EL", 30 } };

			var targetWeights = new StockWeights
			{ { "TLV", 0.21m }, { "FP", 0.3m }, { "EL", 0.5m } };

			var builder = new PortfolioBuilder()
				.UsePrices(prices)
				.UseTargetWeights(targetWeights)
				.UseToBuyAmount(100);

			Assert.DoesNotThrow(() => builder.Build());
		}

		[Test]
		public void BuildPortfolio_InitialStocks_EnoughAvailableAmount_NewStocksAreAdded()
		{
			var prices = new StockPrices
			{ { "TLV", 10 }, { "FP", 20 }, { "EL", 30 } };

			var stocks = new[]
			{
				new Stock("TLV") { Count = 2, Price = 10, Weight = 0.2m }, // should add 2
				new Stock("FP") { Count = 1, Price = 20, Weight = 0.2m }, // should add 3
				new Stock("EL") { Count = 2, Price = 30, Weight = 0.6m } // should add 2
			};

			var targetWeights = new StockWeights
			{ { "TLV", 0.2m }, { "FP", 0.3m }, { "EL", 0.5m } }; // target stock 4 * TLV + 4 * FP + 4 * EL = 240

			var portfolio = new PortfolioBuilder()
				.UsePrices(prices)
				.UseStocks(stocks)
				.UseWeightAdjustmentStrategy(new FollowTargetAdjustmentStrategy())
				.UseTargetWeights(targetWeights)
				.UseToBuyAmount(140)
				.Build();

			Assert.AreEqual(4, portfolio["EL"].Count);
			Assert.AreEqual(4, portfolio["FP"].Count);
			Assert.AreEqual(4, portfolio["TLV"].Count);

			Assert.AreEqual(240, portfolio.TotalValue);
		}
	}
}