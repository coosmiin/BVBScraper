using Investments.Domain.Stocks;
using Investments.Logic.Calculus;
using Investments.Logic.Portfolios;
using NUnit.Framework;
using System.Linq;

namespace Investments.Logic.Tests.Portfolios
{
	public partial class PortfolioBuilderTests
	{
		[Test]
		public void BuildPortfolio_WithExistingStocks_PortfolioCorrectlyBuilt()
		{
			var prices = new StockPrices
			{
				{ "TLV", 2.59m },
				{ "FP", 1.195m },
				{ "SNP", .4415m },
				{ "SNG", 38.4m },
				{ "BRD", 15.72m },
				{ "TGN", 373.0m },
				{ "EL", 11.20m },
				{ "DIGI", 32.5m },
				{ "SNN", 14.64m },
				{ "TEL", 20.0m },
				{ "ALR", 2.12m },
				{ "M", 33.40m },
				{ "COTE", 80.0m },
				{ "WINE", 22.3m },
				{ "SFG", 18.7m },
				{ "BVB", 26.0m }
			};

			var targetWeights = new StockWeights
			{
				{ "TLV", 0.2033m },
				{ "FP", 0.193m },
				{ "SNP", 0.1833m },
				{ "SNG", 0.1085m },
				{ "BRD", 0.1071m },
				{ "TGN", 0.0536m },
				{ "EL", 0.0379m },
				{ "DIGI", 0.0318m },
				{ "SNN", 0.0208m },
				{ "TEL", 0.0143m },
				{ "ALR", 0.0111m },
				{ "M", 0.0108m },
				{ "COTE", 0.0068m },
				{ "WINE", 0.0065m },
				{ "SFG", 0.0053m },
				{ "BVB", 0.0051m }
			};

			var stocks = new[]
			{
				new Stock("TLV") { Count = 158 },
				new Stock("FP") { Count = 326 },
				new Stock("SNP") { Count = 838 },
				new Stock("SNG") { Count = 6 },
				new Stock("BRD") { Count = 14 },
				new Stock("EL") { Count = 7 },
				new Stock("DIGI") { Count = 2 },
				new Stock("SNN") { Count = 3 },
				new Stock("TEL") { Count = 1 },
				new Stock("ALR") { Count = 11 },
				new Stock("M") { Count = 1 },
				new Stock("WINE") { Count = 1 },
				new Stock("SFG") { Count = 1 }
			};

			var portfolio = new PortfolioBuilder()
				.UsePrices(prices)
				.UseStocks(stocks)
				.UseTargetWeights(targetWeights)
				.UseToBuyAmount(2000)
				.Build();

			foreach (var stock in portfolio)
			{
				System.Console.WriteLine($"{stock.Symbol} ({stock.Price})\t {stock.Count}\t {stock.Weight} ({targetWeights[stock.Symbol]})");
			}

			Assert.IsTrue(MathHelper.IsApproxOne(portfolio.Sum(s => s.Weight)));
		}
	}
}