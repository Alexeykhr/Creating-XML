using System.Collections.Generic;

namespace XML.classes.db.currency
{
    class CurrencyModel : Database
    {
        public static IEnumerable<CurrencyTable> Query(string q, object[] args = null)
        {
            try
            {
                if (args == null)
                    return con.Query<CurrencyTable>(q);

                return con.Query<CurrencyTable>(q, args);
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<CurrencyTable> GetAll()
        {
            return Query("SELECT * FROM " + typeof(CurrencyTable).Name);
        }

        public static IEnumerable<CurrencyTable> GetOneByCurrencyId(string currencyId)
        {
            return Query("SELECT * FROM " + typeof(CurrencyTable).Name
                + " WHERE CurrencyId = ? LIMIT 1", new object[] { currencyId });
        }

        public static IEnumerable<CurrencyTable> GetOneByRate(string rate)
        {
            return Query("SELECT * FROM " + typeof(CurrencyTable).Name
                + " WHERE Rate = ? LIMIT 1", new object[] { rate });
        }

        public static int GetCount()
        {
            return con.Table<CurrencyTable>().Count();
        }
    }
}
