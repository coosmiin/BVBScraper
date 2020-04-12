using Investments.Domain.Portfolios;
using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using Investments.Logic.Weights;
using Investments.Utils.Serialization;
using NUnit.Framework;
using System.Collections.Generic;
using Trading.IntegrationTests.Properties;

namespace Trading.IntegrationTests.Logic
{
	public class WeightAdjustmentStrategyTests
	{
		[Test]
		public void FollowTargetAdjustmentStrategy_AsExpected()
		{
			var strategy = new FollowTargetAdjustmentStrategy();

			var bvbStocks = JsonSerializerHelper.Deserialize<Stock[]>(Resources.bvb_index);
			var existingStocks = JsonSerializerHelper.Deserialize<Dictionary<string, int>>(Resources.portfolio).AsStocks();

			existingStocks = existingStocks.UpdatePrices(bvbStocks.AsStockPrices());

			var currentPortfolio = new Portfolio(existingStocks);

			var currentWeights = currentPortfolio.AsStockWeights();
			var targetWeights = bvbStocks.AsStockWeights();

			var adjustedWeights = strategy.AdjustWeights(currentWeights, targetWeights, currentPortfolio.TotalValue / 2000);
		}
	}
}