using System.Collections.Generic;

namespace Investments.Domain.Stocks
{
	public class StockWeights : Dictionary<string, decimal>
	{
		public StockWeights()
		{
		}

		public StockWeights(IDictionary<string, decimal> weights)
			: base(weights)
		{
		}
	}
}
