using AuthVideo.Domain.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthVideo.Infrastructure.Services
{
    public class ContentService : IContentService
    {
        public string GetBase64Code(string pathToFile)
        {
            if (!File.Exists(pathToFile))
            {
                throw new FileNotFoundException(pathToFile);
            }

            byte[] fileBytes = File.ReadAllBytes(pathToFile);
            return Convert.ToBase64String(fileBytes);
        }

        public byte[] GetByteArray(string base64Code)
        {
            return Convert.FromBase64String(base64Code);
        }

        public string GetFileExtencion(byte[] bytes)
        {
            if (bytes.Length > 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
            {
                return ".jpg";
            }
            else if (bytes.Length > 3 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E)
            {
                return ".png";
            }
            else if (bytes.Length > 3 && bytes[0] == 0x00 && bytes[1] == 0x00 && bytes[2] == 0x00)
            {
                return ".mp4";
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public string GetFileName(string pathToFile)
        {
            return Path.GetFileName(pathToFile) ?? throw new ArgumentNullException();
        }
    }
}
