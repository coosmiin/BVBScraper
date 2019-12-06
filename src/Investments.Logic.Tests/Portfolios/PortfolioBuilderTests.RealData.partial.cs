using Investments.Domain.Stocks;
using Investments.Logic.Calculus;
using Investments.Logic.Portfolios;
using NUnit.Framework;
using System.Linq;

namespace Investments.Logic.Tests.Portfolios
{
	public partial class PortfolioBuilderTests
	{
		[Ignore("still TODO")]
		[Test]
		public void BuildPortfolio_WithExistingStocks_PortfolioCorrectlyBuilt()
		{
			var prices = new StockPrices
			{
				{ "TLV", 2.58m },
				{ "FP", 1.19m },
				{ "SNP", .44m },
				{ "SNG", 37.55m },
				{ "BRD", 15.3m },
				{ "TGN", 368 },
				{ "EL", 11.1m },
				{ "DIGI", 32.3m },
				{ "SNN", 13.96m },
				{ "TEL", 20 },
				{ "ALR", 2.13m },
				{ "M", 33 },
				{ "COTE", 79.2m },
				{ "WINE", 22.4m },
				{ "SFG", 18.7m },
				{ "BVB", 26 }
			};

			var targetWeights = new StockWeights
			{
				{ "TLV", 0.2045m },
				{ "FP", 0.1941m },
				{ "SNP", 0.1845m },
				{ "SNG", 0.1071m },
				{ "BRD", 0.1052m },
				{ "TGN", 0.0535m },
				{ "EL", 0.0379m },
				{ "DIGI", 0.0319m },
				{ "SNN", 0.0208m },
				{ "TEL", 0.0145m },
				{ "ALR", 0.0113m },
				{ "M", 0.0108m },
				{ "COTE", 0.0068m },
				{ "WINE", 0.0066m },
				{ "SFG", 0.0054m },
				{ "BVB", 0.0052m }
			};

			var stocks = new[]
			{
				new Stock("TLV") { Count = 158, Price = 2.58m },
				new Stock("FP") { Count = 326, Price = 1.19m },
				new Stock("SNP") { Count = 838, Price = 0.44m },
				new Stock("SNG") { Count = 6, Price = 37.55m },
				new Stock("BRD") { Count = 14, Price = 15.3m },
				new Stock("EL") { Count = 7, Price = 11.1m },
				new Stock("DIGI") { Count = 2, Price = 32.3m },
				new Stock("SNN") { Count = 3, Price = 13.96m },
				new Stock("TEL") { Count = 1, Price = 20.0m },
				new Stock("ALR") { Count = 11, Price = 2.13m },
				new Stock("M") { Count = 1, Price = 33.0m },
				new Stock("WINE") { Count = 1, Price = 22.4m },
				new Stock("SFG") { Count = 1, Price = 18.7m }
			};

			var portfolio = new PortfolioBuilder()
				.UsePrices(prices)
				.UseStocks(stocks)
				.UseTargetWeights(targetWeights)
				.UseToBuyAmount(2000)
				.Build();

			Assert.IsTrue(MathHelper.IsApproxOne(portfolio.Sum(s => s.Weight)));
			Assert.IsNotNull(portfolio["BVB"]);
		}
	}
}