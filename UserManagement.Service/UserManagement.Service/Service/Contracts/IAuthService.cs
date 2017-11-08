using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Service.Service.Contracts
{
    public interface IAuthService
    {
		Models.User Authenticate(string login, string password);
    }
}
