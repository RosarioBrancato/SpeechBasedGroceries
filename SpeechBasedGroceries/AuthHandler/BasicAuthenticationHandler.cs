using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.AuthHandler
{

	/// <summary>
	/// https://jasonwatmore.com/post/2019/10/21/aspnet-core-3-basic-authentication-tutorial-with-example-api#startup-cs
	/// </summary>
	public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{

		public const string AuthenticationScheme = "BasicAuthentication";


		public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
		{
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			AuthenticateResult result;

			if (!Request.Headers.ContainsKey("Authorization"))
			{
				result = AuthenticateResult.Fail("Missing authorization header");
			}
			else
			{
				bool success = false;
				string username = string.Empty;
				string password = string.Empty;

				try
				{
					var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
					var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
					var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
					username = credentials[0];
					password = credentials[1];
				}
				catch
				{
					result = AuthenticateResult.Fail("Invalid authorization header");
				}

				await Task.Run(() =>
				{
					success = (username == AppSettings.Instance.Auth.UserName && password == AppSettings.Instance.Auth.Password);
				});

				if (success)
				{
					var claims = new[] {
						new Claim(ClaimTypes.Name, username),
					};
					var identity = new ClaimsIdentity(claims, Scheme.Name);
					var principal = new ClaimsPrincipal(identity);
					var ticket = new AuthenticationTicket(principal, Scheme.Name);

					result = AuthenticateResult.Success(ticket);
				}
				else
				{
					result = AuthenticateResult.Fail("Invalid user name or password");
				}
			}

			return result;
		}
	}
}
