using Bygdrift.CsvTools;
using Bygdrift.DataLakeTools;
using Bygdrift.Warehouse;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Module.AppFunctions;
using Module.Services.Models.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Module.Refines
{
    public static class GenericRefine
    {
        public static async Task<Csv> RefineAsync<T>(AppBase app, IEnumerable<T> data, bool truncateTable) where T : class
        {
            var now = DateTime.UtcNow.ToString("HH-mm-ss");
            var name = typeof(T).Name;
            var errors = app.Log.GetErrorsAndCriticals().Count();
            app.Log.LogInformation("- Refine '{name}' data and save to datalake and database.", name);
            var csv = CreateCsv(data);
            RemoveDuplicatedIds(app, csv, "id", name);
            await app.DataLake.SaveCsvAsync(csv, "Refined", $"{name}_{now}.csv", FolderStructure.DatePath);
            app.Mssql.MergeCsv(csv, name, "id", truncateTable, false);

            if (errors != app.Log.GetErrorsAndCriticals().Count())
            {
                var text = string.Join(".\n", app.Log.GetLogs());
                await app.DataLake.SaveStringAsync(text, "Errors", $"{name}_{now}.txt", FolderStructure.DatePath);
            }
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
                            {
                                if(field.name != null)
                                    csv.AddRecord(row, field.name, field.value);
                            }
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

        private static void RemoveDuplicatedIds(AppBase app, Csv csv, string uniqueIdHeaderName, string name)
        {
            var ids = csv.GetColRecords(uniqueIdHeaderName, false);
            var groupsWithMultipleIds = ids.GroupBy(o => o.Value).Where(o => o.Count() > 1);
            if (groupsWithMultipleIds.Any())
            {
                app.Log.LogInformation("- There are {n} concurrent id's in '{name}'. The problem has been solved by removing all none distinct data.", groupsWithMultipleIds.Count(), name);
                foreach (var item in groupsWithMultipleIds)
                {
                    var first = true;
                    foreach (var sub in item)
                    {
                        if (first)
                            first = false;
                        else
                            csv.RemoveRow(sub.Key);
                    }
                }
            }
        }
    }
}