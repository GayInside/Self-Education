using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthVideo.Domain.Entities
{
    public class Content
    {
        public long Id { get; set; }

        public string? Title { get; set; }

        public string? VideoURL { get; set; }

        public string? ImageURL { get; set; }

        public string? Description { get; set; }
    }
}
