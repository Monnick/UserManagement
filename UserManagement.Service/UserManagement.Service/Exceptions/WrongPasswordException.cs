using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Service.Exceptions
{
    public class WrongPasswordException : Exception
	{
		public WrongPasswordException()
			: base()
		{ }

		public WrongPasswordException(string message)
			: base(message)
		{ }

		public WrongPasswordException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
