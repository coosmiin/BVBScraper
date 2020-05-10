using System.Linq;
using Investments.Domain.Stocks.Extensions;
using Investments.Logic.Portfolios;
using Investments.Logic.Weights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Trading.Functions.Advisor
{
	public static class TradeAdvisorFunctions
	{
		private const int MIN_ORDER_VALUE = 250;

		[FunctionName(nameof(CalculateToBuyStocks))]
		public static IActionResult CalculateToBuyStocks(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "calculateToBuyStocks")] AdvisorRequest request)
		{
			var stockPrices = request.BETStocks.AsStockPrices();
			var targetWeights = request.BETStocks.AsStockWeights();

			var existingStocks = request.ExistingStocks.UpdatePrices(stockPrices);

			decimal currentPortfolioValue = existingStocks.Sum(s => s.TotalValue);

			var strategy =
				new MinOrderValueCutOffStrategy(
					new FollowTargetAdjustmentStrategy(), MIN_ORDER_VALUE / request.ToBuyAmount);

			var portfolio = new PortfolioBuilder()
				.UseStocks(existingStocks)
				.UsePrices(stockPrices)
				.UseTargetWeights(targetWeights)
				.UseToBuyAmount(request.ToBuyAmount)
				.UseMinOrderValue(MIN_ORDER_VALUE)
				.UseWeightAdjustmentStrategy(strategy)
				.Build();

			return new OkObjectResult(portfolio.DeriveToBuyStocks(existingStocks));
		}
	}
}
