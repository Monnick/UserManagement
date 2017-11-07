using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Service.Models;

namespace UserManagement.Service.Service.Contracts
{
    public interface IUserService
	{
		User Update(User user);

		User Create(User user);
		
		User GetById(Guid id);
	}
}
