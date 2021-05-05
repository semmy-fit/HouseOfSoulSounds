using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseOfSoulSounds.Areas.Admin.Models
{
    public class NewPage
    {
        public Guid Id { get; set; }
        public string PageTitle { get; set; }
        public string ImagePage { get; set; }
        public string BaseText { get; set; }
        public string Saidbar_text { get; set; }
    }
}
