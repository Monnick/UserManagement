using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Service.Exceptions;
using UserManagement.Service.Models;
using UserManagement.Service.Validators;
using UserManagement.Storage.Contracts;

namespace UserManagement.Service.Service
{
    public class UserService : Contracts.IUserService
    {
		private readonly Configuration.AppSettings _settings;
		private readonly IUserContext _context;

		public UserService(Configuration.AppSettings settings, IUserContext context)
		{
			_settings = settings;
			_context = context;
		}
		
		public User Create(User user)
		{
			// validation
			if (_context.Users.Any(x => x.Login == user.Login))
				throw new InvalidOperationException("Login '" + user.Login + "' is already taken");

			var validator = new UserValidator();
			var results = validator.Validate(user);

			if (!results.IsValid)
				throw new ValidationException(results);

			byte[] passwordHash, passwordSalt;
			_settings.CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

			var entity = Converters.UserConverter.Convert(user);
			
			entity.PasswordHash = passwordHash;
			entity.PasswordSalt = passwordSalt;

			_context.Add(entity);
			_context.SaveChanges();

			user.Id = entity.Id;
			return user;
		}
		
		public User Update(User user)
		{
			var entity = _context.Find(user.Id);

			if (entity == null)
				throw new UserNotFoundException(user.Name);

			if (!string.IsNullOrEmpty(user.Password) && string.IsNullOrEmpty(user.OldPassword))
				throw new WrongPasswordException("Old password is required");

			var validator = new UserValidator();
			var results = validator.Validate(user);

			if (!results.IsValid)
				throw new ValidationException(results);

			// update user properties
			entity.Name = user.Name;
			entity.Phone = user.Phone;
			entity.Email = user.Email;
			entity.Street = user.Street;
			entity.City = user.City;
			entity.Zip = user.Zip;
			entity.Country = user.Country;

			// update password if it was entered
			if (!string.IsNullOrWhiteSpace(user.Password))
			{
				if (!_settings.VerifyPasswordHash(user.OldPassword, entity.PasswordHash, entity.PasswordSalt))
					throw new WrongPasswordException("Old password is not correct");

				byte[] passwordHash, passwordSalt;
				_settings.CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

				entity.PasswordHash = passwordHash;
				entity.PasswordSalt = passwordSalt;
			}

			_context.Update(entity);
			_context.SaveChanges();

			user.Password = string.Empty;
			user.OldPassword = string.Empty;

			return user;
		}

		public User GetById(Guid id)
		{
			return Converters.UserConverter.Convert(_context.Users.SingleOrDefault(u => u.Id == id));
		}
	}
}
