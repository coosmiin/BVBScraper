using System;
using System.Runtime.Serialization;

namespace Investments.Advisor.Exceptions
{
	public class InvalidBvbDataException : Exception
	{
		public InvalidBvbDataException()
		{
		}

		public InvalidBvbDataException(string message) : base(message)
		{
		}

		public InvalidBvbDataException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InvalidBvbDataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
