using ConectaEsporte.API.Models;
using ConectaEsporte.API.Setup;
using ConectaEsporte.Core.Helper;
using ConectaEsporte.Core.Models;
using ConectaEsporte.Core.Services.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
        private readonly IServiceRepository _serviceRepository;
        public APIController(IUserRepository userRepository, IConfiguration configuration, IServiceRepository serviceRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _serviceRepository = serviceRepository;
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
                var dateTimeExpire = DateTime.Now.AddMinutes(30);
                var tokenString = BuildToken(dateTimeExpire);
                response = Ok(new { token = tokenString, expired = dateTimeExpire.ToString("yyyy-MM-dd HH:mm:ss") });
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

        private string BuildToken(DateTime dateTimeExpire)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                expires: dateTimeExpire,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        #endregion

        //[Authorize]
        //[HttpGet("Authenticate/Status")]
        //public async Task<IActionResult> AuthenticateStatus()
        //{
        //    var json = new LargeJsonResult();
        //    IActionResult response = Unauthorized();
        //    try
        //    {
        //        response = Ok(true);

        //        json.Value = new
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //        };
        //    }
        //    catch (Exception ex) { }  

        //    try
        //    {
      

        //    }
        //    catch (Exception ex)
        //    {
        //        json.Value = new
        //        {
        //            StatusCode = HttpStatusCode.BadRequest,
        //            Data = ex
        //        };
        //    }
        //    return json;
        //}


        [Authorize]
        [HttpPost("Authenticate/Login")]
        public async Task<IActionResult> AuthenticateLogin(LoginModel login)
        {
            var json = new LargeJsonResult();
            try
            {
                var result = await _userRepository.AddOrUpdate(login.Key, login.Name, login.Email, login.Fcm, login.Phone, login.PhotoUrl);
                var resultFinal = new UserModel();
                if (result != null && result.Id > 0)
                {
                    resultFinal = new UserModel { Id = result.Id, Name = result.Name, Email = result.Email, Picture = result.Picture, Key = result.KeyMobile, Fcm = result.Fcm, Phone = result.Phone };
                }

                json.Value = new
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = resultFinal
                };
            }
            catch (Exception ex)
            {
                json.Value = new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = ex
                };
            }
            return json;
        }

        /*
        [Authorize]
        [HttpPost("Authenticate/SyncLogin")]
        public async Task<IActionResult> SyncLogin(UserModel login)
        {
            var json = new LargeJsonResult();

            var result = await _userRepository.UpdateUserMobile(login.Key, login.Name, login.Email, login.Fcm, login.Phone, login.Picture);
            if (result != null && result.Id > 0)
            {
                json.Value = new UserModel { Id = result.Id, Name = result.Name, Email = result.Email };
            }
            return json;
        }
        */


        [Authorize]
        [HttpPost("Payment/List")]
        public async Task<IActionResult> PaymentList(LoginMailModel user)
        {
            var json = new LargeJsonResult();
            try
            {

                var resultPlanGroup = await _serviceRepository.ListPlanGroup();

                var result = await _serviceRepository.GetPlanUser(user.Email);

                var active = false;
                var willexpire = false;


                if (result != null)
                {
                    var dtNow = DateTime.Now;
                    if (result.Created >= dtNow && result.Created <= dtNow)
                    {
                        active = true;
                    }

                    var r = dtNow.Subtract(result.Created);
                    willexpire = r.TotalDays <= 7;

                    var resultItem = new PlanUserEntity
                    {
                        Created = result.Created,
                        Finished = result.Finished,
                        Id = result.Id,
                        PlanId = result.PlanId,
                        UserId = result.UserId,
                    };


                    json.Value = new
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = new PaymentModel
                        {
                            Plans = resultPlanGroup,
                            PlanSelected = resultItem,
                            UserEmail = user.Email,
                            UserId = resultItem.UserId,
                            Active = active,
                            WillExpire = willexpire,
                            Id = resultItem.Id,
                        }
                    };
                }
                else
                {
                    json.Value = new
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = new PaymentModel
                        {
                            Plans = resultPlanGroup,
                            PlanSelected = null,
                            UserEmail = user.Email,
                            UserId = 0,
                            Active = active,
                            WillExpire = willexpire,
                            Id = 0,
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                json.Value = new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = ex
                };
            }
            return json;
        }



        [Authorize]
        [HttpPost("Dashboard/Get")]
        public async Task<IActionResult> DashboardGet(LoginMailModel user)
        {
            var json = new LargeJsonResult();
            try
            {

                var result = _serviceRepository.GetUserByEmail(user.Email).Result;

                var totalNotification = _serviceRepository.TotalNotification(user.Email).Result;

                var resultCheckin = _serviceRepository.ListCheckin(user.Email).Result;

                var listDone = new List<CheckinDetailModel>();
                var listToday = new List<CheckinDetailModel>();
                var listNext = new List<CheckinDetailModel>();

                var dtNow = DateTime.Now;

                foreach (var item in resultCheckin.Where(r => r.BookedDt < dtNow && r.Booked == true))
                {
                    listDone.Add(new CheckinDetailModel
                    {
                        Booked = item.Booked,
                        BookedDt = item.BookedDt,
                        FromEmail = item.FromEmail,
                        FromName = item.FromName,
                        id = item.Id,
                        Title = item.Title,
                    });
                }


                var dtIni = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 0, 0, 0);
                var dtFim = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 23, 59, 59);
                foreach (var item in resultCheckin.Where(r => (r.BookedDt >= dtIni && r.BookedDt <= dtFim)))
                {
                    listToday.Add(new CheckinDetailModel
                    {
                        Booked = item.Booked,
                        BookedDt = item.BookedDt,
                        FromEmail = item.FromEmail,
                        FromName = item.FromName,
                        id = item.Id,
                        Title = item.Title,
                    });
                }


                foreach (var item in resultCheckin.Where(r => r.BookedDt > dtFim))
                {
                    listNext.Add(new CheckinDetailModel
                    {
                        Booked = item.Booked,
                        BookedDt = item.BookedDt,
                        FromEmail = item.FromEmail,
                        FromName = item.FromName,
                        id = item.Id,
                        Title = item.Title,
                    });
                }

                var totalCheckin = listToday.Count + listNext.Count;

                json.Value = new
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new DashModel
                    {
                        Email = result.Email,
                        Name = result.Name,
                        Picture = result.Picture,
                        Phone = result.Phone,
                        ListDone = listDone,
                        ListNext = listNext,
                        ListToday = listToday,
                        TotalCheckin = totalCheckin,
                        TotalNotification = totalNotification
                    }
                };

            }
            catch (Exception ex)
            {
                json.Value = new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = ex
                };
            }
            return json;
        }


        [Authorize]
        [HttpPost("Checkin/List")]
        public async Task<IActionResult> CheckinList(LoginCheckinModel user)
        {

            var json = new LargeJsonResult();
            try
            {

                var result = new List<CheckinDetailModel>();

                var resultCheckin = _serviceRepository.ListCheckin(user.Email).Result;

                var dtNow = DateTime.Now;
                var dtIni = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 0, 0, 0);
                foreach (var item in resultCheckin.Where(r => (r.BookedDt >= dtIni && r.Booked == user.Booked)))
                {
                    result.Add(new CheckinDetailModel
                    {
                        Booked = item.Booked,
                        BookedDt = item.BookedDt,
                        FromEmail = item.FromEmail,
                        FromName = item.FromName,
                        id = item.Id,
                        Title = item.Title,
                    });
                }


                json.Value = new
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = result
                };

            }
            catch (Exception ex)
            {
                json.Value = new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = ex
                };
            }
            return json;
        }


        [Authorize]
        [HttpPost("Checkin/Get")]
        public async Task<IActionResult> CheckinGet(LoginCheckinModel user)
        {

            var json = new LargeJsonResult();
            try
            {

                var item = _serviceRepository.GetCheckin(user.Email, user.Id).Result;

                var result = new CheckinDetailModel
                {
                    Booked = item.Booked,
                    BookedDt = item.BookedDt,
                    FromEmail = item.FromEmail,
                    FromName = item.FromName,
                    id = item.Id,
                    Title = item.Title,
                };

                json.Value = new
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = result
                };


            }
            catch (Exception ex)
            {
                json.Value = new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = ex
                };
            }
            return json;
        }


        [Authorize]
        [HttpPost("Checkin/Set")]
        public async Task<IActionResult> CheckinSet(LoginCheckinModel user)
        {

            var json = new LargeJsonResult();
            try
            {

                var item = _serviceRepository.SetCheckin(user.Email, user.Id, user.Booked).Result;

                json.Value = new
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = item
                };


            }
            catch (Exception ex)
            {
                json.Value = new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = ex
                };
            }
            return json;
        }

        //[Authorize]
        //[HttpPost("Notification/Add")]
        //public async Task<IActionResult> NotificationAdd(NotificationModel model)
        //{
        //    var result = _serviceRepository.AddNotification(new Notification()
        //    {
        //        Email = model.Email,
        //        CheckinId = model.CheckinId,
        //        Created = model.Created,
        //        FromEmail = model.SenderEmail,
        //        FromName = model.SenderName,
        //        FromPicture = model.SenderImage,
        //        IsRead = model.IsRead,
        //        Text = model.Text,
        //        Title = model.Title

        //    });
        //    var json = new LargeJsonResult();
        //    json.Value = result;
        //    return json;
        //}

        [Authorize]
        [HttpPost("Notification/List")]
        public async Task<IActionResult> GetNotifications(LoginMailModel user)
        {
            var json = new LargeJsonResult();
            try
            {
                var resultNotification = _serviceRepository.ListNotification(user.Email).Result;

                var listResult = new List<NotificationModel>();

                var dtNow = DateTime.Now;
                var dt7before = dtNow.AddDays(-7);
                var dtIni = new DateTime(dt7before.Year, dt7before.Month, dt7before.Day, 0, 0, 0);

                foreach (var item in resultNotification.Where(r => r.Created >= dtIni))
                {
                    listResult.Add(new NotificationModel
                    {
                        Created = item.Created,
                        SenderImage = item.FromPicture,
                        SenderEmail = item.FromEmail,
                        SenderName = item.FromName,
                        CheckinId = item.CheckinId,
                        IsRead = item.IsRead,
                        Id = item.Id,
                        Text = item.Text,
                        Title = item.Title,
                        Email = user.Email,
                    });
                }


                json.Value = new
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = listResult
                };

            }
            catch (Exception ex)
            {
                json.Value = new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = ex
                };
            }
            return json;
        }


        [Authorize]
        [HttpPost("Notification/UpdateRead")]
        public async Task<IActionResult> NotificationUpdateRead(ParamIdentity model)
        {
            var json = new LargeJsonResult();
            try
            {
                var result = _serviceRepository.UpdateNotificationRead(new Notification()
                {
                    Id = model.Id
                });


                json.Value = new
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                json.Value = new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = ex
                };
            }
            return json;
        }


        //[Authorize]
        //[HttpPost("Notification/Remove")]
        //public async Task<IActionResult> NotificationRemove(long id)
        //{
        //    var result = await _serviceRepository.RemoveNotification(id);
        //    var json = new LargeJsonResult();
        //    json.Value = result;
        //    return json;
        //}

    }
}
