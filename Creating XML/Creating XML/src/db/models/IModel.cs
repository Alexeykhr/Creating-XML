using System.Collections.Generic;
using System.ComponentModel;

namespace Creating_XML.src.db.models
{
    public interface IModel<T>
    {
        IEnumerable<T> List(string search = "", int limit = 10, int offset = 0, string orderBy = "name",
            ListSortDirection sortDirection = ListSortDirection.Ascending);
        
        long Add(T obj);

        long Upd(T obj);

        long Del(long id);
    }
}
