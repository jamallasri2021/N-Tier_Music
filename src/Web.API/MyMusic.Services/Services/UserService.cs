using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            return await _UnitOfWork.Users.Authenticate(username, password);
        }

        public async Task<User> Create(User user, string password)
        {
            var userResult = await _UnitOfWork.Users.Create(user, password);

            await _UnitOfWork.CommitAsync();

            return userResult;
        }

        public void Delete(int id)
        {
            _UnitOfWork.Users.Delete(id);
            _UnitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _UnitOfWork.Users.GetAllUsersAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _UnitOfWork.Users.GetUserByIdAync(id);
        }

        public void Update(User user, string password = null)
        {
            _UnitOfWork.Users.Update(user, password);
            _UnitOfWork.CommitAsync();
        }
    }
}
