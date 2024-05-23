using ConectaEsporte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Services.Repositories
{
	public interface IUserRepository
	{
		Task<UserEntity> Login(string email, string password, EnumProfile profile);
		Task<bool> ForgotPassword(string email);

		Task<List<User>> GetUsers();

		Task<User> GetUserById(int id);

		Task<User> GetUserByEmail(string email);

		Task<UserEntity> AddUser(UserEntity user);

		Task UpdateUser(User user);

		Task DeleteUser(int id);

	}
}
