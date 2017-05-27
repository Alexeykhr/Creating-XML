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
    }
}
