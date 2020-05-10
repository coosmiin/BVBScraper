using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Investments.Advisor.Trading;
using Microsoft.Extensions.Logging;

namespace Trading.Functions.Orchestrator
{
	public class OrchestratorFunctions
	{
		private readonly ITradeSessionOrchestrator _orchestrator;

		public OrchestratorFunctions(ITradeSessionOrchestrator orchestrator)
		{
			_orchestrator = orchestrator;
		}

		[FunctionName(nameof(StartTradingSession))]
		public void StartTradingSession(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "startTradingSession")] HttpRequest request)
		{
			_orchestrator.Run();
		}

		[FunctionName(nameof(TriggerTradingSession))]
		public void TriggerTradingSession([TimerTrigger("0 0 12 8 * *")] TimerInfo timer, ILogger logger)
		{
			if (timer.IsPastDue)
			{
				logger.LogInformation("Timer is running late");
				return;
			}

			_orchestrator.Run();
		}
	}
}
