using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Service.Converters
{
    class UserConverter
    {
		internal static Models.User Convert(Storage.Entities.User user)
		{
			if (user == null)
				return null;

			return new Models.User
			{
				Id = user.Id,
				Login = user.Login,
				Name = user.Name,
				Phone = user.Phone,
				Email = user.Email,
				Street = user.Street,
				City = user.City,
				Zip = user.Zip,
				Country = user.Country
			};
		}

		internal static Storage.Entities.User Convert(Models.User user)
		{
			if (user == null)
				return null;

			return new Storage.Entities.User
			{
				Id = user.Id.HasValue ? user.Id.Value : Guid.Empty,
				Login = user.Login,
				Name = user.Name,
				Phone = user.Phone,
				Email = user.Email,
				Street = user.Street,
				City = user.City,
				Zip = user.Zip,
				Country = user.Country
			};
		}
	}
}
