namespace Investments.Domain.Trading
{
	public class TradeOrder
	{
		public string Symbol { get; set; } = string.Empty;

		public int Count { get; set; }

		public OperationType OperationType { get; set; } = OperationType.Buy;

		/// <summary>
		/// The maket type; Available values: REGS (Regular Shares), XRS1 (Extra Regular Shares - Category 1)
		/// </summary>
		public string Market { get; set; } = "REGS";
	}
}