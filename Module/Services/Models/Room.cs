using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Services.Models.Helpers;

namespace Module.Services.Models
{
    public class Room
    {
        public int id { get; set; }
        public int floorID { get; set; }
        public float calculatedgrossarea { get; set; }
        public float netarea { get; set; }
        public DateTime? lastChange { get; set; }
        public Userdefinedfield[] userDefinedFields { get; set; }
    }
}
