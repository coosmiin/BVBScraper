using Investments.Advisor.AzureProxies;
using Investments.Advisor.Providers;
using Investments.Advisor.Trading;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Trading.BVBScraper;
using Trading.Functions.Environments;

[assembly: FunctionsStartup(typeof(Trading.Functions.Startup))]

namespace Trading.Functions
{
	public class Startup : FunctionsStartup
	{
		private const string BVB_HTTP_CLIENT = "BVBHttpClient";
		private const string TRADE_AUTOMATION_CLIENT = "TradeAutomationHttpClient";
		private const string TRADE_ADVISOR_CLIENT = "TradeAdvisorHttpClient";
		private const string AZURE_TRADE_ORCHESTRATION_KEY_NAME = "Azure-TradeOrchestrationFuncKey";
		private const string AZURE_TRADE_AUTOMATION_KEY_NAME = "Azure-TradeAutomationFuncKey";

		public override void Configure(IFunctionsHostBuilder builder)
		{
			var services = builder.Services;

			services
				.AddSingleton(new StockScraper())
				.AddSingleton<IEnvironment>(ResolveEnvironment)
				.AddSingleton<IBVBDataProvider>(ResolveBVBDataProvider)
				.AddSingleton<ITradeAutomation>(ResolveTradeAutomation)
				.AddSingleton<ITradeAdvisor>(ResolveTradeAdvisor)
				.AddLogging();

			services.AddHttpClient(BVB_HTTP_CLIENT, ConfigureBVBClient);
			services.AddHttpClient(TRADE_AUTOMATION_CLIENT, ConfigureTradeAutomationClient);
			services.AddHttpClient(TRADE_ADVISOR_CLIENT, ConfigureTradeAdvisorClient);

			services
				.AddSingleton<ITradeSessionOrchestrator, TradeSessionOrchestrator>();
		}

		private IBVBDataProvider ResolveBVBDataProvider(IServiceProvider provider)
		{
			var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(BVB_HTTP_CLIENT);
			return new AzureBVBDataProviderProxy(httpClient, GetEnvironmentVariable(AZURE_TRADE_ORCHESTRATION_KEY_NAME));
		}

		private ITradeAutomation ResolveTradeAutomation(IServiceProvider provider)
		{
			var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(TRADE_AUTOMATION_CLIENT);
			return new AzureTradeAutomationProxy(httpClient, GetEnvironmentVariable(AZURE_TRADE_AUTOMATION_KEY_NAME));
		}

		private ITradeAdvisor ResolveTradeAdvisor(IServiceProvider provider)
		{
			var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(TRADE_ADVISOR_CLIENT);
			return new AzureTradeAdvisorProxy(httpClient, GetEnvironmentVariable(AZURE_TRADE_ORCHESTRATION_KEY_NAME));
		}

		private void ConfigureBVBClient(IServiceProvider provider, HttpClient client)
		{
			var environment = provider.GetService<IEnvironment>();
			client.BaseAddress = environment?.TradingFunctionsHost;
		}

		private void ConfigureTradeAutomationClient(IServiceProvider provider, HttpClient client)
		{
			var environment = provider.GetService<IEnvironment>();
			client.BaseAddress = environment?.TradeAutomationFunctionsHost;
		}

		private void ConfigureTradeAdvisorClient(IServiceProvider provider, HttpClient client)
		{
			var environment = provider.GetService<IEnvironment>();
			client.BaseAddress = environment?.TradingFunctionsHost;
		}

		private IEnvironment ResolveEnvironment(IServiceProvider provider)
		{
			bool isDevelopment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development";

			return isDevelopment ? new LocalEnvironment() as IEnvironment : new ProductionEnvironment();
		}

		private static string GetEnvironmentVariable(string key)
			=> Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process) ?? string.Empty;
	}
}
