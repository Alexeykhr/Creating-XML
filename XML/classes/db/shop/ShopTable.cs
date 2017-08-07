using SQLite;

namespace XML.classes.db.shop
{
    class ShopTable
    {
        [PrimaryKey, AutoIncrement]
        public byte Id { get; set; }
        
        public string Name { get; set; }
        
        public string Company { get; set; }
        
        public string Url { get; set; }
    }
}
