using ConectaEsporte.API.Models;
using ConectaEsporte.API.Setup;
using ConectaEsporte.Core.Models;
using ConectaEsporte.Core.Services.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ConectaEsporte.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class APIController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public APIController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }



        #region ### CREATE TOKEN ###
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] AuthModel authentication)
        {

            IActionResult response = Unauthorized();
            var result = AuthenticateAsync(authentication);
            if (result)
            {
                var tokenString = BuildToken();
                response = Ok(new { token = tokenString });
            }

            return response;

        }

        private bool AuthenticateAsync(AuthModel authentication)
        {
            var result = false;
            var objKey = _configuration["ApiKeysJwt"];
            if (objKey != null && !String.IsNullOrEmpty(objKey) && objKey.Equals(authentication.Key))//,StringComparison.InvariantCultureIgnoreCase))
            {
                result = true;
            }

            return result;
        }

        private string BuildToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        #endregion


        [Authorize]
        [HttpGet("Authenticate/Status")]
        public async Task<IActionResult> StatusLogin()
        {
            IActionResult response = Unauthorized();
            try
            {
                response = Ok(true);
            }
            catch (Exception ex) { }

            return response;
        }

        [Authorize]
        [HttpPost("Authenticate/Login")]
        public async Task<IActionResult> CheckLogin(LoginModel login)
        {
            var json = new LargeJsonResult();
            //json.StatusCode = 200;
            var result = await _userRepository.Login(login.Username, login.Password, Core.EnumProfile.Aluno);
            if (result != null && result.Id > 0)
            {
                json.Value = new UserModel { Id = result.Id, Name = result.Name, Email = result.Email };
            }
            return json;
        }


        [Authorize]
        [HttpPost("Authenticate/SyncLogin")]
        public async Task<IActionResult> SyncLogin(UserModel login)
        {
            var json = new LargeJsonResult();
            //json.StatusCode = 200;
            var result = await _userRepository.UpdateUserMobile(login.Key, login.Name, login.Email, login.Fcm, login.Phone, login.Picture);
            if (result != null && result.Id > 0)
            {
                json.Value = new UserModel { Id = result.Id, Name = result.Name, Email = result.Email };
            }
            return json;
        }




        [Authorize]
        [HttpGet("GetPayments")]
        public async Task<IActionResult> GetPayments(UserModel user)
        {

            var json = new LargeJsonResult();
            //json.StatusCode = 200;


            json.Value = new Payment
            {
                OwnerId = 123,
                Plans = new List<PlanItem>() {
                 new PlanItem{ Id = 1, Description = "Gratuito",  Price = 0},
                 new PlanItem{ Id = 2, Description = "Mensal", Price = 150},
                 new PlanItem{ Id = 3, Description = "Trimestral", Price = 450},
                 new PlanItem{ Id = 4, Description = "Semestral", Price = 850},
                 new PlanItem{ Id = 4, Description = "Anual", Price = 1050},
                },

                PlanSelected = new PlanItem()
                {
                    Id = 1,
                    Created = new DateTime(2024, 05, 01)
                },
                UserEmail = user.Email,
                UserId = user.Id,
            };
            return json;
        }



        [Authorize]
        [HttpGet("GetNotifications")]
        public async Task<IActionResult> GetNotifications(UserModel user)
        {

            var json = new LargeJsonResult();
            //json.StatusCode = 200;


            json.Value = new List<NotificationModel>()
            {
                new NotificationModel()
                {
                    SenderImage = "https://images.unsplash.com/photo-1544005313-94ddf0286df2?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NDZ8fHByb2ZpbGV8ZW58MHx8MHx8fDA%3D&auto=format&fit=crop&w=900&q=60",
                    SenderEmail = "sender@gmail.com",
                     ContentId = 1001,
                     Created = DateTime.Now,
                      Id = 1,
                      Text = "Bla blal ablabla ablabalb alab lbabl ",
                       IsRead = true,
                       UserEmail = "leandrofonseca.ti@gmail.com",

                },
                new NotificationModel()
                {
                    SenderImage = "https://images.unsplash.com/photo-1580489944761-15a19d654956?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mzl8fHByb2ZpbGV8ZW58MHx8MHx8fDA%3D&auto=format&fit=crop&w=900&q=60",
                    SenderEmail = "sender2@gmail.com",
                    ContentId= 1002,
                     Created = DateTime.Now,
                      Id= 2,
                       Text = "Bla blal ablabla ablabalb alab lbabl ",
                       IsRead= false,
                        UserEmail = "leandrofonseca.ti@gmail.com",
                },          new NotificationModel()
                {
                    SenderImage = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MjB8fHByb2ZpbGV8ZW58MHx8MHx8fDA%3D&auto=format&fit=crop&w=900&q=60",
                    SenderEmail = "sender3@gmail.com",
                     ContentId = 1003,
                     Created = DateTime.Now,
                      Id = 1,
                      Text = "Bla blal ablabla ablabalb alab lbabl ",
                       IsRead = true,
                       UserEmail = "leandrofonseca.ti@gmail.com",

                },
                new NotificationModel()
                {
                    SenderImage = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MTh8fHByb2ZpbGV8ZW58MHx8MHx8fDA%3D&auto=format&fit=crop&w=900&q=60",
                    SenderEmail = "sender4@gmail.com",
                    ContentId= 1004,
                     Created = DateTime.Now,
                      Id= 2,
                       Text = "Bla blal ablabla ablabalb alab lbabl ",
                       IsRead= false,
                        UserEmail = "leandrofonseca.ti@gmail.com",
                }
            };

            return json;
        }


        /*
        [Authorize]
        [HttpPost("User/GetProfile")]
        public async Task<IActionResult> GetDashboard(IdentityModel model)
        {
            var result = await _userRepository.GetDashboard(model.Email);
            var json = new LargeJsonResult();
            json.Value = result;
            return json;
        }

        [Authorize]
        [HttpPost("Notification/List")]
        public async Task<IActionResult> GetNotifications(IdentityModel model)
        {
            var result = await _userRepository.GetNotifications(model.Email);
            var json = new LargeJsonResult();
            json.Value = result;
            return json;
        }


        [Authorize]
        [HttpPost("Notification/Add")]
        public async Task<IActionResult> NotificationAdd(IdentityModel model)
        {
            var result = await _userRepository.GetNotifications(model.Email);
            var json = new LargeJsonResult();
            json.Value = result;
            return json;
        }


        [Authorize]
        [HttpPost("Notification/RemoveAll")]
        public async Task<IActionResult> NotificationRemoveAll(IdentityModel model)
        {
            var result = await _userRepository.GetNotifications(model.Email);
            var json = new LargeJsonResult();
            json.Value = result;
            return json;
        }
        */
    }
}
