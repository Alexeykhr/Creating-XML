using Creating_XML.src.db.tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creating_XML.src.db.models
{
    class CategoryModel : IModel
    {
        public IEnumerable<CategoryTable> List<CategoryTable>()
        {
            return null;
        }

        public IEnumerable<CategoryTable> One<CategoryTable>()
        {
            return null;
        }
    }
}
