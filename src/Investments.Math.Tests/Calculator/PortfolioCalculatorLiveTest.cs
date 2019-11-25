using Investments.Math.Calculator;
using NUnit.Framework;
using System.Collections.Generic;

namespace Investments.Math.Tests.Calculator
{
	public class PortfolioCalculatorLiveTest
	{
		[Test]
		public void BuildPortfolio_LiveBVBValues()
		{
			var prices = new Dictionary<string, decimal>
			{
				{ "TLV", 2.485m },
				{ "FP", 1.19m },
				{ "SNP", .436m },
				{ "SNG", 37.5m },
				{ "BRD", 14.9m },
				{ "TGN", 369 },
				{ "EL", 11.2m },
				{ "DIGI", 32.7m },
				{ "SNN", 13.9m },
				{ "TEL", 20 },
				{ "ALR", 2.18m },
				{ "M", 33 },
				{ "COTE", 79.8m },
				{ "WINE", 22.7m },
				{ "SFG", 18.75m },
				{ "BVB", 26.2m }
			};

			var targetWeights = new Dictionary<string, decimal>
			{
				{ "TLV", 0.1992m },
				{ "FP", 0.1963m },
				{ "SNP", 0.1848m },
				{ "SNG", 0.1082m },
				{ "BRD", 0.1036m },
				{ "TGN", 0.0542m },
				{ "EL", 0.0387m },
				{ "DIGI", 0.0326m },
				{ "SNN", 0.0209m },
				{ "TEL", 0.0146m },
				{ "ALR", 0.0116m },
				{ "M", 0.0109m },
				{ "COTE", 0.0069m },
				{ "WINE", 0.0068m },
				{ "SFG", 0.0054m },
				{ "BVB", 0.0053m },
			};

			var portfolio = new PortfolioCalculator(prices).BuildPortfolio(2000, targetWeights);
		}
	}
}
