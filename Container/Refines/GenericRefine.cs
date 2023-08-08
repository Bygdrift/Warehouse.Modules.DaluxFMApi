using Bygdrift.Tools.CsvTool;
using Bygdrift.Tools.DataLakeTool;
using Bygdrift.Warehouse;
using Container.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Container.Refines
{
    public static class GenericRefine
    {
        public static async Task<Csv> RefineAsync<T>(AppBase app, IEnumerable<T> data) where T : class
        {
            var csv = CreateCsv(data);
            var name = typeof(T).Name;
            await app.DataLake.SaveCsvAsync(csv, "Refined", name + ".csv", FolderStructure.DatePath);
            app.Mssql.MergeCsv(csv, name, "id", true, false);
            return csv;
        }

        private static Csv CreateCsv<T>(IEnumerable<T> data)
        {
            var csv = new Csv();
            var row = 1;
            foreach (var item in data)
            {
                foreach (PropertyInfo prop in item.GetType().GetProperties())
                    if (prop.PropertyType.IsArray)
                    {
                        if (prop.PropertyType == typeof(Userdefinedfield[]))
                            foreach (var field in prop.GetValue(item) as Userdefinedfield[])
                                csv.AddRecord(row, field.name, field.value);
                        else
                        {
                            var res = new JArray();
                            if (prop.GetValue(item) is object[] values)
                                foreach (var value in values)
                                    res.Add(value);

                            csv.AddRecord(row, prop.Name, res.Count == 0 ? null : res.ToString(Formatting.None));
                        }
                    }
                    else
                        csv.AddRecord(row, prop.Name, prop.GetValue(item));
                row++;
            }
            return csv;
        }
    }
}