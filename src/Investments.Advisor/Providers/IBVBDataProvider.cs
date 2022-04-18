using Investments.Domain.Stocks;
using System.Threading.Tasks;

namespace Investments.Advisor.Providers
{
	public interface IBvbDataProvider
	{
		Task<Stock[]> GetBvbStocksAsync();
	}
}