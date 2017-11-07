using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Service.Exceptions
{
    public class UserNotFoundException : Exception
	{
		public UserNotFoundException(string userName)
			: base("User not found: " + userName)
		{ }
	}
}
