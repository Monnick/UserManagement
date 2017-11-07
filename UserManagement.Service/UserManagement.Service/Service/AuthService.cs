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

			var entity = _context.Users.SingleOrDefault(x => x.Login == login && x.Status == Storage.Entities.UserStatus.Activated);

			// check if username exists
			if (entity == null)
				throw new UserNotFoundException(login);

			// check if password is correct
			if (!_settings.VerifyPasswordHash(password, entity.PasswordHash, entity.PasswordSalt))
				throw new WrongPasswordException("Password is not correct");

			// authentication successful
			return Converters.UserConverter.Convert(entity);
		}

		public User ActivateUser(string login, string secret)
		{
			if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(secret))
				return null;

			var user = _context.Users.FirstOrDefault(u => u.Login == login && u.Secret == secret && u.Status == Storage.Entities.UserStatus.Created);

			if (user == null)
				return null;

			user.Status = Storage.Entities.UserStatus.Activated;
			user.Secret = string.Empty;
			_context.Update(user);
			_context.SaveChanges();

			return Converters.UserConverter.Convert(user);
		}
	}
}
