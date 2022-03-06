using System;
using System.Runtime.Serialization;

namespace Investments.Advisor.Exceptions
{
	public class InvalidPortfolioDataException : Exception
	{
		public InvalidPortfolioDataException()
		{
		}

		public InvalidPortfolioDataException(string? message) : base(message)
		{
		}

		public InvalidPortfolioDataException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected InvalidPortfolioDataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
