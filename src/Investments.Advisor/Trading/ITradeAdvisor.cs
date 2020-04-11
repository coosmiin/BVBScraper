using Investments.Domain.Stocks;
using System.Threading.Tasks;

namespace Investments.Advisor.Trading
{
	public interface ITradeAdvisor
	{
		Task<Stock[]> CalculateToBuyStocksAsync(decimal toBuyAmount, Stock[] existingStocks, Stock[] bvbStocks);
	}
}
