using SQLite;
using System.Collections.Generic;
using Creating_XML.src.db.tables;

namespace Creating_XML.src.db
{
    class Database
    {
        private List<Table> _tables = new List<Table> {
            new CategoryParametersTable(),
            new CategoryTable(),
            new CurrencyTable(),
            new OfferImageTable(),
            new OfferParametersTable(),
            new OfferTable(),
            new VendorTable()
        };

        public List<Table> Tables
        {
            get { return _tables; }
        }
    }
}
