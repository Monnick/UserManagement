using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Service.Configuration;
using UserManagement.Service.Exceptions;
using UserManagement.Service.Models;
using UserManagement.Service.Services.Contracts;
using UserManagement.Service.Telemetry.Contracts;

namespace UserManagement.Service.Controllers
{
	[Route("auth")]
    public class AuthenticateController : Controller
	{
		private readonly ITelemetry _telemetry;
		private readonly IAuthService _service;
		private readonly AppSettings _settings;
		private readonly ILogger _logger;

		public AuthenticateController(IAuthService service, ITelemetry telemetry, AppSettings settings, ILogger<AuthenticateController> logger)
		{
			_settings = settings;
			_service = service;
			_logger = logger;
			_telemetry = telemetry;
		}

		[HttpPost]
		public IActionResult Authenticate([FromBody] LoginUser user)
		{
			using (var operation = _telemetry.StartOperation("CreateUser"))
			{
				try
				{
					if (user == null)
					{
						_logger.LogWarning("Create: User object is null.");
						return BadRequest();
					}

					var authUser = _service.Authenticate(user.Login, user.Password);

					if (authUser == null)
						return NotFound();

					var tokenHandler = new JwtSecurityTokenHandler();
					var key = Encoding.ASCII.GetBytes(_settings.Secret);
					var tokenDescriptor = new SecurityTokenDescriptor
					{
						Subject = new ClaimsIdentity(new Claim[]
						{
					new Claim(ClaimTypes.Name, authUser.Id.ToString())
						}),
						Expires = DateTime.UtcNow.AddDays(1),
						SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
					};
					var token = tokenHandler.CreateToken(tokenDescriptor);
					var tokenString = tokenHandler.WriteToken(token);

					// return basic user info (without password) and token to store client side
					return Ok(new
					{
						Id = authUser.Id,
						Token = tokenString
					});
				}
				catch (UserNotFoundException ex)
				{
					_telemetry.TrackException(ex);
					return NotFound(ex.Message);
				}
				catch (WrongPasswordException ex)
				{
					_telemetry.TrackException(ex);
					var response = new ContentResult()
					{
						StatusCode = StatusCodes.Status403Forbidden,
						Content = ex.Message
					};
					return response;
				}
				catch (Exception ex)
				{
					_telemetry.TrackException(ex);
					return BadRequest();
				}
			}
		}
	}
}
