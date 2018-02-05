using Creating_XML.src.db.tables;
using System.Collections.Generic;
using System.ComponentModel;

namespace Creating_XML.src.db.models
{
    class OfferModel : Model, IModel<OfferTable>
    {
        public IEnumerable<OfferTable> List(
            string search = "", int limit = 10, int offset = 0, string orderBy = "name",
            ListSortDirection sortDirection = ListSortDirection.Ascending
        ) {
            string query = "SELECT * FROM " + typeof(OfferTable).Name;
            object find = search;

            if (!string.IsNullOrWhiteSpace(search))
            {
                query += " WHERE ";

                if (!int.TryParse(search.ToString(), out int art))
                {
                    query += "Article = ?";
                    find = art;
                }
                else
                    query += "Name = ?";
            }

            query += " ORDER BY ? ? LIMIT ? OFFSET ?";

            // TODO: JOIN
            
            return Query<OfferTable>(query, new object[] {
                find,
                orderBy,
                sortDirection == ListSortDirection.Ascending ? "ASC" : "DESC",
                limit,
                offset
            });
        }

        public long Add(OfferTable obj)
        {
            return Insert(obj);
        }

        public long Upd(OfferTable obj)
        {
            return Update(obj);
        }

        public long Del(long id)
        {
            return Delete<OfferTable>(id);
        }
    }
}
