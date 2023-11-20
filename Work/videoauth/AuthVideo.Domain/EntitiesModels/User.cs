using AuthVideo.Domain.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthVideo.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }
        
        public string? Login { get; set; }
        
        public string? PasswordHash { get; set; }
        
        public string? RefreshToken { get; set; }
        
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public UserRole Role { get; set; }
    }
}
