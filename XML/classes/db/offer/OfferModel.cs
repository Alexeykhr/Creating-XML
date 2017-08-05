using System.Collections.Generic;

namespace XML.classes.db.offer
{
    class OfferModel : Database
    {
        public IEnumerable<OfferTable> Query(string q, object[] args = null)
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

        public IEnumerable<OfferTable> GetAll()
        {
            return Query("SELECT * FROM " + typeof(OfferTable).Name);
        }

        public IEnumerable<OfferTable> GetOneByOfferId(int offerId)
        {
            return Query("SELECT * FROM " + typeof(OfferTable).Name
                + " WHERE OfferId = ? LIMIT 1", new object[] { offerId });
        }

        public IEnumerable<OfferTable> GetOneByName(string name)
        {
            return Query("SELECT * FROM " + typeof(OfferTable).Name
                + " WHERE Name = ? LIMIT 1", new object[] { name });
        }

        public IEnumerable<OfferTable> GetOneByCategoryTitle(string CategoryTitle)
        {
            return Query("SELECT * FROM " + typeof(OfferTable).Name
                + " WHERE CategoryTitle = ? LIMIT 1", new object[] { CategoryTitle });
        }

        public IEnumerable<OfferTable> GetOneByCurrencyId(string CurrencyId)
        {
            return Query("SELECT * FROM " + typeof(OfferTable).Name
                + " WHERE CurrencyId = ? LIMIT 1", new object[] { CurrencyId });
        }

        public int GetCount()
        {
            try
            {
                return con.Table<OfferTable>().Count();
            }
            catch { return 0; }
        }
    }
}
