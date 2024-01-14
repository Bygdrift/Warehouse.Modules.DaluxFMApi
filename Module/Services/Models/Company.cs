using Module.Services.Models.Helpers;

namespace Module.Services.Models
{
    public class Company
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string organisationID { get; set; }
        public bool isActive { get; set; }
        public string address { get; set; }
        public string zipCode { get; set; }
        public string phoneNumber { get; set; }
        public string city { get; set; }
        public string email { get; set; }
        public string[] disciplines { get; set; }
        public string[] companyTypes { get; set; }
        public Userdefinedfield[] userDefinedFields { get; set; }
    }
}
