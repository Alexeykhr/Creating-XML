using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

using XML.classes.db;
using XML.classes.db.shop;
using XML.classes.db.category;
using XML.classes.db.currency;

namespace XML.forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Text = "XML - Main";

            if ( ! Database.SetConnection() )
                Close();
        }

        private void CompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormShop f = new FormShop();
            f.Show(this);
        }

        private void CurrencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormList f = new FormList(false);
            f.Show(this);
        }

        private void CategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormList f = new FormList(true);
            f.Show(this);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ExportXML();
        }

        private void ExportXML()
        {
            File.Delete("base.xml");

            XDocument doc = CreateDocumentXML();
            XElement shop = XMLAddShop(doc);

            if (shop == null)
                return;
            
            XMLAddCategory(shop);
            XMLAddCurrencies(shop);

            doc.Save("base.xml");
        }

        private XDocument CreateDocumentXML()
        {
            return new XDocument(
                new XElement("yml_catalog",
                    new XAttribute("date", DateTime.Now.ToString("yyyy-MM-dd H:mm"))
                )
            );
        }

        private XElement XMLAddShop(XDocument root)
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

        private void XMLAddCategory(XElement shop)
        {
            var items = CategoryModel.GetAll();
           
            XElement categories = new XElement("categories");
            foreach (var item in items)
            {
                XElement category = new XElement("category");
                category.Value = item.Title;
                category.SetAttributeValue("id", item.CategoryId);

                categories.Add(category);
            }

            shop.Add(categories);
        }

        private void XMLAddCurrencies(XElement shop)
        {
            XElement currencies = new XElement("currencies");
            var itemsCurrencies = CurrencyModel.GetAll();

            foreach (var item in itemsCurrencies)
            {
                XElement currency = new XElement("currency");
                currency.SetAttributeValue("id", item.CurrencyId);
                currency.SetAttributeValue("rate", item.Rate);

                currencies.Add(currency);
            }

            shop.Add(currencies);
        }
    }
}
