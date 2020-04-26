using System;

namespace Trading.Functions.Environments
{
	public class ProductionEnvironment : IEnvironment
	{
		public Uri TradeAutomationFunctionsHost => new Uri("https://tradeautomation.azurewebsites.net");

		public Uri TradingFunctionsHost => new Uri("https://tradeorchestration.azurewebsites.net");
	}
}
