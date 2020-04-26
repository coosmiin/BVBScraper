using System;

namespace Trading.Functions.Environments
{
	public class LocalEnvironment : IEnvironment
	{
		public Uri TradeAutomationFunctionsHost { get; } = new Uri("http://localhost:7072");

		public Uri TradingFunctionsHost { get; } = new Uri("http://localhost:7071");
	}
}
