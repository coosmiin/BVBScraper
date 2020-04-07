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

			var azureFuncKey = secretStore.GetSecret("Azure-TradeOrchestrationFuncKey");

			var bvbDataProvider = new AzureBVBDataProviderProxy(new HttpClient(), azureFuncKey);
			// var bvbDataProvider = new StaticDataBVBDataProvider();
			var tradeAdvisor = new AzureTradeAdvisorProxy(new HttpClient(), azureFuncKey);
			var tradeAutomation = new AzureTradeAutomationProxy(new HttpClient(), azureFuncKey);
			var orchestrator = new TradeSessionOrchestrator(bvbDataProvider, tradeAutomation, tradeAdvisor);

			// await orchestrator.Run();

			var toBuyStocks = await orchestrator.GetToBuyStocks(2393.13m);

			foreach (var stock in toBuyStocks)
			{
				System.Console.WriteLine($"{stock.Symbol} ({stock.Price})\t {stock.Count}\t ({stock.Count * stock.Price})\t {stock.Weight}");
			}

			System.Console.WriteLine("-------------------------------------");

			System.Console.WriteLine($"Invested sum: {toBuyStocks.Sum(s => s.Count * s.Price)}");
		}
	}
}
