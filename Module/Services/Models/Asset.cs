using System;
using Module.Services.Models.Helpers;

namespace Module.Services.Models
{
    public class Asset
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string disciplin { get; set; }
        public string classificationName { get; set; }
        public string classificationCode { get; set; }
        public int classificationID { get; set; }
        public int? classificationParentID { get; set; }
        public int buildingID { get; set; }
        public string gps { get; set; }
        public int roomID { get; set; }
        public int floorID { get; set; }
        public string[] bimInstanceID { get; set; }
        public string labels { get; set; }
        public string productName { get; set; }
        public int parentComponentID { get; set; }
        public DateTime? lastChange { get; set; }
        public Userdefinedfield[] userDefinedFields { get; set; }
        public string deepLink { get; set; }
    }
}
