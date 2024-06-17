using ConectaEsporte.Core.Database;
using ConectaEsporte.Core.Models;
using ConectaEsporte.Core.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.user.Where(t => t.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<Checkin>> ListCheckin(string email)
        {
            return await _dbContext.checkin.Where(t => t.Email == email).ToListAsync();
        }

        public async Task<Checkin> GetCheckin(string email, long id)
        {
            return await _dbContext.checkin.Where(t => t.Email == email && t.Id == id).FirstOrDefaultAsync();
        }
        public async Task<int> TotalNotification(string email)
        {
            return await _dbContext.notification.Where(t => t.Email == email).CountAsync();
        }


        public async Task<bool> RemoveNotification(long id)
        {
            var entity = await _dbContext.notification.Where(t => t.Id == id).SingleOrDefaultAsync();

            if(entity != null)
            {
                _dbContext.Remove(entity);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<Notification>> ListNotification(string email)
        {
            return await _dbContext.notification.Where(t => t.Email == email).ToListAsync();
        }


        public async Task<List<Plan>> ListPlan()
        {
            return await _dbContext.plan.Where(t=>t.Active == true).OrderBy(t => t.Order).ToListAsync();
        }

        public async Task<PlanUser> GetPlanUser(string email)
        {
            var query =  (from users in _dbContext.user
                         join plans in _dbContext.planuser on users.Id equals plans.UserId
                         where users.Email == email
                         select plans).ToListAsync();

            return query.Result.FirstOrDefault();
        }

        public bool AddNotification(Notification entity)
        {
            _dbContext.notification.Add(entity);
             _dbContext.SaveChanges();
            return true;
        }
    }
}
