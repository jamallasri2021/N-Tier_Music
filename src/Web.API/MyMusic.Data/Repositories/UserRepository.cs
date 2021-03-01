using Microsoft.EntityFrameworkCore;
using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        #region Properties & Contructors
        private MyMusicDbContext MyMusicDbContext
        {
            get { return Context as MyMusicDbContext; }
        }
        public UserRepository(MyMusicDbContext context) : base(context)
        {

        }
        #endregion

        #region Functions
        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = await MyMusicDbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Password is required");
            }

            var resultUser = await MyMusicDbContext.Users.AnyAsync(u => u.UserName == user.UserName);

            if (resultUser)
            {
                throw new Exception($"UserName : {user.UserName} is already taken");
            }

            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await MyMusicDbContext.Users.AddAsync(user);

            return user;
        }

        public void Delete(int id)
        {
            var user = MyMusicDbContext.Users.Find(id);

            if (user != null)
            {
                MyMusicDbContext.Users.Remove(user);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await MyMusicDbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAync(int id)
        {
            return await MyMusicDbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public void Update(User user, string password = null)
        {
            var userResult = MyMusicDbContext.Users.Find(user.Id);

            if (userResult == null)
            {
                throw new Exception("User not found");
            }

            // Check if the new username is already taken
            if (MyMusicDbContext.Users.Any(u => u.UserName == user.UserName))
            {
                throw new Exception($"This username({user.UserName}) is already taken");
            }

            // Update
            userResult.FirstName = user.FirstName;
            userResult.LastName = user.LastName;
            userResult.UserName = user.UserName;

            // Update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;

                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                userResult.PasswordHash = passwordHash;
                userResult.PasswordSalt = passwordSalt;
            }

            MyMusicDbContext.Users.Update(userResult);
        }

        /// <summary>
        /// Verifing password authentication
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty or white space!", "password");
            }

            if (passwordHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash(64 bytes expected)", "passwordHash");
            }

            if (passwordSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt(128 bytes expected)", "passwordSalt");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Generate password hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty or white space!", "password");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        #endregion
    }
}
