using Investments.Domain.Portfolios;
using Investments.Domain.Stocks;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace Investments.Domain.Tests.Portfolios
{
	public class PortfolioTests
	{
		[Test]
		public void Ctor_NullInitialStock_IsEmpty()
		{
			var portfolio = new Portfolio();

			Assert.True(portfolio.Any());
		}

		[Test]
		public void Ctor_WithInitialStock_IsInitializedCorrectly()
		{
			var portfolio = new Portfolio(new[] { new Stock("FP") { Count = 100, Price = 10 } });

			Assert.AreEqual(100, portfolio["FP"].Count);
		}

		[Test]
		public void TotalValue_EmptyPortfolio_IsZero()
		{
			Assert.AreEqual(0, new Portfolio().TotalValue);
		}

		[Test]
		public void TotalValue_NonEmptyPortfolio_IsCorrect()
		{
			var portfolio = new Portfolio(
				new[]
				{
					new Stock("TVL") { Price = 10, Count = 2, Weight = 49 },
					new Stock("TVL") { Price = 3, Count = 7, Weight = 51 }
				});

			Assert.AreEqual(41, portfolio.TotalValue);
		}

		[Test]
		public void RecalculateWeights_WithInitialStocks_WeightsAreCorrect()
		{
			var portfolio = new Portfolio(new[]
			{
				new Stock("EL") { Count = 1, Price = 30 },
				new Stock("FP") { Count = 1, Price = 20 },
				new Stock("TLV") { Count = 1, Price = 10 }
			});

			Assert.AreEqual(0.5m, portfolio["EL"].Weight);
			Assert.AreEqual(0.33m, portfolio["FP"].Weight);
			Assert.AreEqual(0.17m, portfolio["TLV"].Weight);
		}

		[Test]
		public void RecalculateWeights_StocksAddedSequentially_WeightsAreCorrectlyCaclulatedOnlyOnce()
		{
			var portfolioMock = new Mock<Portfolio> { CallBase = true };

			portfolioMock.Object.AddStock(new Stock("EL") { Count = 1, Price = 30 });
			portfolioMock.Object.AddStock(new Stock("FP") { Count = 1, Price = 20 });
			portfolioMock.Object.AddStock(new Stock("TLV") { Count = 1, Price = 10 });

			Assert.AreEqual(0.5m, portfolioMock.Object["EL"].Weight);
			Assert.AreEqual(0.33m, portfolioMock.Object["FP"].Weight);
			Assert.AreEqual(0.17m, portfolioMock.Object["TLV"].Weight);

			portfolioMock.Verify(p => p.RecalculateWeights(), Times.Once);
		}

		[Test]
		public void AddStock_NoCurrentStock_StockIsCorrectlyAdded()
		{
			var portfolio = new Portfolio();

			portfolio.AddStock(new Stock("FP") { Count = 10, Price = 10 });

			Assert.AreEqual(10, portfolio["FP"].Count);
		}

		[Test]
		public void AddStock_WithCurrentStock_StockIsCorrectlyAdded()
		{
			var portfolio = new Portfolio(new[] { new Stock("FP") { Count = 2, Price = 10 } });

			portfolio.AddStock(new Stock("FP") { Count = 10, Price = 20 });

			Assert.AreEqual(12, portfolio["FP"].Count);
		}

		[Test]
		public void GetEnumarator_NoIndexerCalled_WeightsAreRefreshed()
		{
			var portfolio = new Portfolio(new[]
{
				new Stock("EL") { Count = 1, Price = 30 },
				new Stock("FP") { Count = 1, Price = 20 },
				new Stock("TLV") { Count = 1, Price = 10 }
			});

			Assert.IsFalse(portfolio.Any(s => s.Weight == 0));
		}
	}
}
