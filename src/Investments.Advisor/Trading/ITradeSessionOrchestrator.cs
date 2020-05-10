using System.Threading.Tasks;

namespace Investments.Advisor.Trading
{
	public interface ITradeSessionOrchestrator
	{
		Task Run();
	}
}