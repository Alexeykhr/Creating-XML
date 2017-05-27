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

        public static IEnumerable<CurrencyTable> GetOneByCurrencyId(object[] args)
        {
            return Query("SELECT * FROM " + typeof(CurrencyTable).Name
                + " WHERE CurrencyId = ? LIMIT 1", args);
        }

        public static int GetCount()
        {
            return con.Table<CurrencyTable>().Count();
        }
    }
}
