using System;
using SQLite;
using System.IO;

using XML.classes.db.shop;
using XML.classes.db.offer;
using XML.classes.db.currency;
using XML.classes.db.category;
using XML.classes.db.parametrs;

namespace XML.classes.db
{
    public class Database
    {
        public static string FILE_NAME = "data.sqlite";
        public static string DIR = AppDomain.CurrentDomain.BaseDirectory;
        public static string FILE_URI = DIR + "\\saves\\" + FILE_NAME;

        protected static SQLiteConnection con;

        public static bool Connection()
        {
            try
            {
                Directory.CreateDirectory(DIR + "\\saves");
                con = new SQLiteConnection(FILE_URI);

                NewProject();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void CreateTable<T>()
        {
            try
            {
                con.CreateTable<T>();
            }
            catch { }
        }

        public static int Insert(object ob)
        {
            try
            {
                return con.Insert(ob);
            }
            catch
            {
                return 0;
            }
        }

        public static int Update(object ob)
        {
            try
            {
                return con.Update(ob);
            }
            catch
            {
                return 0;
            }
        }

        public static int DeleteObject<T>(object id)
        {
            try
            {
                return con.Delete<T>(id);
            }
            catch
            {
                return 0;
            }
        }

        public static void NewProject()
        {
            CreateTable<OfferTable>();
            CreateTable<ShopTable>();
            CreateTable<CategoryTable>();
            CreateTable<CurrencyTable>();
            CreateTable<ParametrsTable>();
        }

        public static void CloseConnection()
        {
            if (con != null)
                con.Close();
        }
    }
}
