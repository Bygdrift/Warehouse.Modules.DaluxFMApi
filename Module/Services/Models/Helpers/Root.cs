using System;

namespace Module.Services.Models.Helpers
{
    public class Root<T> where T : class
    {

        public Metadata metadata { get; set; }
        public Item<T>[] items { get; set; }
        public Link1[] links { get; set; }
        public int responseCode { get; set; }
        public string responseMessage { get; set; }
        public DateTime date { get; set; }
    }

    public class Metadata
    {
        public int totalItems { get; set; }
        public object limit { get; set; }
        public string bookmark { get; set; }
        public string nextBookmark { get; set; }
    }

    public class Item<T> where T : class
    {
        public T data { get; set; }
        public Link[] links { get; set; }
    }

    public class Userdefinedfield
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
        public string method { get; set; }
    }

    public class Link1
    {
        public string rel { get; set; }
        public string href { get; set; }
        public string method { get; set; }
    }
}
