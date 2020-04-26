namespace Investments.Domain.Trading
{
	public class TradeOrder
	{
		public string Symbol { get; set; } = string.Empty;

		public int Count { get; set; }

		public OperationType OperationType { get; set; } = OperationType.Buy;
	}
}