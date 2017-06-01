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

        public static int GetCount()
        {
            return con.Table<ConfigTable>().Count();
        }
    }
}
