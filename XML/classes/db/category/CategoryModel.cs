using System.Collections.Generic;

namespace XML.classes.db.category
{
    class CategoryModel : Database
    {
        public static IEnumerable<CategoryTable> Query(string q, object[] args = null)
        {
            try
            {
                if (args == null)
                    return con.Query<CategoryTable>(q);

                return con.Query<CategoryTable>(q, args);
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<CategoryTable> GetAll()
        {
            return Query("SELECT * FROM " + typeof(CategoryTable).Name);
        }

        public static IEnumerable<CategoryTable> GetOne(int categoryId)
        {
            return Query("SELECT * FROM " + typeof(CategoryTable).Name
                + " WHERE CategoryId = ? LIMIT 1", new object[] { categoryId });
        }

        public static IEnumerable<CategoryTable> GetOne(string title)
        {
            return Query("SELECT * FROM " + typeof(CategoryTable).Name
                + " WHERE Title = ? LIMIT 1", new object[] { title});
        }

        public static int GetCount()
        {
            return con.Table<CategoryTable>().Count();
        }
    }
}
