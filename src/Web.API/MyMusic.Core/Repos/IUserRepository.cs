using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string username, string password);

        Task<User> Create(User user, string password);

        void Update(User user, string password = null);

        void Delete(int id);

        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User> GetUserByIdAync(int id);
    }
}
