using System;
using System.Runtime.Serialization;

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

		protected InvalidBETDataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
