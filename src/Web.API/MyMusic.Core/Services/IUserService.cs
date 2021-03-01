using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);

        Task<IEnumerable<User>> GetAll();

        Task<User> Create(User user, string password);

        void Update(User user, string password = null);

        void Delete(int id);

        Task<User> GetUserById(int id);
    }
}
