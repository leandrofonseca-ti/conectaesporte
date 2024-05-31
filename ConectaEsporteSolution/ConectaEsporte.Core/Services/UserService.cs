using ConectaEsporte.Core.Database;
using ConectaEsporte.Core.Models;
using ConectaEsporte.Core.Services.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Services
{
	public class UserService : IUserRepository
	{
		private readonly AppDbContext _dbContext;
		public UserService(AppDbContext context)
		{
			_dbContext = context;
		}

		public Task<bool> ForgotPassword(string email)
		{
			var entity = _dbContext.user.Where(t => t.Email == email).FirstOrDefault();

			if (entity != null && entity.Id > 0)
			{
				// TODO: Send Recover Email
				return Task.FromResult(true);
			}
			return Task.FromResult(false);
		}

		public async Task<UserEntity> Login(string email, string password, EnumProfile profile)
		{
			var passwordEncrypt = Util.EncodePassword(password);

			var entity =
					await (from usr in _dbContext.user
						   join usrprof in _dbContext.userprofile on usr.Id equals usrprof.UserId
						   join prof in _dbContext.profile on usrprof.ProfileId equals prof.Id
						   where (prof.Id == profile.GetHashCode()) && usr.Email == email && usr.Password == passwordEncrypt
						   select new UserEntity
						   {
							   Created_Date = usr.Created_Date,
							   Email = usr.Email,
							   Name = usr.Name,
							   Id = usr.Id,
							   Phone = usr.Phone,
							   Picture = usr.Picture
						   }).FirstOrDefaultAsync();

			if (entity != null && entity.Id > 0)
			{
				entity.Profiles = await LoadProfiles(entity.Id);
				return entity;
			}
			return null;
		}


		public async Task<UserEntity> Login(int userid, EnumProfile profile)
		{

			var entity =
					await (from usr in _dbContext.user
						   join usrprof in _dbContext.userprofile on usr.Id equals usrprof.UserId
						   join prof in _dbContext.profile on usrprof.ProfileId equals prof.Id
						   where (prof.Id == profile.GetHashCode()) && usr.Id == userid
						   select new UserEntity
						   {
							   Created_Date = usr.Created_Date,
							   Email = usr.Email,
							   Name = usr.Name,
							   Id = usr.Id,
							   Phone = usr.Phone,
							   Picture = usr.Picture
						   }).FirstOrDefaultAsync();

			if (entity != null && entity.Id > 0)
			{
				entity.Profiles = await LoadProfiles(entity.Id);
				return entity;
			}
			return null;
		}
		public async Task<List<Profile>> LoadProfiles(int id)
		{

			var entity =
					await (from usrprof in _dbContext.userprofile
						   join prof in _dbContext.profile on usrprof.ProfileId equals prof.Id
						   where usrprof.UserId == id
						   select new Profile
						   {
							   Id = prof.Id,
							   Name = prof.Name,
						   }).ToListAsync();

			return entity;
		}




		public async Task<List<User>> GetUsers()
		{
			return await _dbContext.user.ToListAsync();
		}

		public async Task<User> GetUserById(int id)
		{
			return await _dbContext.user.FindAsync(id);
		}

		public async Task<User> GetUserByEmail(string email)
		{
			return await _dbContext.user.Where(t => t.Email == email).FirstOrDefaultAsync();
		}

		public async Task<UserEntity> AddUser(UserEntity user)
		{
			var resultExists = await _dbContext.user.AnyAsync(r => r.Email == user.Email);
			if (!resultExists)
			{
				user.Password = Util.EncodePassword(user.Password);
				var result = await _dbContext.user.AddAsync(
					 new UserEntity
					 {
						 Created_Date = user.Created_Date,
						 Email = user.Email,
						 Name = user.Name,
						 Password = user.Password,
						 Phone = user.Phone,
						 Picture = user.Picture,
					 });

				var resCode = await _dbContext.SaveChangesAsync();

				var resultProfile = await _dbContext.userprofile.AddAsync(
					 new UserProfile
					 {
						 ProfileId = user.Profiles.FirstOrDefault().Id,
						 UserId = result.Entity.Id
					 });
				var resCode2 = await _dbContext.SaveChangesAsync();

				return new UserEntity
				{
					Id = result.Entity.Id,
					Created_Date = result.Entity.Created_Date,
					Email = result.Entity.Email,
					Name = result.Entity.Name,
					Password = result.Entity.Password,
					Phone = result.Entity.Phone,
					Picture = result.Entity.Picture,
					Profiles = user.Profiles
				};
			}
			return new UserEntity();

		}

		public async Task UpdateUser(User user)
		{
			_dbContext.Entry(user).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteUser(int id)
		{
			var entity = await _dbContext.user.FindAsync(id);
			_dbContext.user.Remove(entity);
			await _dbContext.SaveChangesAsync();
		}


	}
}
