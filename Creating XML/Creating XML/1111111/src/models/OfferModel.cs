using System.Collections.Generic;
using Creating_XML.src.db.tables;
using Creating_XML.src.objects;
using System.ComponentModel;
using System.Linq;
using System;

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
                + " FROM OfferTable OT"
                + " LEFT JOIN CategoryTable CatT ON OT.CategoryId = CatT.Id"
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

            query += " ORDER BY ? " + (sortDirection == ListSortDirection.Ascending ? "ASC" : "DESC") + " LIMIT ? OFFSET ?";

            // Get data from the server
            if (!string.IsNullOrWhiteSpace(search))
                list = Database.Query<OfferObject>(query, search, orderBy, take, (page - 1) * take).ToList();
            else
                list = Database.Query<OfferObject>(query, orderBy, take, (page - 1) * take).ToList();

            // Initiate List
            foreach (var item in list)
            {
                item.Images = new List<OfferImageTable>();
                item.Parameters = new List<OfferParameterTable>();
            }

            // Join 2 tables
            if (list.Count > 0)
            {
                list = AddParameter(list);
                list = AddImages(list);
            }

            return list;
        }

        /// <summary>
        /// Add all parameters to their collection.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<OfferObject> AddParameter(List<OfferObject> list)
        {
            string questionMarkCount = string.Concat(Enumerable.Repeat("?,", list.Count)).Remove(list.Count * 2 - 1);

            var listParameters = Database.Query<OfferParameterTable>(
                "SELECT * FROM OfferParameterTable WHERE OfferId IN (" + questionMarkCount + ")",
                Array.ConvertAll(list.ToArray(), i => i.Id.ToString())
            ).ToList();

            foreach (var parameter in listParameters)
            {
                foreach (var item in list)
                {
                    if (parameter.OfferId == item.Id)
                        item.Parameters.Add(parameter);
                }
            }

            return list;
        }

        /// <summary>
        /// Add all images to their collection.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<OfferObject> AddImages(List<OfferObject> list)
        {
            string questionMarkCount = string.Concat(Enumerable.Repeat("?,", list.Count)).Remove(list.Count * 2 - 1);

            var listImages = Database.Query<OfferImageTable>(
                "SELECT * FROM OfferImageTable WHERE OfferId IN (" + questionMarkCount + ")",
                Array.ConvertAll(list.ToArray(), i => i.Id.ToString())
            ).ToList();

            foreach (var image in listImages)
            {
                foreach (var item in list)
                {
                    if (image.OfferId == item.Id)
                        item.Images.Add(image);
                }
            }

            return list;
        }
    }
}
