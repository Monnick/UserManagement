using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Service.Models;

namespace UserManagement.Service.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUser>
	{
		public UpdateUserValidator()
		{
			RuleFor(user => user.Name).NotEmpty().WithMessage("Please specify a name");
			RuleFor(user => user.Email).NotEmpty().WithMessage("Please specify an email");
			RuleFor(user => user.Country).NotEmpty().WithMessage("Please specify a country");
		}
	}
}
