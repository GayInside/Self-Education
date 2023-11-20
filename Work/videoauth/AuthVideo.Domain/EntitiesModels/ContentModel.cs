using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthVideo.Domain.EntitiesModels
{
    public class ContentModel
    {
        public string? Title { get; set; }

        public IFormFile? Video { get; set; }

        public IFormFile? Image { get; set; }

        public string? Description { get; set; }
    }
}
