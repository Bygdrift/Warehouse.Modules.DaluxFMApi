using System;
using Module.Services.Models.Helpers;

namespace Module.Services.Models
{
    public class Floor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string alternativeName { get; set; }
        public string buildingName { get; set; }
        public int buildingID { get; set; }
        public string floorType { get; set; }
        public float height { get; set; }
        public float grossarea { get; set; }
        public float netarea { get; set; }
        public DateTime lastChange { get; set; }
        public Userdefinedfield[] userDefinedFields { get; set; }
    }
}
