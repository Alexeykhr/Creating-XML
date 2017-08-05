using System.Collections.Generic;

namespace XML.classes.db.shop
{
    class ShopModel : Database
    {
        public IEnumerable<ShopTable> Get()
        {
            try
            {
                return con.Query<ShopTable>("SELECT * FROM " + typeof(ShopTable).Name);
            }
            catch
            {
                return null;
            }
        }
    }
}
