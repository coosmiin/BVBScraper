using Investments.Advisor.Models;
using Investments.Advisor.Providers.StaticData;
using Investments.Domain.Stocks;
using Investments.Utils.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace Investments.Advisor.Providers
{
	public class StaticDataBVBDataProvider : IBVBDataProvider
	{
		public Task<Stock[]> GetBETStocksAsync()
		{
			var indexStocks = JsonSerializerHelper.Deserialize<BETStock[]>(Resources.bet_index);

			return Task.FromResult(indexStocks.Select(s => new Stock(s.Symbol) { Price = s.Price, Weight = s.Weight }).ToArray());
		}
	}
}
