using SQLite;
using Creating_XML.src.db.tables;

namespace Creating_XML.src.db
{
    class Database
    {
        private static SQLiteAsyncConnection conn;

        /// <summary>
        /// Set connection to DB.
        /// </summary>
        /// <param name="file"></param>
        public static void Connection(string file)
        {
            try
            {
                conn = new SQLiteAsyncConnection(file);
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Ошибка подключения к БД");
            }
        }

        /// <summary>
        /// Migration tables to File.
        /// </summary>
        public static void Migration()
        {
            conn.CreateTableAsync<CategoryParametersTable>();
            conn.CreateTableAsync<CategoryTable>();
            conn.CreateTableAsync<CurrencyTable>();
            conn.CreateTableAsync<OfferImageTable>();
            conn.CreateTableAsync<OfferParametersTable>();
            conn.CreateTableAsync<OfferTable>();
            conn.CreateTableAsync<VendorTable>();
        }
    }
}
