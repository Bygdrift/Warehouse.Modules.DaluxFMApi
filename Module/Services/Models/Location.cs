using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Services.Models
{
    public class Location
    {
        public int id { get; set; }
        public string name { get; set; }
        public object description { get; set; }
        public object parent { get; set; }
        public bool isMunicipality { get; set; }
    }
}
