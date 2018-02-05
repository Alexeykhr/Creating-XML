using SQLite;
using System.Collections.Generic;
using Creating_XML.src.db.tables;

namespace Creating_XML.src.db
{
    class Database
    {
        protected static SQLiteConnection con;

        public static void Migration()
        {
            using (con = CreateConnection())
            {
                con.CreateTable<CategoryParametersTable>();
                con.CreateTable<CategoryTable>();
                con.CreateTable<CurrencyTable>();
                con.CreateTable<OfferImageTable>();
                con.CreateTable<OfferParametersTable>();
                con.CreateTable<OfferTable>();
                con.CreateTable<VendorTable>();
            }
        }

        private static SQLiteConnection CreateConnection(string file = null)
        {
            if (file == null)
                file = Project.GetCurrentFileDB();

            return new SQLiteConnection(file);
        }
    }
}
