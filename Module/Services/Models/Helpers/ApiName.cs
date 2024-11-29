using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Services.Models.Helpers
{
    public class ApiName
    {
        public List<ApiNameClass> ApiNames { get; set; } = new List<ApiNameClass>
        {
            new (typeof(Asset), "asset", JsonType.Data),
            new (typeof(AssetClassification), "assetClassifications", JsonType.Data),
            new (typeof(Building), "buildings", JsonType.Data),
            new (typeof(Company), "companies", JsonType.Data),
            new (typeof(Document), "documents", JsonType.Data),
            new (typeof(Estate), "estates", JsonType.Data),
            new (typeof(Floor), "floors", JsonType.Data),
            new (typeof(Location), "locations", JsonType.Data),
            new (typeof(Lot), "lots", JsonType.Data),
            new (typeof(Room), "rooms", JsonType.Data),
            new (typeof(Ticket), "tickets", JsonType.Data),
            new (typeof(WorkOrder), "workorders", JsonType.Data),
            new (typeof(WorkOrderTeam), "workorders/teams", JsonType.Items),
        };

        public JsonType GetJsonType<T>() where T : class
        {
            var res = ApiNames.FirstOrDefault(o => o.Type == typeof(T))?.JsonType;
            return res ?? throw new NotImplementedException();
        }

        public string GetUrl<T>() where T : class
        {
            var res = ApiNames.FirstOrDefault(o=> o.Type == typeof(T))?.Url;
            return res ?? throw new NotImplementedException();
        }

        public Type GetType(string name)
        {
            var res = ApiNames.FirstOrDefault(o => o.Type.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Type;
            return res ?? throw new NotImplementedException();
        }



        public bool Compare<T>(string name) where T : class => typeof(T).Equals(GetType(name));
    }

    public class ApiNameClass
    {
        public ApiNameClass(Type type, string url, JsonType jsonType)
        {
            Type = type;
            TypeName = type.Name;
            Url = url;
            JsonType = jsonType;
        }

        public Type Type { get; set; }
        public string TypeName { get; set; }
        public string Url { get; set; }
        public JsonType JsonType { get; set; }
    }

    

    /// <summary>
    /// Der er to responses fra Dalux: 
    /// </summary>
    public enum JsonType
    {
        /// <summary>
        /// "items": [{"data": {...
        /// </summary>
        Data,
        /// <summary>
        /// "items": [...
        /// </summary>
        Items,
    }
}
