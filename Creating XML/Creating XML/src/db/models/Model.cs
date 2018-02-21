using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Creating_XML.src.db.models
{
    abstract class Model : Database
    {
        ///// <summary>
        ///// Connect and execute the request.
        ///// </summary>
        ///// <see cref="SQLiteConnection.Query(object)"/>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="query"></param>
        ///// <param name="args"></param>
        ///// <returns></returns>
        //protected static async Task<List<T>> Query<T>(string query, object[] args = null) where T : new()
        //{
        //    return await conn.QueryAsync<T>(query, args);
        //}

        ///// <summary>
        ///// Connect and add a record.
        ///// </summary>
        ///// <see cref="SQLiteConnection.Insert(object)"/>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //protected static long Insert(object obj)
        //{
        //    using (con = new SQLiteConnection(Project.GetCurrentFileDB()))
        //    {
        //        return con.Insert(obj);
        //    }
        //}

        ///// <summary>
        ///// Connect and update a record.
        ///// </summary>
        ///// <see cref="SQLiteConnection.Update(object)"/>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //protected static long Update(object obj)
        //{
        //    using (con = new SQLiteConnection(Project.GetCurrentFileDB()))
        //    {
        //        return con.Update(obj);
        //    }
        //}

        ///// <summary>
        ///// Connect and delete a record.
        ///// </summary>
        ///// <see cref="SQLiteConnection.Delete(object)"/>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //protected static long Delete<T>(long id)
        //{
        //    using (con = new SQLiteConnection(Project.GetCurrentFileDB()))
        //    {
        //        return con.Delete<T>(id);
        //    }
        //}
    }
}
