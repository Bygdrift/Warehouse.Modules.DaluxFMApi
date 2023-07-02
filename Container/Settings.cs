using Bygdrift.Warehouse.Helpers.Attributes;

namespace Container
{
    public class Settings
    {
        /// <summary>An ID that you can get by contacting Dalux and say that it is to their web service. The Id would look like: f07axb-ex28-49ax-b3xd-faxa01217dd5"</summary>
        [ConfigSecret(NotSet = NotSet.ThrowError, ErrorMessage ="You are missing the secret in the keyvault: DaluxFMApiKey.")]
        public string DaluxFMApiKey { get; set; }

        [ConfigSetting(NotSet = NotSet.ShowLogWarning, ErrorMessage = "In the appSetting 'DaluxFMDataToFetch', there are not written any data to fetch, so no data will be loadet.")]
        public string DaluxFMDataToFetch { get; set; }
    }
}
