using Investments.Domain.Stocks;
using System.Threading.Tasks;

namespace Investments.Advisor.Providers
{
	public interface IBVBDataProvider
	{
		Task<Stock[]> GetBETStocksAsync();
	}
}