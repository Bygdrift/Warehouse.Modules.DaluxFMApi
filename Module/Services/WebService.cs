using Bygdrift.Warehouse;
using Module.Services.Models.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Module.Services
{
    public class WebService
    {
        private DateTime lastDownloadOfAccessToken;
        private HttpClient _client;
        private readonly string baseUrl = "https://fm-api.dalux.com/api/v1/";
        public HttpResponseMessage ClientResponse { get; private set; }

        public AppBase<Settings> App { get; }

        public WebService(AppBase<Settings> app) => App = app;

        public async Task<MetaDataExtend<T>> GetDataAsync<T>(int maxMinutes, int bookmark = 0, int? take = null) where T : class
        {
            var subUrl = new ApiName().GetName<T>();
            return await GetDataAsync<T>(subUrl, maxMinutes, bookmark, take);
        }

        public HttpClient Client
        {
            get
            {
                if (_client == null || lastDownloadOfAccessToken.AddHours(1) < DateTime.Now)
                {
                    _client = GetHttpClient();
                    lastDownloadOfAccessToken = DateTime.Now;
                }

                return _client;
            }
        }

        internal HttpClient GetHttpClient()
        {
            var handler = new HttpClientHandler();
            var client = new HttpClient(handler);
            client.Timeout = new TimeSpan(10, 0, 0);
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("X-API-KEY", App.Settings.DaluxFMApiKey);
            return client;
        }

        private async Task<MetaDataExtend<T>> GetDataAsync<T>(string url, int maxMinutes, int bookmark = 0, int? take = null) where T : class
        {
            var res = new List<T>();
            int limit = 100;
            int breaker = 0;
            var stopTime = DateTime.Now.AddMinutes(maxMinutes);
            Root<T> data;
            if (take != null && take < limit)
                limit = (int)take;

            while (true)
            {
                data = await GetDataPackageAsync<T>(url, bookmark, limit);
                if (data == null)
                    return res.Any() ? new MetaDataExtend<T>(res, res.Count, "0", "0") : default;

                res.AddRange(data.items.Select(o => o.data));

                if (!int.TryParse(data.metadata.nextBookmark, out bookmark))
                    break;

                if (bookmark == 0)
                    break;

                if (take != null && take == res.Count)
                    break;

                if (DateTime.Now > stopTime)
                    break;

                breaker++;
                if (breaker > 10000)  //1.000.000 posts - there is an error
                    throw new Exception("A programmer has to look at this error!");
            }
            return new MetaDataExtend<T>(res, data.metadata.totalItems, data.metadata.bookmark, data.metadata.nextBookmark);
        }

        private async Task<Root<T>> GetDataPackageAsync<T>(string url, int bookmark, int limit) where T : class
        {
            var response = await Client.GetAsync($"{url}?bookmark={bookmark}&limit={limit}");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                App.Log.LogError($"The webservice '{url}' failed while trying to fetch from bookmark {bookmark}, with limit {limit}. Error: {response.ReasonPhrase}.");
                return default;
            }
            try
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Root<T>>(json);
            }
            catch (Exception)
            {
                App.Log.LogError($"The webservice '{url}' loaded data from bookmark {bookmark}, with limit {limit} successfully. But when trying to convert data into the model i DaluxFMApi, this error was raised: {response.ReasonPhrase}.");
                return default;
            }
        }

        /// <summary>
        /// Denne virker desværre ikke fordi man ikke kan lave parallelle kald til Dalux. De overvejer at gøre det muligt
        /// </summary>
        private async Task<List<T>> GetDataAsyncParrallel_NotWorking<T>(string url, int? take = null) where T : class
        {
            int limit = 100;
            var tasks = new List<Task<Root<T>>>();

            if (take != null && take < limit)
                limit = (int)take;

            tasks.Add(GetDataPackageAsync<T>(url, 1, limit));
            await Task.WhenAll(tasks);

            var totalItems = tasks.First()?.Result.metadata.totalItems;
            if (totalItems == null)
                return null;

            if (take != null && take < totalItems)
                totalItems = take;

            var maxBookmark = Math.Floor((int)totalItems / limit + 0d);
            for (int i = 0; i < maxBookmark; i++)
                tasks.Add(GetDataPackageAsync<T>(url, i, limit));

            await Task.WhenAll(tasks);

            return tasks.SelectMany(o => o.Result.items.Select(p => p.data)).ToList();
        }
    }
}
