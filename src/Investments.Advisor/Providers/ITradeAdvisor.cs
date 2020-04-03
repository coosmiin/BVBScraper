using Investments.Domain.Stocks;
using System.Threading.Tasks;

namespace Investments.Advisor.Providers
{
	public interface ITradeAdvisor
	{
		Task<Stock[]> CalculateToBuyStocksAsync(decimal toBuyAmount, Stock[] existingStocks, Stock[] bvbStocks);
	}
}
