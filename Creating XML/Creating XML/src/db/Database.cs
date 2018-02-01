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
            using (con = new SQLiteConnection(Project.GetCurrentFileDB()))
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
    }
}
