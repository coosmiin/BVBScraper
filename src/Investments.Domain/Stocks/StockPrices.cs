using System.Collections.Generic;

namespace Investments.Domain.Stocks
{
	public class StockPrices : Dictionary<string, decimal>
	{
		public StockPrices()
		{
		}

		public StockPrices(IDictionary<string, decimal> prices)
			: base(prices)
		{
		}
	}
}
