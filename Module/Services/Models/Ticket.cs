using System;
using Module.Services.Models.Helpers;
using Newtonsoft.Json;

namespace Module.Services.Models
{
    public class Ticket
    {
        [JsonProperty("ID")]
        public int id { get; set; }
        public string number { get; set; }
        public string userEmail { get; set; }
        public string userPhoneNo { get; set; }
        public string userName { get; set; }
        public int? buildingMasterId { get; set; }
        public int? roomMasterId { get; set; }
        public int buildingPartMasterId { get; set; }
        public string lotMasterId { get; set; }
        public string gps { get; set; }
        public int topicMasterID { get; set; }
        public string description { get; set; }
        public string placementDescription { get; set; }
        public int status { get; set; }
        public string responsible { get; set; }
        public int teamID { get; set; }
        public string lastModified { get; set; }
        public DateTime? createdDate { get; set; }
        public History[] history { get; set; }
        public Userdefinedfield[] userDefinedFields { get; set; }
    }

    public class History
    {
        public string title { get; set; }
        public string date { get; set; }
        public Line[] lines { get; set; }
        public object[] downloadRequestUrls { get; set; }
    }

    public class Line
    {
        public string title { get; set; }
        public string value { get; set; }
    }
}




