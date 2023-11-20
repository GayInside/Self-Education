using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthVideo.Domain.ServiceInterfaces
{
    public interface IContentService
    {
        public string GetBase64Code(string pathToFile);

        public byte[] GetByteArray(string base64Code);

        public string GetFileExtencion(byte[] bytes);

        public string GetFileName(string pathToFile);
    }
}
