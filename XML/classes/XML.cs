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
                XElement currency = new XElement("currency");
                currency.SetAttributeValue("id", item.CurrencyId);
                currency.SetAttributeValue("rate", item.Rate);

                currencies.Add(currency);
            }

            shop.Add(currencies);
        }

        public static void AddCategories(XElement shop)
        {
            XElement categories = new XElement("categories");
            var items = CategoryModel.GetAll();

            foreach (var item in items)
            {
                XElement category = new XElement("category");
                category.Value = item.Title;
                category.SetAttributeValue("id", item.CategoryId);

                categories.Add(category);
            }

            shop.Add(categories);
        }

        public static void AddOffers(XElement shop)
        {
            XElement offers = new XElement("offers");
            var items = OfferModel.GetAll();
        }
    }
}
