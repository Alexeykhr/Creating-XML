using SQLite;
using System.Collections.Generic;

namespace XML.classes.db.offer
{
    class OfferTable
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        // Table Currency
        public int CurrencyId { get; set; }

        // Table Category
        public int CategoryId { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public string URL { get; set; }

        public string PictureURL { get; set; }

        public bool IsAviable { get; set; }

        public Dictionary<string, string> Params { get; set; }
    }
}
