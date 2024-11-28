using System;
using Module.Services.Models.Helpers;

namespace Module.Services.Models
{
    public class Building
    {
        public int id { get; set; }
        public string name { get; set; }
        public string estateName { get; set; }
        public int estateID { get; set; }
        public string alternativeName { get; set; }
        public string road { get; set; }
        public string number { get; set; }
        public string zipCode { get; set; }
        public string city { get; set; }
        public bool owned { get; set; }
        public string label { get; set; }
        public string region { get; set; }
        public string[] userRegion { get; set; }
        public float grossArea { get; set; }
        public float netArea { get; set; }
        public DateTime? lastChange { get; set; }
        public DateTime? lastBIMChange { get; set; }
        public Userdefinedfield[] userDefinedFields { get; set; }
    }
}
