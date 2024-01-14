using System.Collections.Generic;

namespace Module.Services.Models.Helpers
{
    public class MetaDataExtend<T> where T : class
    {
        public MetaDataExtend(List<T> items, int totalItems, string bookMark, string nextBookMark)
        {
            Items = items;
            TotalItems = totalItems;
            BookMark = int.Parse(bookMark);
            NextBookMark = int.TryParse(nextBookMark, out int res) ? res : null;
        }

        public List<T> Items { get; }
        public int TotalItems { get; }
        public int BookMark { get; }
        public int? NextBookMark { get; }
    }
}
