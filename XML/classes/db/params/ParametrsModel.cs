using System.Collections.Generic;

namespace XML.classes.db.parametrs
{
    class ParametrsModel : Database
    {
        public IEnumerable<ParametrsTable> Query(string q, object[] args = null)
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

        public IEnumerable<ParametrsTable> GetAllByCategoryTitle(string title)
        {
            return Query("SELECT * FROM " + typeof(ParametrsTable).Name
                + " WHERE CategoryTitle = ?", new object[] { title });
        }

        public IEnumerable<ParametrsTable> GetOneByCategoryTitle(string title)
        {
            return Query("SELECT * FROM " + typeof(ParametrsTable).Name
                + " WHERE CategoryTitle = ? LIMIT 1", new object[] { title });
        }

        public int GetCount()
        {
            return con.Table<ParametrsTable>().Count();
        }
    }
}
