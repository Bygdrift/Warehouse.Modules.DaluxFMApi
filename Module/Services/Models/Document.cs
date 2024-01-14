using System;
using Module.Services.Models.Helpers;

namespace Module.Services.Models
{
    public class Document
    {
        public int id { get; set; }
        public string fileName { get; set; }
        public int?[] buildingID { get; set; }
        public int?[] assetID { get; set; }
        public string size { get; set; }
        public int version { get; set; }
        public DateTime? lastChange { get; set; }
        public string uploadedBy { get; set; }
        public DateTime? uploadTime { get; set; }
        public Userdefinedfield[] userDefinedFields { get; set; }
    }
}
