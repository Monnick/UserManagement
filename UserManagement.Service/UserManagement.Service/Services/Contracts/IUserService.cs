using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Service.Models;

namespace UserManagement.Service.Services.Contracts
{
    public interface IUserService
	{
		UpdateUser Update(UpdateUser user);

		User Create(User user);
		
		User GetById(Guid id);
	}
}
