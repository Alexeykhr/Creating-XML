using System;
using System.Linq;
using System.Xml.Linq;

using XML.classes.db.shop;
using XML.classes.db.offer;
using XML.classes.db.category;
using XML.classes.db.currency;
using System.Windows.Forms;

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
            string vendor = ShopModel.Get().First().Name;
            
            foreach (var item in items)
            {
                int categoryId = CategoryModel.GetOne(item.CategoryTitle).First().CategoryId;

                XElement offer = new XElement("offer",
                    new XAttribute("available", item.IsAviable),
                    new XAttribute("id", item.Id),
                    new XElement("name", item.Name),
                    new XElement("description", item.Description),
                    new XElement("url", item.URL),
                    new XElement("picture", item.PictureURL),
                    new XElement("price", item.Price),
                    new XElement("vendor", vendor),
                    new XElement("currencyId", item.CurrencyId),
                    new XElement("categoryId", categoryId));

                AddParams(offer, item.Params);
                offers.Add(offer);
            }

            shop.Add(offers);
        }

        private static void AddParams(XElement offer, string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            string[] arr = value.Split('|');
            int len = arr.Length / 2;
            
            for (int i = 0, j = 0; i < len; i++, j += 2)
            {
                offer.Add(new XElement("param",
                    new XAttribute("name", arr[j]),
                    arr[j + 1]));
            }
        }

        public static bool ImportXML(string uri, bool isOverWrite)
        {
            XDocument doc = XDocument.Load(uri);
            
            if (doc.Root.Element("shop") == null)
                return false;

            XElement shop = doc.Root.Element("shop");

            foreach (XElement el in shop.Elements())
            {
                switch (el.Name.ToString())
                {
                    case "name":
                        MessageBox.Show("test");
                        break;
                    case "company":
                        break;
                    case "url":
                        break;
                    case "currencies":
                        break;
                    case "categories":
                        break;
                    case "offers":
                        break;
                }
            }

            return true;
        }
    }
}
