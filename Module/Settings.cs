using Bygdrift.Warehouse.Attributes;

namespace Module
{
    public class Settings
    {
        [ConfigSetting(NotSet = NotSet.ThrowError, ErrorMessage = "ScheduleExpression has to be set")]
        public string ScheduleExpression { get; set; }

        [ConfigSetting(NotSet = NotSet.ThrowError, ErrorMessage = "Resourcegroup has to be set")]
        public string ResourceGroup { get; set; }

        [ConfigSetting(Default = "Linux")]
        public string ContainerOperatingSystem { get; set; }

        [ConfigSetting]
        public string Container1Name { get; set; }

        [ConfigSetting]
        public string Container1Image { get; set; }
        
        [ConfigSetting]
        public string Container1Variables { get; set; }

        [ConfigSetting]
        public string Container2Name { get; set; }

        [ConfigSetting]
        public string Container2Image { get; set; }
        
        [ConfigSetting]
        public string Container2Variables { get; set; }

        [ConfigSetting]
        public string Container3Name { get; set; }

        [ConfigSetting]
        public string Container3Image { get; set; }

        [ConfigSetting]
        public string Container3Variables { get; set; }

        [ConfigSetting(Default = 10000)]
        public int MonthsToKeepDataInDataLake { get; set; }
    }
}
