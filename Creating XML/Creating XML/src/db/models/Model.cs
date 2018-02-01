using SQLite;
using System.Collections.Generic;

namespace Creating_XML.src.db.models
{
    abstract class Model : Database
    {
        /// <summary>
        /// Connect and execute the request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(string query, object[] args = null) where T : new()
        {
            using (con = new SQLiteConnection(Project.GetCurrentFileDB()))
            {
                return con.Query<T>(query, args);
            }
        }

        /// <summary>
        /// Connect and add a record.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long Insert(object obj)
        {
            using (con = new SQLiteConnection(Project.GetCurrentFileDB()))
            {
                return con.Insert(obj);
            }
        }

        /// <summary>
        /// Connect and update a record.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long Update(object obj)
        {
            using (con = new SQLiteConnection(Project.GetCurrentFileDB()))
            {
                return con.Update(obj);
            }
        }

        /// <summary>
        /// Connect and delete a record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static long Delete<T>(long id)
        {
            using (con = new SQLiteConnection(Project.GetCurrentFileDB()))
            {
                return con.Delete<T>(id);
            }
        }
    }
}
