using SQLite;

namespace XML.classes.db.offer
{
    class OfferTable
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        [Unique, NotNull]
        public int OfferId { get; set; }

        [Unique, NotNull]
        public string Name { get; set; }
        
        [NotNull]
        public string Description { get; set; }
        
        [NotNull]
        public string URL { get; set; }

        [NotNull]
        public string PicturesURL { get; set; }

        [NotNull]
        public string Vendor { get; set; }

        [NotNull]
        public double Price { get; set; }

        [NotNull]
        public bool IsAviable { get; set; }

        [NotNull]
        public string Params { get; set; } // It's array |

        [NotNull]
        public string CategoryTitle { get; set; } // Table Category
        
        [NotNull]
        public string CurrencyId { get; set; } // Table Currency
    }
}
