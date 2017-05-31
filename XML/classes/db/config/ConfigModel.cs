using System.Collections.Generic;

namespace XML.classes.db.config
{
    class ConfigModel : Database
    {
        public static IEnumerable<ConfigTable> Query(string q, object[] args = null)
        {
            try
            {
                if (args == null)
                    return con.Query<ConfigTable>(q);

                return con.Query<ConfigTable>(q, args);
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<ConfigTable> GetAll()
        {
            return Query("SELECT * FROM " + typeof(ConfigTable).Name);
        }

        public static IEnumerable<ConfigTable> GetOne(string currencyId)
        {
            return Query("SELECT * FROM " + typeof(ConfigTable).Name
                + " WHERE CurrencyId = ? LIMIT 1", new object[] { currencyId });
        }

        public static IEnumerable<ConfigTable> GetOne(double rate)
        {
            return Query("SELECT * FROM " + typeof(ConfigTable).Name
                + " WHERE Rate = ? LIMIT 1", new object[] { rate });
        }

        public static int GetCount()
        {
            return con.Table<ConfigTable>().Count();
        }
    }
}
