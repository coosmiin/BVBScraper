using Investments.Domain.Portfolios;
using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using Investments.Logic.Calculus;
using Investments.Logic.Weights;
using Investments.Utils.Serialization;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Trading.IntegrationTests.Properties;

namespace Trading.IntegrationTests.Logic
{
	public class WeightAdjustmentStrategyTests
	{
		private const int MIN_ORDER_VALUE = 250;

		[Test]
		public void FollowTargetAdjustmentStrategy_AsExpected()
		{
			var toBuyAmount = 2000m;
			var (currentPortfolio, targetWeights) = ReadDataResources();

			var strategy = new FollowTargetAdjustmentStrategy();

			var adjustedWeights = 
				strategy.AdjustWeights(currentPortfolio.AsStockWeights(), targetWeights, currentPortfolio.TotalValue / toBuyAmount);

			Assert.IsTrue(adjustedWeights.Sum(w => w.Value).IsApproxOne());
		}

		[Test]
		public void MinOrderValueCutOffStrategy_AsExpected()
		{
			var toBuyAmount = 2000m;
			var (currentPortfolio, targetWeights) = ReadDataResources();

			var strategy = new MinOrderValueCutOffStrategy(new FollowTargetAdjustmentStrategy(), MIN_ORDER_VALUE / toBuyAmount);

			var adjustedWeights =
				strategy.AdjustWeights(currentPortfolio.AsStockWeights(), targetWeights, currentPortfolio.TotalValue / toBuyAmount);

			Assert.IsTrue(adjustedWeights.Sum(w => w.Value).IsApproxOne());
		}

		private (Portfolio currentPortfolio, StockWeights targetWeights) ReadDataResources()
		{
			var bvbStocks = JsonSerializerHelper.Deserialize<Stock[]>(Resources.bvb_index);
			var existingStocks = JsonSerializerHelper.Deserialize<Dictionary<string, int>>(Resources.portfolio).AsStocks();

			existingStocks = existingStocks.UpdatePrices(bvbStocks.AsStockPrices());

			var currentPortfolio = new Portfolio(existingStocks);
			var targetWeights = bvbStocks.AsStockWeights();

			return (currentPortfolio, targetWeights);
		}
	}
}