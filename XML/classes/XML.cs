using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using XML.classes.db;
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
                    new XAttribute("rate", Methods.ReplaceComma(item.Rate.ToString()))
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
                var elem = new XElement("category",
                    new XAttribute("id", item.CategoryId),
                    item.Title
                );

                if (item.ParCategoryId > 0)
                    elem.Add(new XAttribute("parentId", item.ParCategoryId));

                categories.Add(elem);
            }

            shop.Add(categories);
        }

        public static void AddOffers(XElement shop)
        {
            XElement offers = new XElement("offers");
            var items = OfferModel.GetAll();
            
            foreach (var item in items)
            {
                int categoryId = CategoryModel.GetOne(item.CategoryTitle).First().CategoryId;

                XElement offer = new XElement("offer",
                    new XAttribute("available", item.IsAviable),
                    new XAttribute("id", item.OfferId),
                    new XElement("name", item.Name),
                    new XElement("description", item.Description),
                    new XElement("url", item.URL),
                    new XElement("price", item.Price),
                    new XElement("vendor", item.Vendor),
                    new XElement("currencyId", item.CurrencyId),
                    new XElement("categoryId", categoryId));

                // Pictures
                string[] pictures = item.PicturesURL.Split('\n');
                foreach (string picture in pictures)
                {
                    offer.Add(new XElement("picture", picture.Trim()));
                }

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

        // |
        // | \/\/\/\/\/
        // | \/IMPORT\/
        // | \/\/\/\/\/
        // |

        public static bool ImportXML(string uri, bool isOverWrite)
        {
            XDocument doc;

            try
            {
                doc = XDocument.Load(uri);
            }
            catch { return false; }
            
            if (doc.Root.Element("shop") == null)
                return false;

            XElement shop = doc.Root.Element("shop");
            bool isNewShop = ShopModel.Get().Count() < 1;

            foreach (XElement el in shop.Elements())
            {
                switch (el.Name.ToString())
                {
                    case "name":
                        LoadShop(el.Value, null, null, isOverWrite, isNewShop);
                        break;
                    case "company":
                        LoadShop(null, el.Value, null, isOverWrite, isNewShop);
                        break;
                    case "url":
                        LoadShop(null, null, el.Value, isOverWrite, isNewShop);
                        break;
                    case "currencies":
                        LoadCurrencies(el, isOverWrite);
                        break;
                    case "categories":
                        LoadCategories(el, isOverWrite);
                        break;
                    case "offers":
                        LoadOffers(el, isOverWrite);
                        break;
                }
            }

            return true;
        }

        private static void LoadShop(string name, string company, string url, bool isOverWrite, bool isNewShop)
        {
            //ShopTable shop;
            //var shopDB = ShopModel.Get();
            //bool isNew = shopDB.Count() < 1;

            //if (isNew)
            //    shop = new ShopTable { Id = 1 };
            //else if (isOverWrite || isNewShop)
            //    shop = shopDB.First();
            //else
            //    return;

            //if (name != null)
            //    shop.Name = name.Trim();
            //else if (company != null)
            //    shop.Company = company.Trim();
            //else if (url != null)
            //    shop.Url = url.Trim();

            //if (isNew)
            //    Database.Insert(shop);
            //else
            //    Database.Update(shop);
        }

        private static void LoadCurrencies(XElement currencies, bool isOverWrite)
        {
            foreach (XElement currency in currencies.Elements())
            {
                if (currency.Attribute("id") == null || currency.Attribute("rate") == null)
                    continue;

                string currencyId = currency.Attribute("id").Value.Trim();
                string rate = Methods.ReplaceComma(currency.Attribute("rate").Value.Trim());

                if (string.IsNullOrWhiteSpace(currencyId) || string.IsNullOrWhiteSpace(rate))
                    continue;

                var model = CurrencyModel.GetOneByCurrencyId(currencyId);
                bool isExists = model.Count() > 0;

                var table = new CurrencyTable
                {
                    CurrencyId = currencyId,
                    Rate = rate
                };

                if (isExists && isOverWrite)
                {
                    //table.Id = model.First().Id;
                    Database.Update(table);
                }
                else if (!isExists)
                    Database.Insert(table);
            }
        }

        private static void LoadCategories(XElement categories, bool isOverWrite)
        {
            foreach (XElement category in categories.Elements())
            {
                if (category.Attribute("id") == null)
                    continue;

                bool isInt = int.TryParse(category.Attribute("id").Value.Trim(), out int categoryId);
                string title = category.Value.Trim();

                if (!isInt || categoryId < 1 || string.IsNullOrWhiteSpace(title))
                    continue;

                var model = CategoryModel.GetOne(title);
                bool isExists = model.Count() > 0;

                var table = new CategoryTable
                {
                    CategoryId = categoryId,
                    Title = title
                };

                // ParentId
                if (category.Attribute("parentId") != null)
                {
                    bool isCorrectParentId = int.TryParse(category.Attribute("parentId").Value.Trim(), out int parCategoryId);

                    if (isCorrectParentId)
                        table.ParCategoryId = parCategoryId;
                }
                // End

                if (isExists && isOverWrite)
                {
                    //table.Id = model.First().Id;
                    Database.Update(table);
                }
                else if (!isExists)
                    Database.Insert(table);
            }
        }

        private static void LoadOffers(XElement offers, bool isOverWrite)
        {
            foreach (XElement offer in offers.Elements())
            {
                // Check important data => name, price, currencyId, categoryId
                if (offer.Attribute("id") == null || offer.Element("name") == null ||
                    offer.Element("price") == null || offer.Element("currencyId") == null ||
                    offer.Element("categoryId") == null)
                    continue;

                bool isCorrectOfferId = int.TryParse(offer.Attribute("id").Value.Trim(), out int offerId);
                bool isDouble = Double.TryParse(Methods.ReplaceDot(offer.Element("price").Value).Trim(), out double price);
                string name = offer.Element("name").Value.Trim();
                bool isInt = int.TryParse(offer.Element("categoryId").Value.Trim(), out int categoryId);
                string currencyId = offer.Element("currencyId").Value.Trim();

                if (string.IsNullOrWhiteSpace(name) || !isDouble || price < 0 || offerId < 1 ||
                    !isInt || categoryId < 1 || string.IsNullOrWhiteSpace(currencyId))
                    continue;
                // End

                // Check categoryId and currencyId in tables.
                var categoryModel = CategoryModel.GetOne(categoryId);

                if (categoryModel.Count() < 1)
                    continue;

                if (CurrencyModel.GetOneByCurrencyId(currencyId).Count() < 1)
                    continue;
                // End

                var model = OfferModel.GetOneByName(name);
                bool isExists = model.Count() > 0;

                var table = new OfferTable
                {
                    OfferId = offerId,
                    Name = name,
                    Price = price,
                    CurrencyId = currencyId,
                    CategoryTitle = categoryModel.First().Title,
                    IsAviable = true
                };

                // Add More data
                if (offer.Element("url") != null)
                    table.URL = offer.Element("url").Value.Trim();

                // -- Picture
                string pictures = string.Empty;
                foreach (var picture in offer.Elements("picture"))
                {
                    pictures += picture.Value.Trim() + Environment.NewLine;
                }
                if (!string.IsNullOrWhiteSpace(pictures))
                    table.PicturesURL = pictures.Trim();

                // -- Parametrs
                string parametrs = string.Empty;
                foreach (var parametr in offer.Elements("param"))
                {
                    if (parametr.Attribute("name") == null || string.IsNullOrWhiteSpace(parametr.Value))
                        continue;

                    string key = parametr.Attribute("name").Value.Trim();
                    string value = parametr.Value.Trim();

                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    parametrs += key + "|" + value + "|";
                }
                if (!string.IsNullOrWhiteSpace(parametrs))
                    table.Params = parametrs.Substring(0, parametrs.Length - 1);

                if (offer.Element("description") != null)
                    table.Description = offer.Element("description").Value.Trim();

                if (offer.Attribute("available") != null)
                    table.IsAviable = offer.Attribute("available").Value.Equals("true");

                if (offer.Element("vendor") != null)
                    table.Vendor = offer.Element("vendor").Value.Trim();
                // End

                // Insert / update in DB
                if (isExists && isOverWrite)
                {
                    table.OfferId = model.First().OfferId;
                    Database.Update(table);
                }
                else if (!isExists)
                {
                    //table.OfferId = OfferModel.GetCount() + 1;
                    //table.OfferId = int.Parse(offer.Attribute("id").Value.Trim());
                    Database.Insert(table);
                }
            }
        }

        // |
        // | \/\/\/\/\/
        // | \/REPAIR\/
        // | \/\/\/\/\/
        // |

        public static void Repair(string uri)
        {
            string str = string.Empty;

            using (StreamReader reader = File.OpenText(uri))
            {
                str = reader.ReadToEnd();
            }

            str = str.Replace("&", "and");

            using (StreamWriter file = new StreamWriter(uri))
            {
                file.Write(str);
            }
        }
    }
}
