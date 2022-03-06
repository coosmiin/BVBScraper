namespace Trading.BVBScraper
{
	public class BETStock
	{
		public string Symbol { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public decimal Weight { get; set; }
	}
}
