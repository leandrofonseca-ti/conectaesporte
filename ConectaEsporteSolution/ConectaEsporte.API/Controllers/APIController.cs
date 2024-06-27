using ConectaEsporte.API.Models;
using ConectaEsporte.API.Setup;
using ConectaEsporte.Core;
using ConectaEsporte.Core.Helper;
using ConectaEsporte.Core.Models;
using ConectaEsporte.Core.Services.Repositories;
using ConectaEsporte.Uol.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
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
        private readonly IPaymentRepository _paymentRepository;
        public APIController(IUserRepository userRepository, IConfiguration configuration, IServiceRepository serviceRepository, IPaymentRepository paymentRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _serviceRepository = serviceRepository;
            _paymentRepository = paymentRepository;
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



        [Authorize]
        [HttpPost("Authenticate/Login")]
        public async Task<IActionResult> AuthenticateLogin(LoginModel login)
        {
            var json = new LargeJsonResult();
            try
            {
                var result = await _userRepository.AddOrUpdateMobile(new ConectaEsporte.Core.Models.UserEntity()
                {
                    KeyMobile = login.Key,
                    Fcm = login.Fcm,
                    Email = login.Email,
                    Name = login.Name,
                    Password = string.Empty,
                    Phone = login.Phone,
                    Picture = login.PhotoUrl,// "https://cdn.icon-icons.com/icons2/1446/PNG/512/22278owl_98790.png",
                    Created_Date = DateTime.Now,
                    Profiles = new List<ConectaEsporte.Core.Models.Profile> { new ConectaEsporte.Core.Models.Profile() { Id = EnumProfile.Aluno.GetHashCode() } }
                });

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

                    json.Value = new
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = new PaymentModel
                        {
                            Plans = resultPlanGroup,
                            PlanSelected = result,
                            UserEmail = user.Email,
                            UserId = result.UserId,
                            Active = active,
                            WillExpire = willexpire,
                            Id = result.Id,
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
        [HttpPost("Payment/Add")]
        public async Task<IActionResult> PaymentAdd(LoginPaymentSetModel user)
        {
            var json = new LargeJsonResult();
            try
            {
                var userDetail = await _serviceRepository.GetUserByEmail(user.Email);
                var phoneDDD = Util.GetPhoneDDD(userDetail.Phone);
                var phoneNumber = Util.GetPhoneNumber(userDetail.Phone);
                var resultPayment = _paymentRepository.CreatePayment(new Uol.Models.PaymentItemEntity()
                {
                    Code = "1000",
                    UserReference = string.Format("REF{0}", userDetail.Id),
                    Description = user.Description,
                    Price = user.Amount,
                    UserEmail = user.Email,
                    UserName = userDetail.Name,
                    UserPhone = phoneNumber,
                    UserPhoneDDD = phoneDDD
                },
                  new Uol.Models.AppSetupEntity()
                  {
                      RedirectUri = "https://conectaesporte.com/checkoutresponse",
                  });

                if (resultPayment.Errors.Count == 0 && string.IsNullOrEmpty(resultPayment.MessageError))
                {
                    // TODO: API PAGAMENTO
                    //var result = await _serviceRepository.UpdatePaymentUser(user.Email, user.Amount, user.Description, user.OwnerId);

                    //if (result != null)
                    //{
                    //  await _serviceRepository.UpdateAmountPayment(user.Email);
                    json.Value = new
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = true,
                        Message = "",
                        Uri = resultPayment.Uri
                    };
                    //}
                    //else
                    //{
                    //    json.Value = new
                    //    {
                    //        StatusCode = HttpStatusCode.OK,
                    //        Data = false,
                    //        Message = "Problemas para gerar pagamento. Tente mais tarde!",
                    //    };
                    //}
                }
                else
                {
                    json.Value = new
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = false,
                        Message = resultPayment.MessageError,
                        Errors = resultPayment.Errors
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
        [HttpPost("Payment/Build")]
        public async Task<IActionResult> PaymentBuild(LoginPaymentModel user)
        {
            var json = new LargeJsonResult();
            try
            {
                var result = await _serviceRepository.GetPlanBuild(user.Email, user.PlanId);

                if (result != null)
                {
                    json.Value = new
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = new PaymentBuildModel
                        {
                            Created = result.Created,
                            Finished = result.Finished,
                            Price = result.Price,
                            Tax = result.Tax,
                            Total = result.Total,
                            Id = result.Id,
                        }
                    };
                }
                else
                {
                    json.Value = new
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = new PaymentBuildModel()
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
                var dtLast7Days = dtNow.AddDays(-7);

                foreach (var item in resultCheckin.Where(r => (r.BookedDt >= dtLast7Days && r.BookedDt < dtNow)))
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

                var resultUser = await _serviceRepository.GetPlanUser(user.Email);

                var listEventTop = await _serviceRepository.ListRoomType(user.Email, EnumTypeRoom.Event, 0);

                var listClassTop = await _serviceRepository.ListRoomType(user.Email, EnumTypeRoom.Class, 0);

                var totalEvent = await _serviceRepository.TotalRoomType(user.Email, EnumTypeRoom.Event);

                var totalClass = await _serviceRepository.TotalRoomType(user.Email, EnumTypeRoom.Class);

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
                        TotalNotification = totalNotification,
                        TotalClass = totalClass,
                        TotalEvent = totalEvent,
                        Amount = resultUser.Amount,
                        ListEventTop = listEventTop,
                        ListClassTop = listClassTop
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
                var dtFim = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 23, 59, 59);
                foreach (var item in resultCheckin.Where(r => (r.BookedDt >= dtIni && r.BookedDt <= dtFim)))
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

                foreach (var item in resultCheckin.Where(r => r.BookedDt > dtFim))
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
                    Data = result.OrderBy(t => t.BookedDt).ToList()
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
        [HttpPost("Event/List")]
        public async Task<IActionResult> EventList(ListEventModel user)
        {
            var json = new LargeJsonResult();
            try
            {
                var result = new List<RoomClassEntity>();

                if (user.Type.Equals("CLASS", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = await _serviceRepository.ListRoomType(user.Email, EnumTypeRoom.Class, user.PageIndex);
                }

                if (user.Type.Equals("EVENT", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = await _serviceRepository.ListRoomType(user.Email, EnumTypeRoom.Event, user.PageIndex);
                }

                json.Value = new
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = result,
                    PageIndex = user.PageIndex,
                    PageNext = user.PageIndex + 1
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
        [HttpPost("Event/Get")]
        public async Task<IActionResult> EventGet(ListEventDetailModel user)
        {
            var json = new LargeJsonResult();
            try
            {
                var result = new RoomClassEntity();

                result = await _serviceRepository.GetRoomType(user.Email, user.Id);

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
        public async Task<IActionResult> NotificationList(LoginMailModel user)
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
