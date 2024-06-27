using ConectaEsporte.Core.Database;
using ConectaEsporte.Core.Helper;
using ConectaEsporte.Core.Models;
using ConectaEsporte.Core.Services.Repositories;
using MercadoPago.Resource.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Services
{
    public class GeralService : IServiceRepository
    {
        private readonly AppDbContext _dbContext;
        public GeralService(AppDbContext context)
        {
            _dbContext = context;
        }


        public async Task<Models.User> GetUserByEmail(string email)
        {
            return await _dbContext.user.Where(t => t.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<Checkin>> ListCheckin(string email)
        {
            return await _dbContext.checkin.Where(t => t.Email == email).OrderBy(t => t.BookedDt).ToListAsync();
        }

        public async Task<Checkin> GetCheckin(string email, long id)
        {
            return await _dbContext.checkin.Where(t => t.Email == email && t.Id == id).FirstOrDefaultAsync();
        }


        public async Task<bool> SetCheckin(string email, long id, bool booked)
        {
            var code = id;
            var entityResult = _dbContext.checkin.Where(t => t.Email == email && t.Id == code).FirstOrDefaultAsync().Result;

            if (entityResult != null)
            {
                entityResult.Booked = booked;
                entityResult.ConfirmDt = DateTime.Now;
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }
        public async Task<int> TotalNotification(string email)
        {
            var dtNow = DateTime.Now;
            var dtLast7 = dtNow.AddDays(-7);
            var dtIni = new DateTime(dtLast7.Year, dtLast7.Month, dtLast7.Day, 0, 0, 0);

            return await _dbContext.notification.Where(t =>
            t.Email == email
            && (t.Created >= dtIni)
            && t.IsRead == false).CountAsync();
        }


        public async Task<bool> RemoveNotification(long id)
        {
            var entity = await _dbContext.notification.Where(t => t.Id == id).SingleOrDefaultAsync();

            if (entity != null)
            {
                _dbContext.Remove(entity);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<Notification>> ListNotification(string email)
        {
            return await _dbContext.notification.Where(t => t.Email == email).OrderBy(t => t.Created).ToListAsync();
        }

        public async Task<bool> UpdatePaymentUser(string email, decimal amount, string description, long ownerid)
        {
            var userEntity = await _dbContext.user.Where(t => t.Email == email).SingleOrDefaultAsync();

            if (userEntity != null)
            {
                _dbContext.planuserhistory.Add(new PlanUserHistory()
                {
                    Created = DateTime.Now,
                    Description = description,
                    OwnerId = ownerid,
                    UserId = userEntity.Id,
                    Amount = amount
                });
                _dbContext.SaveChanges();

                return true;
            }
            return false;
        }



        public async Task<RoomClassEntity> GetRoomType(string email, long id)
        {
            var result = await (from rc in _dbContext.roomclass
                                join rcu in _dbContext.roomclassuser on rc.Id equals rcu.RoomClassId
                                join teacher in _dbContext.user on rc.OwnerId equals teacher.Id
                                where rc.Id == id
                                select new RoomClassEntity()
                                {
                                    Id = rc.Id,
                                    Title = rc.Title,
                                    Subtitle = teacher.Name,
                                    Picture = teacher.Picture,
                                    LocalId = rc.LocalId,
                                    OwnerId = rc.OwnerId,
                                })
                   .FirstOrDefaultAsync();

            if (result != null && result.Id > 0)
            {
                result.Local = await GetLocalEntity(result.LocalId);
            }



            if (result != null && result.Id > 0)
            {
                result.People = await ListPeopleEntity(result.Id);
            }
            return result;

        }

        public async Task<List<UserViewEntity>> ListPeopleEntity(long roomid)
        {
            var query = await (from rcu in _dbContext.roomclassuser
                               join usr in _dbContext.user on rcu.UserId equals usr.Id
                               where rcu.RoomClassId == roomid // && rcu.Confirmed == confirmed
                               select new UserViewEntity()
                               {
                                   Title = usr.Name,
                                   Picture = usr.Picture,
                                   OwnerId = usr.Id,
                                   Id = usr.Id,
                                   Confirmed = rcu.Confirmed
                               })
                        .OrderBy(y => y.Title)
                        .ToListAsync();

            return query;
        }

        public async Task<List<UserViewEntity>> ListPeopleEntity(long roomid, bool confirmed)
        {
            var query = await (from rcu in _dbContext.roomclassuser
                               join usr in _dbContext.user on rcu.UserId equals usr.Id
                               where rcu.RoomClassId == roomid && rcu.Confirmed == confirmed
                               select new UserViewEntity()
                               {
                                   Title = usr.Name,
                                   Picture = usr.Picture,
                                   OwnerId = usr.Id,
                                   Id = usr.Id,
                                   Confirmed = rcu.Confirmed
                               })
                        .OrderBy(y => y.Title)
                        .ToListAsync();

            return query;
        }


        private async Task<LocalEntity> GetLocalEntity(long localId)
        {
            var entity = await _dbContext.local.Where(t => t.Id == localId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return new LocalEntity()
                {
                    Description = entity.Description,
                    Id = entity.Id,
                    OwnerId = entity.OwnerId,
                    Picture = entity.Picture,
                    Title = entity.Title,
                };
            }
            return null;
        }

        public async Task<List<RoomClassEntity>> ListRoomType(string email, EnumTypeRoom eventEnum, int pageIndex)
        {
            var result = new List<RoomClassEntity>();
            var top = 10;
            if (eventEnum == EnumTypeRoom.Class)
            {
                var userEntity = await _dbContext.user.Where(t => t.Email == email).SingleOrDefaultAsync();

                if (userEntity != null)
                {
                    result = await (from rc in _dbContext.roomclass
                                    join rcu in _dbContext.roomclassuser on rc.Id equals rcu.RoomClassId
                                    join teacher in _dbContext.user on rc.OwnerId equals teacher.Id
                                    where rc.Type == "CLASS" && rcu.UserId == userEntity.Id
                                    select new RoomClassEntity()
                                    {
                                        Id = rc.Id,
                                        Title = rc.Title,
                                        Subtitle = teacher.Name,
                                        Picture = rc.Picture,
                                    })
                   .OrderBy(y => y.Title)
                   .Page(top, pageIndex)
                   .ToListAsync();
                }

            }

            if (eventEnum == EnumTypeRoom.Event)
            {
                var userEntity = await _dbContext.user.Where(t => t.Email == email).SingleOrDefaultAsync();

                if (userEntity != null)
                {
                    result = await (from rc in _dbContext.roomclass
                                    join rcu in _dbContext.roomclassuser on rc.Id equals rcu.RoomClassId
                                    join teacher in _dbContext.user on rc.OwnerId equals teacher.Id
                                    where rc.Type == "EVENT" && rcu.UserId == userEntity.Id
                                    select new RoomClassEntity()
                                    {
                                        Id = rc.Id,
                                        Title = rc.Title,
                                        Subtitle = teacher.Name,
                                        Picture = teacher.Picture,
                                    })
                   .OrderBy(y => y.Title)
                   .Page(top, pageIndex)
                   .ToListAsync();
                }
            }

            return result;
        }


        public async Task<int> TotalRoomType(string email, EnumTypeRoom eventEnum)
        {
            var result = 0;
            if (eventEnum == EnumTypeRoom.Class)
            {
                var userEntity = await _dbContext.user.Where(t => t.Email == email).SingleOrDefaultAsync();

                if (userEntity != null)
                {
                    result = await (from rc in _dbContext.roomclass
                                    join rcu in _dbContext.roomclassuser on rc.Id equals rcu.RoomClassId
                                    join teacher in _dbContext.user on rc.OwnerId equals teacher.Id
                                    where rc.Type == "CLASS" && rcu.UserId == userEntity.Id
                                    select new RoomClassEntity()
                                    {
                                        Id = rc.Id,
                                        Title = rc.Title,
                                        Subtitle = teacher.Name,
                                        Picture = rc.Picture,
                                    })
                   .CountAsync();
                }

            }

            if (eventEnum == EnumTypeRoom.Event)
            {
                var userEntity = await _dbContext.user.Where(t => t.Email == email).SingleOrDefaultAsync();

                if (userEntity != null)
                {
                    result = await (from rc in _dbContext.roomclass
                                    join rcu in _dbContext.roomclassuser on rc.Id equals rcu.RoomClassId
                                    join teacher in _dbContext.user on rc.OwnerId equals teacher.Id
                                    where rc.Type == "EVENT" && rcu.UserId == userEntity.Id
                                    select new RoomClassEntity()
                                    {
                                        Id = rc.Id,
                                        Title = rc.Title,
                                        Subtitle = teacher.Name,
                                        Picture = teacher.Picture,
                                    })
                   .CountAsync();
                }
            }

            return result;
        }
        public async Task<bool> UpdateAmountPayment(string email)
        {
            var userEntity = await _dbContext.user.Where(t => t.Email == email).SingleOrDefaultAsync();

            if (userEntity != null)
            {
                var entities = await _dbContext.planuserhistory.Where(t => t.UserId == userEntity.Id).ToListAsync();

                if (entities != null)
                {
                    decimal total = 0;
                    entities.ForEach(t =>
                    {
                        total += t.Amount;
                    });

                    var userEntityPlan = await _dbContext.planuser.Where(t => t.UserId == userEntity.Id).SingleOrDefaultAsync();

                    if (userEntityPlan != null)
                    {
                        userEntityPlan.Total = total;
                        _dbContext.SaveChanges();
                    }
                }
                return true;
            }
            return false;
        }


        public async Task<PlanBuildEntity> GetPlanBuild(string email, long planId)
        {
            var plan = await _dbContext.plan.Where(t => t.Id == planId && t.Active == true).SingleOrDefaultAsync();
            var plantax = await _dbContext.plantax.Where(t => t.Name == "PLAN_TAX" && t.Active == true).SingleOrDefaultAsync();

            if (plantax != null && plan != null)
            {
                var tax = plantax.Price;
                var price = plan.Price;
                var dtIni = DateTime.Now;
                var dtFim = dtIni.AddMonths(plan.PeriodMonth);

                var taxFinal = decimal.Round((price * tax), 2, MidpointRounding.AwayFromZero);

                var totalFinal = decimal.Round(price + (price * tax), 2, MidpointRounding.AwayFromZero);
                return new PlanBuildEntity()
                {
                    Price = price,
                    Tax = taxFinal,
                    Total = totalFinal,
                    Created = dtIni,
                    Finished = dtFim,
                };
            }

            return new PlanBuildEntity();
        }

        public async Task<List<Plan>> ListPlan()
        {
            return await _dbContext.plan.Where(t => t.Active == true).OrderBy(t => t.Order).ToListAsync();
        }


        public async Task<List<PlanEntity>> ListPlanGroup()
        {
            var query = (from groups in _dbContext.plangroup
                         join plans in _dbContext.plan on groups.Id equals plans.GroupId
                         where plans.Active == true && groups.Active == true
                         select new PlanEntity()
                         {
                             Active = plans.Active,
                             Description = plans.Description,
                             Id = plans.Id,
                             Name = plans.Name,
                             Order = plans.Order,
                             Price = plans.Price,
                             ProfileIds = groups.ProfileIds,
                             GroupDescription = groups.Description,
                             GroupId = groups.Id,
                             GroupName = groups.Name,
                             GroupOrder = groups.Order,
                         })
                         .OrderBy(y => y.GroupOrder)
                         .ThenBy(y => y.Order)
                         .ToListAsync();

            return query.Result;
        }

        public async Task<PlanUserEntity> GetPlanUser(string email)
        {
            var query = (from users in _dbContext.user
                         join plansuser in _dbContext.planuser on users.Id equals plansuser.UserId
                         where users.Email == email
                         select new PlanUserEntity
                         {
                             Created = plansuser.Created,
                             Finished = plansuser.Finished,
                             Amount = plansuser.Total,
                             Id = plansuser.Id,
                             UserId = plansuser.UserId
                         }).ToListAsync();

            return query.Result.FirstOrDefault();
        }

        public bool AddNotification(Notification entity)
        {
            _dbContext.notification.Add(entity);
            _dbContext.SaveChanges();
            return true;
        }
        public async Task<bool> UpdateNotificationRead(Notification entity)
        {
            var code = entity.Id;
            var entityResult = _dbContext.notification.Where(t => t.Id == code).FirstOrDefaultAsync().Result;

            if (entityResult != null)
            {
                entityResult.IsRead = true;
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }


    }
}
