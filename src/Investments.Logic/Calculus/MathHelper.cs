using Investments.Domain.Stocks;
using Investments.Domain.Stocks.Extensions;
using System.Linq;

namespace Investments.Logic.Calculus
{
	public static class MathHelper
	{
		public static bool IsApproxOne(this decimal value)
		{
			return value > 0.9m && value < 1.1m;
		}

		/// <summary>
		/// Equaly redistributes weights such that their sum is 1. 
		/// </summary>
		/// <remarks>
		/// This applies for both cases, weights over 1 or less than 1.
		/// </remarks>
		public static StockWeights Redistribute(this StockWeights weights)
		{
			var totalWeight = weights.Sum(w => w.Value); // this needs to become 1 after redistribution
			return weights.Select(w => (w.Key, w.Value / totalWeight)).AsStockWeights();
		}
	}
}