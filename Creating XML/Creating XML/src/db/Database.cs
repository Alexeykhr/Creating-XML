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
                conn = new SQLiteConnection(file);
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Ошибка подключения к БД");
            }
        }

        public static void CloseConnection()
        {
            conn.Dispose();
        }

        public static List<T> List<T>() where T : new()
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

        public static int Insert<T>(T table)
        {
            return conn.Insert(table);
        }

        /// <summary>
        /// Migration tables to File.
        /// </summary>
        public static void Migration()
        {
            conn.CreateTable<CategoryParametersTable>();
            conn.CreateTable<CategoryTable>();
            conn.CreateTable<CurrencyTable>();
            conn.CreateTable<OfferImageTable>();
            conn.CreateTable<OfferParametersTable>();
            conn.CreateTable<OfferTable>();
            conn.CreateTable<VendorTable>();
        }
    }
}
