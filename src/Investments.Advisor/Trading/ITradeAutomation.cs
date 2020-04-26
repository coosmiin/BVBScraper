using Investments.Domain.Stocks;
using Investments.Domain.Trading;
using System.Threading.Tasks;

namespace Investments.Advisor.Trading
{
	public interface ITradeAutomation
	{
		Task<(Stock[] Stocks, decimal AvailableAmount)> GetPortfolio();

		Task SubmitOrders(TradeOrder[] orders);
	}
}
