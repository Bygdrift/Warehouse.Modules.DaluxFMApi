using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Module.Services.Models.Helpers
{
    public class Root<T> where T : class
    {
        public Root(IEnumerable<JToken> itemsRaw, int totalItems, string bookMark, string nextBookMark)
        {
            ItemsRaw = itemsRaw;
            TotalItems = totalItems;
            Bookmark = int.Parse(bookMark);
            NextBookmark = int.TryParse(nextBookMark, out int res) ? res : null;
        }

        public Root(IEnumerable<JToken> itemsRaw, int totalItems, int bookmark, int? nextBookmark)
        {
            ItemsRaw = itemsRaw;
            TotalItems = totalItems;
            Bookmark = bookmark;
            NextBookmark = nextBookmark;
        }

        public IEnumerable<JToken> ItemsRaw { get; set; }

        public int TotalItems { get; set; }

        public int Bookmark { get; set; }

        public int? NextBookmark { get; set; }

        private List<T> _items;

        public List<T> Items
        {
            get { return _items ??= (from JToken result in ItemsRaw select result.ToObject<T>()).ToList(); }
            set { _items = value; }
        }
    }
}
