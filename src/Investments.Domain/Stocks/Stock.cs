using System;

namespace Investments.Domain.Stocks
{
	public class Stock
	{
		public string Symbol { get; }

		public int Count { get; set; }

		public decimal Price { get; set; }

		public decimal Weight { get; set; }

		public decimal TotalValue => Count * Price;

		public Stock(string symbol)
		{
			Symbol = symbol;
		}

		/// <summary>
		/// Adds two stocks with same symbol
		/// </summary>
		/// <remarks>
		/// In case prices are different, the second stock is leading; Weight is reset;
		/// </remarks>
		public static Stock operator +(Stock stock1, Stock stock2)
		{
			if (stock1.Symbol == stock2.Symbol)
			{
				return new Stock(stock2.Symbol) { Price = stock2.Price, Count = stock1.Count + stock2.Count };
			}

			throw new ArgumentException($"'{stock1.Symbol}' cannot be added to '{stock2.Symbol}'");
		}

		/// <summary>
		/// Unary operator for decreasing stock count by 1
		/// </summary>
		public static Stock operator --(Stock stock)
		{
			return new Stock(stock.Symbol) { Price = stock.Price, Count = stock.Count-- };
		}
	}
}
