using Investments.Domain.Stocks;
using System;

namespace Trading.Functions.Advisor
{
	public class AdvisorRequest
	{
		public Stock[] BvbStocks { get; set; } = Array.Empty<Stock>();

		public Stock[] ExistingStocks { get; set; } = Array.Empty<Stock>();

		public decimal ToBuyAmount { get; set; }
	}
}
