using System;

namespace Trading.Functions.Environments
{
	public interface IEnvironment
	{
		Uri TradeAutomationFunctionsHost { get; }

		Uri TradingFunctionsHost { get; }
	}
}
