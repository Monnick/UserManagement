using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Service.Exceptions;
using UserManagement.Service.Models;
using UserManagement.Storage.Contracts;

namespace UserManagement.Service.Service
{
	public class AuthService : Contracts.IAuthService
	{
		private readonly Configuration.AppSettings _settings;
		private readonly IUserContext _context;

		public AuthService(Configuration.AppSettings settings, IUserContext context)
		{
			_settings = settings;
			_context = context;
		}

		public User Authenticate(string login, string password)
		{
			if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
				return null;

			var entity = _context.Users.SingleOrDefault(u => u.Login == login);

			// check if username exists
			if (entity == null)
				throw new UserNotFoundException(login);

			// check if password is correct
			if (!_settings.VerifyPasswordHash(password, entity.PasswordHash, entity.PasswordSalt))
				throw new WrongPasswordException("Password is not correct");

			// authentication successful
			return Converters.UserConverter.Convert(entity);
		}
	}
}
