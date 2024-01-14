namespace Module.AppFunctions
{
    /// <summary>
    /// An exchange model between Orchestrator and ActivityTrigger.
    /// Designed to only keep few data, because Orchestrator saves it multiple places as json inside a datalake
    /// So data from webservice I have to store, is saved right place in DataLake.
    /// </summary>
    public class Call
    {
        public Call(string name) => Name = name;

        public string Name { get; }
        public bool IsLoaded { get; set; }
        public int? NextBookMark { get; set; }
    }
}
