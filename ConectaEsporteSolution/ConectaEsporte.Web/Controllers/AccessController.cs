using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ConectaSolution.Models;
using ConectaEsporte.Core.Services.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using ConectaEsporte.Web.Setup;
using System.Net;
using ConectaEsporte.Web.Models;
using ConectaEsporte.Core;


namespace ConectaSolution.Controllers
{
	public class AccessController : Controller
	{
		private readonly IUserRepository _userRepository;
		public AccessController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public IActionResult LoginAluno()
		{
			ClaimsPrincipal claimUser = HttpContext.User;

			if (claimUser.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Dashboard");


			return View();
		}

		public IActionResult LoginProfessor()
		{
			ClaimsPrincipal claimUser = HttpContext.User;

			if (claimUser.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Dashboard");


			return View();
		}

		//public IActionResult LoginMaster()
		//{
		//	ClaimsPrincipal claimUser = HttpContext.User;

		//	if (claimUser.Identity.IsAuthenticated)
		//		return RedirectToAction("Index", "Dashboard");


		//	return View();
		//}

		public IActionResult ForgotAluno()
		{
			ClaimsPrincipal claimUser = HttpContext.User;

			if (claimUser.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Dashboard");


			return View();
		}


        public IActionResult ForgotProfessor()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");


            return View();
        }

        public IActionResult RegisterAluno()
		{
			ClaimsPrincipal claimUser = HttpContext.User;


			if (claimUser.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Dashboard");


			return View();
		}

		public IActionResult RegisterProfessor()
		{
			ClaimsPrincipal claimUser = HttpContext.User;


			if (claimUser.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Dashboard");


			return View();
		}


		[HttpPost]
		public async Task<JsonResult> LoginMaster(VMLogin model)
		{
			var _email = model.Email;
			var _password = model.Password;
			var result = await _userRepository.Login(_email, _password, EnumProfile.Master);
			if (result != null && result.Id > 0)
			{
				ClaimsCookie cc = new ClaimsCookie(this.HttpContext.User, this.HttpContext);

				ClaimsCookie.KeyName[] keys = new[] {
					ClaimsCookie.KeyName.UserEmail,
					ClaimsCookie.KeyName.UserName,
					ClaimsCookie.KeyName.UserPicture,
					ClaimsCookie.KeyName.UserProfiles,
					ClaimsCookie.KeyName.UserProfileId
				};

				String[] values = new[] {
					result.Email,
					result.Name,
					result.Picture,
					String.Join(",", result.Profiles.Select(y => y.Id)),
					result.Profiles.FirstOrDefault().Id.ToString()};

				cc.SetValue(keys, values);

				ConfigurationBuilder builder = new ConfigurationBuilder();
				builder.AddJsonFile(System.IO.Path.Combine(AppContext.BaseDirectory, "appsettings.json"), false, true);
				var configRoot = builder.Build();
				var rootUrl = configRoot.GetValue<string>("AppSettings:RootUrl");
				var url = string.Concat(rootUrl, "/Dashboard/Index");
				return new JsonResult(new { ErrorMessage = "", Data = true, Redirect = url });
				//return RedirectToAction("Index", "Dashboard");
			}

			return new JsonResult(new { ErrorMessage = "", Data = false, Message = "Problemas ao realizar login!" });

		}

		[HttpPost]
		public async Task<JsonResult> LoginProfessor(VMLogin model)
		{
			var _email = model.Email;
			var _password = model.Password;
			var result = await _userRepository.Login(_email, _password, EnumProfile.Professor);
			if (result != null && result.Id > 0)
			{

				ClaimsCookie cc = new ClaimsCookie(this.HttpContext.User, this.HttpContext);

				ClaimsCookie.KeyName[] keys = new[] {
					ClaimsCookie.KeyName.UserEmail,
					ClaimsCookie.KeyName.UserName,
					ClaimsCookie.KeyName.UserPicture,
					ClaimsCookie.KeyName.UserProfiles,
					ClaimsCookie.KeyName.UserProfileId
				};

				String[] values = new[] {
					result.Email,
					result.Name,
					result.Picture,
					String.Join(",", result.Profiles.Select(y => y.Id)),
					result.Profiles.FirstOrDefault().Id.ToString()};

				cc.SetValue(keys, values);


				ConfigurationBuilder builder = new ConfigurationBuilder();
				builder.AddJsonFile(System.IO.Path.Combine(AppContext.BaseDirectory, "appsettings.json"), false, true);
				var configRoot = builder.Build();
				var rootUrl = configRoot.GetValue<string>("AppSettings:RootUrl");
				var url = string.Concat(rootUrl, "Dashboard/Index");
				return new JsonResult(new { ErrorMessage = "", Data = true, Redirect = url });
			}

			return new JsonResult(new { ErrorMessage = "", Data = false, Message = "Problemas ao realizar login!" });

		}

		[HttpPost]
		public async Task<JsonResult> LoginAluno(VMLogin model)
		{
			var _email = model.Email;
			var _password = model.Password;
			var result = await _userRepository.Login(_email, _password, EnumProfile.Aluno);
			if (result != null && result.Id > 0)
			{

				ClaimsCookie cc = new ClaimsCookie(this.HttpContext.User, this.HttpContext);

				ClaimsCookie.KeyName[] keys = new[] {
					ClaimsCookie.KeyName.UserEmail,
					ClaimsCookie.KeyName.UserName,
					ClaimsCookie.KeyName.UserPicture,
					ClaimsCookie.KeyName.UserProfiles,
					ClaimsCookie.KeyName.UserProfileId
				};

				String[] values = new[] {
					result.Email,
					result.Name,
					result.Picture,
					String.Join(",", result.Profiles.Select(y => y.Id)),
					result.Profiles.FirstOrDefault().Id.ToString()};

				cc.SetValue(keys, values);


				ConfigurationBuilder builder = new ConfigurationBuilder();
				builder.AddJsonFile(System.IO.Path.Combine(AppContext.BaseDirectory, "appsettings.json"), false, true);
				var configRoot = builder.Build();
				var rootUrl = configRoot.GetValue<string>("AppSettings:RootUrl");
				var url = string.Concat(rootUrl, "Dashboard/Index");
				return new JsonResult(new { ErrorMessage = "", Data = true, Redirect = url });
			}

			return new JsonResult(new { ErrorMessage = "", Data = false, Message = "Problemas ao realizar login!" });

		}

		[HttpPost]
		public async Task<JsonResult> ForgotAluno(VMLogin model)
		{
			var _email = model.Email;
			var result = await _userRepository.GetUserByEmail(_email);
			if (result != null && result.Id > 0)
			{
				return new JsonResult(new { ErrorMessage = "", Data = true, Redirect = Url.Action("Index", "Dashboard") });
			}

			return new JsonResult(new { ErrorMessage = "", Data = false, Message = "Problemas ao recuperar senha!" });
		}

        [HttpPost]
        public async Task<JsonResult> ForgotProfessor(VMLogin model)
        {
            var _email = model.Email;
            var result = await _userRepository.GetUserByEmail(_email);
            if (result != null && result.Id > 0)
            {
                return new JsonResult(new { ErrorMessage = "", Data = true, Redirect = Url.Action("Index", "Dashboard") });
            }

            return new JsonResult(new { ErrorMessage = "", Data = false, Message = "Problemas ao recuperar senha!" });
        }

        [HttpPost]
		public async Task<JsonResult> RegisterAluno(VMLogin model)
		{

			var resultExists = await _userRepository.GetUserByEmail(model.Email);
			if (resultExists != null && resultExists.Id > 0)
			{
				return new JsonResult(new { ErrorMessage = "", Data = false, Message = "Usuário já cadastrado!" });

			}
			else
			{
				var result = await _userRepository.AddUser(new ConectaEsporte.Core.Models.UserEntity()
				{
					Email = model.Email,
					Name = model.Name,
					Password = model.Password,
					Phone = model.Phone,
					Picture = "https://cdn.icon-icons.com/icons2/1446/PNG/512/22278owl_98790.png",
					Created_Date = DateTime.Now,
					Profiles = new List<ConectaEsporte.Core.Models.Profile> { new ConectaEsporte.Core.Models.Profile() { Id = EnumProfile.Aluno.GetHashCode() } }
				});

				if (result != null && result.Id > 0)
				{

					ClaimsCookie cc = new ClaimsCookie(this.HttpContext.User, this.HttpContext);

					ClaimsCookie.KeyName[] keys = new[] {
					ClaimsCookie.KeyName.UserEmail,
					ClaimsCookie.KeyName.UserName,
					ClaimsCookie.KeyName.UserPicture,
					ClaimsCookie.KeyName.UserProfiles,
					ClaimsCookie.KeyName.UserProfileId
				};

					String[] values = new[] {
					result.Email,
					result.Name,
					result.Picture,
					String.Join(",", result.Profiles.Select(y => y.Id)),
					result.Profiles.FirstOrDefault().Id.ToString()};

					cc.SetValue(keys, values);

					//	List<Claim> claims = new List<Claim>()
					//{
					//	new Claim(ClaimTypes.Email, result.Email),
					//	new Claim(ClaimTypes.Name, result.Name),
					//	new Claim("UserPicture", result.Picture)
					//};


					//	ClaimsIdentity identity = new ClaimsIdentity(claims,
					//		CookieAuthenticationDefaults.AuthenticationScheme);

					//	AuthenticationProperties properties = new AuthenticationProperties()
					//	{
					//		AllowRefresh = true,
					//		IsPersistent = true//model.KeepLogin,
					//	};

					//	await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
					//		new ClaimsPrincipal(identity), properties);

					return new JsonResult(new { ErrorMessage = "", Data = true, Redirect = Url.Action("Index", "Dashboard") });

				}
			}

			return new JsonResult(new { ErrorMessage = "", Data = false, Message = "Problemas para cadastrar Usuário!" });
		}


		[HttpPost]
		public async Task<JsonResult> RegisterProfessor(VMLogin model)
		{

			var resultExists = await _userRepository.GetUserByEmail(model.Email);
			if (resultExists != null && resultExists.Id > 0)
			{
				return new JsonResult(new { ErrorMessage = "", Data = false, Message = "Usuário já cadastrado!" });

			}
			else
			{
				var result = await _userRepository.AddUser(new ConectaEsporte.Core.Models.UserEntity()
				{
					Email = model.Email,
					Name = model.Name,
					Password = model.Password,
					Phone = model.Phone,
					Picture = "https://cdn.icon-icons.com/icons2/1446/PNG/512/22278owl_98790.png",
					Created_Date = DateTime.Now,
					Profiles = new List<ConectaEsporte.Core.Models.Profile> { new ConectaEsporte.Core.Models.Profile() { Id = EnumProfile.Professor.GetHashCode() } }
				});

				if (result != null && result.Id > 0)
				{

					ClaimsCookie cc = new ClaimsCookie(this.HttpContext.User, this.HttpContext);

					ClaimsCookie.KeyName[] keys = new[] {
					ClaimsCookie.KeyName.UserEmail,
					ClaimsCookie.KeyName.UserName,
					ClaimsCookie.KeyName.UserPicture,
					ClaimsCookie.KeyName.UserProfiles,
					ClaimsCookie.KeyName.UserProfileId
				};

					String[] values = new[] {
					result.Email,
					result.Name,
					result.Picture,
					String.Join(",", result.Profiles.Select(y => y.Id)),
					result.Profiles.FirstOrDefault().Id.ToString()};

					cc.SetValue(keys, values);

					//	List<Claim> claims = new List<Claim>()
					//{
					//	new Claim(ClaimTypes.Email, result.Email),
					//	new Claim(ClaimTypes.Name, result.Name),
					//	new Claim("UserPicture", result.Picture)
					//};


					//	ClaimsIdentity identity = new ClaimsIdentity(claims,
					//		CookieAuthenticationDefaults.AuthenticationScheme);

					//	AuthenticationProperties properties = new AuthenticationProperties()
					//	{
					//		AllowRefresh = true,
					//		IsPersistent = true//model.KeepLogin,
					//	};

					//	await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
					//		new ClaimsPrincipal(identity), properties);

					return new JsonResult(new { ErrorMessage = "", Data = true, Redirect = Url.Action("Index", "Dashboard") });

				}
			}

			return new JsonResult(new { ErrorMessage = "", Data = false, Message = "Problemas para cadastrar Usuário!" });
		}
	}
}
