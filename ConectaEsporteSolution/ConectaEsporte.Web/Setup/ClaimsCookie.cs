using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ConectaEsporte.Web.Setup
{
	public class ClaimsCookie
	{
		private readonly ClaimsPrincipal _user;
		private readonly HttpContext _httpContext;
		public ClaimsCookie(ClaimsPrincipal user, HttpContext httpContext = null)
		{
			_user = user;
			_httpContext = httpContext;
		}

		public string GetValue(KeyName keyName)
		{
			var principal = _user as ClaimsPrincipal;
			var cp = principal.Identities.First(i => i.AuthenticationType == (CookieAuthenticationDefaults.AuthenticationScheme).ToString());
			return cp.FindFirst(((KeyName)keyName).ToString()).Value;
		}
		public async void SetValue(KeyName[] keyName, string[] value)
		{
			if (keyName.Length != value.Length)
			{
				return;
			}
			var principal = _user as ClaimsPrincipal;

			if (principal.Identity.IsAuthenticated)
			{
				var cp = principal.Identities.First(i => i.AuthenticationType == (CookieAuthenticationDefaults.AuthenticationScheme).ToString());
				for (int i = 0; i < keyName.Length; i++)
				{
					if (cp.FindFirst(((KeyName)keyName[i]).ToString()) != null)
					{
						cp.RemoveClaim(cp.FindFirst(((KeyName)keyName[i]).ToString()));
						cp.AddClaim(new Claim(((KeyName)keyName[i]).ToString(), value[i]));
					}

				}
				await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(cp),
					new AuthenticationProperties
					{
						IsPersistent = true,
						AllowRefresh = true
					});
			}
			else
			{
				List<Claim> claims = new List<Claim>();
				for (int i = 0; i < keyName.Length; i++)
				{
					if (!claims.Any(y => y.Type == ((KeyName)keyName[i]).ToString()))
					{
						claims.Add(new Claim(((KeyName)keyName[i]).ToString(), value[i].ToString()));
					}
				}

				ClaimsIdentity identity = new ClaimsIdentity(claims,
					CookieAuthenticationDefaults.AuthenticationScheme);

				AuthenticationProperties properties = new AuthenticationProperties()
				{
					AllowRefresh = true,
					IsPersistent = true//model.KeepLogin,
				};

				await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
					new ClaimsPrincipal(identity),
					new AuthenticationProperties
					{
						IsPersistent = true,
						AllowRefresh = true
					});

			}
		}

		public enum KeyName
		{
			UserName = 1,
			UserEmail = 2,
			UserProfileId = 3,
			UserPicture = 4,
			UserProfiles = 5,
            UserId = 6,
        }
	}
}
