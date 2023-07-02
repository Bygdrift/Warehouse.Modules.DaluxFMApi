using Azure.ResourceManager.ContainerInstance.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Bygdrift.Warehouse;

namespace Module.AppFunctions.Models
{
    public class Container
    {
        public Container(AppBase<Settings> app, string containerName, string image, double memoryInGB, double cpu, string variablesJson)
        {
            App = app;
            ContainerName = containerName;
            Image = image;
            MemoryInGB = memoryInGB;
            Cpu = cpu;
            Variables = GetVaribles(variablesJson);
        }

        public AppBase<Settings> App { get; }
        public string ContainerName { get; }
        public string Image { get; }
        public double MemoryInGB { get; }
        public double Cpu { get; }
        public Dictionary<string, string> Variables { get; }

        private Dictionary<string, string> GetVaribles(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;

            try
            {
                var jArray = JsonConvert.DeserializeObject<JArray>(json);
                return jArray.ToDictionary(k => ((JObject)k).Properties().First().Name, v => v.Values().First().Value<string>());
            }
            catch (Exception e)
            {
                App.Log.LogWarning($"Could not convert Variable correct. Error: {e.Message}");
                return null;
            }
        }
    }
}
