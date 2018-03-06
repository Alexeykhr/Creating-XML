using Creating_XML.src.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creating_XML.src.store
{
    abstract class Store<T>
    {
        protected static IEnumerable<T> _list;

        /// <summary>
        /// Set and get List of VendorTable.
        /// </summary>
        public static IEnumerable<T> List
        {
            get { return _list; }
            set { _list = value; }
        }
    }
}
