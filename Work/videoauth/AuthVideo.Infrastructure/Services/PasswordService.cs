using AuthVideo.Domain.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthVideo.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        public string GetHashedPassword(string password)
        {
            using SHA256 hash = SHA256.Create();

            return Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(password)));
        }
    }
}
