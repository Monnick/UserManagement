using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Service.Exceptions
{
    public class ValidationException : Exception
	{
		public ValidationResult Result { get; private set; }

		public ValidationException(ValidationResult result)
			: base()
		{
			Result = result;
		}
	}
}
