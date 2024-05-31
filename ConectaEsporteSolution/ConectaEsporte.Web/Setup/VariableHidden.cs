using ConectaEsporte.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ConectaEsporte.Web.Setup
{
	public class VariableHidden
	{
		//ClaimsPrincipal claimUser = null;
		HttpContext _httpContext = null;
		public VariableHidden(HttpContext httpContext)
		{
			//claimUser = _context.User;
			_httpContext = httpContext;
		}

		public bool IsAuthenticated 
		{
			get { return _httpContext.User.Identity.IsAuthenticated; }
		}


		public List<int> UserProfiles
		{
			get {
				if(!IsAuthenticated)
					return new List<int>();

				ClaimsCookie cc = new ClaimsCookie(_httpContext.User, _httpContext);
				var objString = cc.GetValue( ClaimsCookie.KeyName.UserProfiles);
				var codes = objString.Split(",");
				if(codes.Any())
				{
					return codes.Select(t => Int32.Parse(t)).ToList();
				}
				return new List<int>(); 
			}
		}


		public int UserProfileId
		{
			get
			{
                if (!IsAuthenticated)
                    return 0;

                ClaimsCookie cc = new ClaimsCookie(_httpContext.User, _httpContext);
				var objString = cc.GetValue( ClaimsCookie.KeyName.UserProfileId);
				return Int32.Parse(objString);
			}
			set
			{
				ClaimsCookie cc = new ClaimsCookie(_httpContext.User, _httpContext);
				ClaimsCookie.KeyName[] keys = new[] { ClaimsCookie.KeyName.UserProfileId };
				String[] values = new[] { value.ToString() };
				cc.SetValue( keys, values);
			}
		}


        public int UserId
        {
            get
            {
                if (!IsAuthenticated)
                    return 0;

                ClaimsCookie cc = new ClaimsCookie(_httpContext.User, _httpContext);
                var objString = cc.GetValue(ClaimsCookie.KeyName.UserId);
                return Int32.Parse(objString);
            }
            set
            {
                ClaimsCookie cc = new ClaimsCookie(_httpContext.User, _httpContext);
                ClaimsCookie.KeyName[] keys = new[] { ClaimsCookie.KeyName.UserId };
                String[] values = new[] { value.ToString() };
                cc.SetValue(keys, values);
            }
        }

        public string UserPicture
		{
			get
			{
                if (!IsAuthenticated)
                    return string.Empty;

                ClaimsCookie cc = new ClaimsCookie(_httpContext.User, _httpContext);
				var objString = cc.GetValue( ClaimsCookie.KeyName.UserPicture);
				return objString;
			}
		}
		public string UserEmail
		{
			get
			{
                if (!IsAuthenticated)
                    return string.Empty;

                ClaimsCookie cc = new ClaimsCookie(_httpContext.User, _httpContext);
				var objString = cc.GetValue( ClaimsCookie.KeyName.UserEmail);
				return objString;
			}
		}
		public string UserName
		{
			get
			{
                if (!IsAuthenticated)
                    return string.Empty;

                ClaimsCookie cc = new ClaimsCookie(_httpContext.User, _httpContext);
				var objString = cc.GetValue( ClaimsCookie.KeyName.UserName);
				return objString;
			}
		}
	}
}
