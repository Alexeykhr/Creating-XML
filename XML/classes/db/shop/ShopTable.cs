using SQLite;

namespace XML.classes.db.shop
{
    class ShopTable
    {
        [PrimaryKey, NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Company { get; set; }

        [NotNull]
        public string Url { get; set; }
    }
}
