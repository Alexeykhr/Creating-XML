using SQLite;
using System.Linq;
using System.Collections.Generic;
using Creating_XML.src.db.tables;

namespace Creating_XML.src.db
{
    class Database
    {
        private static SQLiteConnection conn;

        /// <summary>
        /// Set connection to DB.
        /// </summary>
        /// <param name="file"></param>
        public static void Connection(string file)
        {
            try
            {
                if (conn == null)
                    conn = new SQLiteConnection(file);
            }
            catch
            {
                throw new System.Exception("Ошибка подключения к БД");
            }
        }

        /// <summary>
        /// Close Connection to DB.
        /// </summary>
        public static void CloseConnection()
        {
            conn.Dispose();
            conn = null;
        }

        /// <summary>
        /// Connection to the database is established.
        /// </summary>
        /// <returns></returns>
        public static bool HasConnection()
        {
            return conn != null;
        }

        /// <summary>
        /// Get a list of data from the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> List<T>() where T : new()
        {
            try
            {
                return conn.Table<T>().ToList();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Find and return record if exists by Primary Key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pk">Primary Key</param>
        /// <returns></returns>
        public static T Find<T>(object pk) where T : new()
        {
            return conn.Find<T>(pk);
        }

        /// <summary>
        /// Return table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TableQuery<T> Table<T>() where T : new()
        {
            return conn.Table<T>();
        }

        /// <summary>
        /// Create Query to the DB.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(string query, object args = null) where T : new()
        {
            try
            {
                return conn.Query<T>(query, args);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Add a new record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns>1 | 0</returns>
        public static int Insert<T>(T table)
        {
            try
            {
                return conn.Insert(table);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Add or replace a record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static int InsertOrReplace<T>(T table)
        {
            try
            {
                return conn.InsertOrReplace(table);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Update row. Required to have a primary key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static int Update<T>(T table)
        {
            try
            {
                return conn.Update(table);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Delete row. Required to have a primary key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static int Delete<T>(T table)
        {
            try
            {
                return conn.Delete(table);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Migration tables to File.
        /// </summary>
        public static void Migration()
        {
            conn.CreateTable<CategoryParameterTable>();
            conn.CreateTable<CategoryTable>();
            conn.CreateTable<CurrencyTable>();
            conn.CreateTable<OfferImageTable>();
            conn.CreateTable<OfferParameterTable>();
            conn.CreateTable<OfferTable>();
            conn.CreateTable<VendorTable>();
        }
    }
}
