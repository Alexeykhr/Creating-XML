using System.Collections.Generic;
using Creating_XML.src.objects;
using System.ComponentModel;
using System.Linq;

namespace Creating_XML.src.db.models
{
    class OfferModel
    {
        /// <summary>
        /// Get List of Offers (paginate).
        /// </summary>
        /// <param name="search"></param>
        /// <param name="take"></param>
        /// <param name="page"></param>
        /// <param name="orderBy"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public static List<OfferObject> List(
            string search = "", int take = 10, int page = 1, string orderBy = "Name",
            ListSortDirection sortDirection = ListSortDirection.Ascending
        )
        {
            List<OfferObject> list = null;
            string query =
                "SELECT OT.*, CatT.Name as CategoryName, CurT.Name as CurrencyName,"
                    + " CurT.Rate as CurrencyRate, VenT.Name as VendorName"
                + " FROM OfferTable OT";

            // Joins
            query += " LEFT JOIN CategoryTable CatT ON OT.CategoryId = CatT.Id"
                + " LEFT JOIN CurrencyTable CurT ON OT.CurrencyId = CurT.Id"
                + " LEFT JOIN VendorTable VenT ON OT.VendorId = VenT.Id";

            // Search Article or Name
            if (!string.IsNullOrWhiteSpace(search))
            {
                query += " WHERE";

                if (int.TryParse(search.ToString(), out int art))
                    query += " OT.Article = ?";
                else
                {
                    query += " OT.Name LIKE ?";
                    search = "%" + search + "%";
                }
            }

            query += " ORDER BY ? " + (sortDirection == ListSortDirection.Ascending ? "ASC" : "DESC")
                + " LIMIT ?"
                + " OFFSET ?";

            if (!string.IsNullOrWhiteSpace(search))
            {
                list = Database.Query<OfferObject>(query,
                        search,
                        orderBy,
                        take,
                        (page - 1) * take
                    ).ToList();
            }
            else
            {
                list = Database.Query<OfferObject>(query,
                        orderBy,
                        take,
                        (page - 1) * take
                    ).ToList();
            }

            // TODO Join 2 tables

            return list;
        }
    }
}
