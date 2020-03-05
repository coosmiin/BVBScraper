using System;

namespace Investments.Advisor.Exceptions
{
	public class InvalidBETDataException : Exception
	{
		public InvalidBETDataException()
		{
		}

		public InvalidBETDataException(string message) : base(message)
		{
		}

		public InvalidBETDataException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
