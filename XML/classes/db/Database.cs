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

        protected SQLiteConnection con;

        public Database()
        {
            try
            {
                Directory.CreateDirectory(DIR + "\\saves");
                con = new SQLiteConnection(FILE_URI);
            }
            catch { }
        }

        public void CreateTable<T>()
        {
            try
            {
                con.CreateTable<T>();
            }
            catch { }
        }

        public int Insert(object ob)
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

        public int Update(object ob)
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

        public int DeleteObject<T>(object id)
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

        public void NewProject()
        {
            CreateTable<OfferTable>();
            CreateTable<ShopTable>();
            CreateTable<CategoryTable>();
            CreateTable<CurrencyTable>();
            CreateTable<ParametrsTable>();
        }
    }
}
