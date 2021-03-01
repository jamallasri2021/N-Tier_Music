using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Core
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public bool IsAdmin { get; set; }
    }
}
