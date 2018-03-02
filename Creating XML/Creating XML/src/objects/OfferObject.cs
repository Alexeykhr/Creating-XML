using Creating_XML.src.db.tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creating_XML.src.objects
{
    class OfferObject
    {
        public int Id { get; set; }

        public int Article { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }


        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencyRate { get; set; }


        public int CategoryId { get; set; }

        public string CategoryName { get; set; }


        public int VendorId { get; set; }

        public string VendorName { get; set; }


        //public List<OfferImageTable> Images { get; set; }
    }
}
