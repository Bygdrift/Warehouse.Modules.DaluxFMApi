using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Services.Models
{
    public class AssetClassification
    {
        public int id { get; set; }
        public string code { get; set; }
        public string value { get; set; }
        public int? parentID { get; set; }
    }
}