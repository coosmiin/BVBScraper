using Investments.Domain.Stocks;
using System.Threading.Tasks;

namespace Investments.Advisor.Trading
{
	public interface ITradeAutomation
	{
		Task<Stock[]> GetPortfolio();
	}
}
