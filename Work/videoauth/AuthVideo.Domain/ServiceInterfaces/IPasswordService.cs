using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthVideo.Domain.ServiceInterfaces
{
    public interface IPasswordService
    {
        public string GetHashedPassword(string password);
    }
}
