using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Investments.Advisor.AzureProxies;
using Investments.Advisor.Providers;
using Investments.Advisor.Trading;
using SecretStore;

namespace Trading.Console
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var secretStore = new LocalSecretStore<Program>();

			var azureOrchestrationFuncKey = secretStore.GetSecret("Azure-TradeOrchestrationFuncKey");
			var azureAutomationFuncKey = secretStore.GetSecret("Azure-TradeAutomationFuncKey");


			var bvbDataProvider = new AzureBVBDataProviderProxy(new HttpClient(), azureOrchestrationFuncKey);
			// var bvbDataProvider = new StaticDataBVBDataProvider();
			var tradeAdvisor = new AzureTradeAdvisorProxy(new HttpClient(), azureOrchestrationFuncKey);
			var tradeAutomation = new AzureTradeAutomationProxy(new HttpClient(), azureAutomationFuncKey);
			var orchestrator = new TradeSessionOrchestrator(bvbDataProvider, tradeAutomation, tradeAdvisor);

			// await orchestrator.Run();

			var toBuyStocks = await orchestrator.GetToBuyStocks();

			foreach (var stock in toBuyStocks)
			{
				System.Console.WriteLine($"{stock.Symbol} ({stock.Price})\t {stock.Count}\t ({stock.Count * stock.Price})\t {stock.Weight}");
			}

			System.Console.WriteLine("-------------------------------------");

			System.Console.WriteLine($"Invested sum: {toBuyStocks.Sum(s => s.Count * s.Price)}");
		}
	}
}
