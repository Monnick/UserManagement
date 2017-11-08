﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Service.Configuration;
using UserManagement.Service.Exceptions;
using UserManagement.Service.Models;
using UserManagement.Service.Services.Contracts;

namespace UserManagement.Service.Controllers
{
	[Route("Users")]
    public class UsersController : Controller
    {
		private readonly IUserService _service;
		private readonly ILogger _logger;

		public UsersController(IUserService service, ILogger<UsersController> logger)
		{
			_service = service;
			_logger = logger;
		}

		[Authorize]
		[HttpGet]
		public IActionResult GetById(Guid id)
		{
			_logger.LogDebug("Get user by id ({0}). Authorized by {1}.", id, "");

			var user = _service.GetById(id);

			if (user == null)
				return NotFound();
			
			return Ok(user);
		}

		[HttpPut]
		public IActionResult CreateUser([FromBody] User user)
		{
			if (user == null)
			{
				_logger.LogWarning("Create: User object is null.");
				return BadRequest();
			}

			try
			{
				_logger.LogDebug("Create user {0}", user.Login);

				return Ok(_service.Create(user));
			}
			catch (InvalidOperationException ex)
			{
				_logger.LogError(ex, "Error occured.");

				var response = new ContentResult()
				{
					StatusCode = StatusCodes.Status409Conflict,
					Content = ex.Message
				};
				return response;
			}
			catch (ValidationException ex)
			{
				_logger.LogError(ex, "Error occured.");

				var response = new ContentResult()
				{
					StatusCode = StatusCodes.Status409Conflict,
					Content = string.Join(';', ex.Result.Errors.Select(e => e.ErrorMessage))
				};
				return response;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occured.");

				return BadRequest();
			}
		}

		[Authorize]
		[HttpPost]
		public IActionResult UpdateUser([FromBody] UpdateUser user)
		{
			try
			{
				if (user == null)
				{
					_logger.LogWarning("Update: User object is null.");
					return BadRequest();
				}

				Guid? userId = IsAuthorized();
				if (!userId.HasValue || user.Id != userId.Value)
				{
					if(userId.HasValue)
						_logger.LogWarning("Update: User {0} tried to change user {1}.", userId, user.Id);
					else
						_logger.LogWarning("Update: User {0} tried to sneak on some priviliges.", userId, user.Id);

					var response = new ContentResult()
					{
						StatusCode = StatusCodes.Status403Forbidden,
						Content = "Something went wrong. You are not permitted to make the change."
					};
					return response;
				}

				_logger.LogDebug("Update user {0}", user.Id);

				return Ok(_service.Update(user));
			}
			catch (UserNotFoundException ex)
			{
				_logger.LogError(ex, "Error occured.");

				return NotFound(ex.Message);
			}
			catch (WrongPasswordException ex)
			{
				_logger.LogError(ex, "Error occured.");

				var response = new ContentResult()
				{
					StatusCode = StatusCodes.Status403Forbidden,
					Content = ex.Message
				};
				return response;
			}
			catch (ValidationException ex)
			{
				_logger.LogError(ex, "Error occured.");

				var response = new ContentResult()
				{
					StatusCode = StatusCodes.Status409Conflict,
					Content = string.Join(';', ex.Result.Errors.Select(e => e.ErrorMessage))
				};
				return response;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occured.");

				return BadRequest();
			}
		}

		private Guid? IsAuthorized()
		{
			Guid result;
			if (Guid.TryParse(User.Identity.Name, out result))
				return result;
			return null;
		}
	}
}
