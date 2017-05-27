using System;
using System.Linq;
using System.Xml.Linq;

using XML.classes.db.shop;
using XML.classes.db.offer;
using XML.classes.db.category;
using XML.classes.db.currency;

namespace XML.classes
{
    class XMLHelpler
    {
        public static XDocument CreateDoc()
        {
            return new XDocument(
                new XElement("yml_catalog",
                    new XAttribute("date", DateTime.Now.ToString("yyyy-MM-dd H:mm"))
                )
            );
        }

        public static XElement AddShop(XDocument root)
        {
            XElement shop = new XElement("shop");
            var items = ShopModel.Get();

            if (items.Count() < 1)
                return null;

            shop.Add(
                new XElement("name", items.First().Name),
                new XElement("company", items.First().Company),
                new XElement("url", items.First().Url)
            );

            root.Root.Add(shop);

            return shop;
        }

        public static void AddCurrencies(XElement shop)
        {
            XElement currencies = new XElement("currencies");
            var items = CurrencyModel.GetAll();

            foreach (var item in items)
            {
                currencies.Add(new XElement("currency",
                    new XAttribute("id", item.CurrencyId),
                    new XAttribute("rate", item.Rate)
                ));
            }

            shop.Add(currencies);
        }

        public static void AddCategories(XElement shop)
        {
            XElement categories = new XElement("categories");
            var items = CategoryModel.GetAll();

            foreach (var item in items)
            {
                categories.Add(new XElement("category",
                    new XAttribute("id", item.CategoryId),
                    item.Title
                ));
            }

            shop.Add(categories);
        }

        public static void AddOffers(XElement shop)
        {
            XElement offers = new XElement("offers");
            var items = OfferModel.GetAll();
            
            foreach (var item in items)
            {
                offers.Add(new XElement("offer",
                    new XAttribute("available", item.IsAviable),
                    new XAttribute("id", item.Id)
                ));
            }

            shop.Add(offers);
        }
    }
}
