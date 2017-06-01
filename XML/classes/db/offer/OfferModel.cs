using System.Collections.Generic;

namespace XML.classes.db.offer
{
    class OfferModel : Database
    {
        public static IEnumerable<OfferTable> Query(string q, object[] args = null)
        {
            try
            {
                if (args == null)
                    return con.Query<OfferTable>(q);

                return con.Query<OfferTable>(q, args);
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<OfferTable> GetAll()
        {
            return Query("SELECT * FROM " + typeof(OfferTable).Name);
        }

        public static IEnumerable<OfferTable> GetOneByOfferId(int offerId)
        {
            return Query("SELECT * FROM " + typeof(OfferTable).Name
                + " WHERE OfferId = ? LIMIT 1", new object[] { offerId });
        }

        public static IEnumerable<OfferTable> GetOneByCategoryTitle(string CategoryTitle)
        {
            return Query("SELECT * FROM " + typeof(OfferTable).Name
                + " WHERE CategoryTitle = ? LIMIT 1", new object[] { CategoryTitle });
        }

        public static IEnumerable<OfferTable> GetOneByCurrencyId(string CurrencyId)
        {
            return Query("SELECT * FROM " + typeof(OfferTable).Name
                + " WHERE CurrencyId = ? LIMIT 1", new object[] { CurrencyId });
        }

        public static int GetCount()
        {
            return con.Table<OfferTable>().Count();
        }
    }
}
