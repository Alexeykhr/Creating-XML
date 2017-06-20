using SQLite;

namespace XML.classes.db.offer
{
    class OfferTable
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        [Unique]
        public int OfferId { get; set; }

        [Unique]
        public string Name { get; set; }
        
        public string Description { get; set; }

        public string URL { get; set; }

        public string PictureURL { get; set; }

        public string Vendor { get; set; }

        public double Price { get; set; }

        public bool IsAviable { get; set; }

        public string Params { get; set; } // It's array |

        public string CategoryTitle { get; set; } // Table Category
        
        public string CurrencyId { get; set; } // Table Currency
    }
}
