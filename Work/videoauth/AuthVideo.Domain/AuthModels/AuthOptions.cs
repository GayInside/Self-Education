using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthVideo.Domain.AuthModels
{
    public class AuthOptions
    {
        public const string ISSUER = "MyServer";

        public const string AUDIENCE = "MyClient";

        private const string KEY = "PizdezKakoiSecretKey";

        public const int LIFETIME = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}
