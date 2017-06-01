using System.Collections.Generic;

namespace XML.classes.db.parametrs
{
    class ParametrsModel : Database
    {
        public static IEnumerable<ParametrsTable> Query(string q, object[] args = null)
        {
            try
            {
                if (args == null)
                    return con.Query<ParametrsTable>(q);

                return con.Query<ParametrsTable>(q, args);
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<ParametrsTable> GetAllByCategoryTitle(string title)
        {
            return Query("SELECT * FROM " + typeof(ParametrsTable).Name
                + " WHERE CategoryTitle = ?", new object[] { title });
        }

        public static IEnumerable<ParametrsTable> GetOneByCategoryTitle(string title)
        {
            return Query("SELECT * FROM " + typeof(ParametrsTable).Name
                + " WHERE CategoryTitle = ? LIMIT 1", new object[] { title });
        }

        public static int GetCount()
        {
            return con.Table<ParametrsTable>().Count();
        }
    }
}
