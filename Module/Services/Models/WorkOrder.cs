using Module.Services.Models.Helpers;
using Newtonsoft.Json;
using System;

namespace Module.Services.Models
{
    public class WorkOrder
    {
        [JsonProperty("ID")]
        public int id { get; set; }
        public int workOrderNumber { get; set; }
        public string name { get; set; }
        public object status { get; set; }
        public int[] linkedWorkOrderIDs { get; set; }
        public int templateID { get; set; }
        public int teamID { get; set; }
        public string responsible { get; set; }
        public string description { get; set; }
        public int priorityID { get; set; }
        public string type { get; set; }
        public int? buildingID { get; set; }
        public int? roomID { get; set; }
        public int? assetID { get; set; }
        public string gps { get; set; }
        public object placementDescription { get; set; }
        public DateTime? deadline { get; set; }
        public int duration { get; set; }
        public DateTime? closedDate { get; set; }
        public int expectedCost { get; set; }
        public DateTime? expectedExecutionDate { get; set; }
        public string accountName { get; set; }
        public string accountNumber { get; set; }
        public int companyID { get; set; }
        public Userdefinedfield[] userDefinedFields { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime? lastChange { get; set; }
        public bool isStatutory { get; set; }
        public bool hasWarranty { get; set; }
        public int[] buildingIDs { get; set; }
        public int[] roomIDs { get; set; }
    }
}
