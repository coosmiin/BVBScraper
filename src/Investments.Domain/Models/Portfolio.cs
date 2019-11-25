using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Investments.Domain.Models
{
	public class Portfolio : IEnumerable<Stock>
	{
		private readonly IList<Stock> _stocks;
		private bool _staleWeights = true;

		public decimal TotalValue => _stocks.Sum(s => s.Count * s.Price);

		public Portfolio()
		{
			_stocks = Enumerable.Empty<Stock>().ToList();
		}

		public Portfolio(Stock[] stocks)
		{
			_stocks = stocks?.ToList() ?? Enumerable.Empty<Stock>().ToList();
		}

		public Stock this[string symbol]
		{
			get
			{
				RecalculateWeightsIfStale();
				return Get(symbol) ?? throw new ArgumentException($"Symbol: {symbol} not in stock list");
			}
		}

		public Portfolio AddStock(Stock stock)
		{
			var currentStock = Get(stock.Symbol);
			if (currentStock == null)
			{
				_stocks.Add(stock);
			}
			else
			{
				_stocks.Remove(currentStock);
				_stocks.Add(currentStock + stock);
			}

			_staleWeights = true;

			return this;
		}

		public IEnumerator<Stock> GetEnumerator()
		{
			RecalculateWeightsIfStale();
			return _stocks.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _stocks.GetEnumerator();
		}

		private Stock Get(string symbol)
		{
			return _stocks.SingleOrDefault(s => s.Symbol == symbol);
		}

		private void RecalculateWeightsIfStale()
		{
			if (_staleWeights)
			{
				_staleWeights = false;
				RecalculateWeights();
			}
		}

		internal virtual void RecalculateWeights()
		{
			var totalValue = TotalValue;

			foreach (var stock in this)
			{
				stock.Weight = Math.Round(stock.Count * stock.Price / totalValue, 2);
			}
		}
	}
}
