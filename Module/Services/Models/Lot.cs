using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Services.Models.Helpers;

namespace Module.Services.Models
{
    public class Lot
    {
        public int id { get; set; }
        public string name { get; set; }
        public string estateName { get; set; }
        public string[] userRegion { get; set; }
        public int estateID { get; set; }
        public string alternativeName { get; set; }
        public string road { get; set; }
        public string number { get; set; }
        public string zipCode { get; set; }
        public string city { get; set; }
        public DateTime? lastChange { get; set; }
        public Userdefinedfield[] userDefinedFields { get; set; }
    }
}
