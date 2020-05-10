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
		public async void StartTradingSession(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "startTradingSession")] HttpRequest request)
		{
			await _orchestrator.Run();
		}

		[FunctionName(nameof(TriggerTradingSession))]
		public async void TriggerTradingSession([TimerTrigger("0 0 12 8 * *")] TimerInfo timer, ILogger logger)
		{
			if (timer.IsPastDue)
			{
				logger.LogInformation("Timer is running late");
				return;
			}

			await _orchestrator.Run();
		}
	}
}
