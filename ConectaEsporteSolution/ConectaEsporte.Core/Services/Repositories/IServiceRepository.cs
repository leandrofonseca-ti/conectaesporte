﻿using ConectaEsporte.Core.Helper;
using ConectaEsporte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Services.Repositories
{
    public interface IServiceRepository
    {
        Task<Checkin> GetCheckin(string email, long id);

        Task<bool> SetCheckin(string email, long id, bool booked);
        Task<User> GetUserByEmail(string email);
        Task<List<Checkin>> ListCheckin(string email);
        Task<List<Notification>> ListNotification(string email);
        Task<List<Plan>> ListPlan();
        Task<List<PlanEntity>> ListPlanGroup();
        Task<PlanUserEntity> GetPlanUser(string email);
        Task<int> TotalNotification(string email);
        bool AddNotification(Notification entity);
        Task<bool> UpdateNotificationRead(Notification entity);
        Task<bool> RemoveNotification(long id);
        Task<PlanBuildEntity> GetPlanBuild(string email, long planId);
        Task<bool> UpdatePaymentUser(string email, decimal amount, string description, long ownerid);

        //Task<bool> UpdateAmountPayment(string email);

        Task<List<RoomClassEntity>> ListRoomType(string email, EnumTypeRoom eventEnum, int pageIndex);

        Task<int> TotalRoomType(string email, EnumTypeRoom eventEnum);
        Task<RoomClassEntity> GetRoomType(string email, long id);
        Task<List<UserViewEntity>> ListPeopleEntity(long roomid);
        Task<List<UserViewEntity>> ListPeopleEntity(long roomid, bool confirmed);
    }
}
