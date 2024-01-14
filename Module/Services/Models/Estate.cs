using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Services.Models.Helpers;

namespace Module.Services.Models
{
    public class Estate
    {
        public int id { get; set; }
        public string name { get; set; }
        public string locationName { get; set; }
        public int locationID { get; set; }
        public string description { get; set; }
        public DateTime? lastChange { get; set; }
        public Userdefinedfield[] userDefinedFields { get; set; }
    }
}
