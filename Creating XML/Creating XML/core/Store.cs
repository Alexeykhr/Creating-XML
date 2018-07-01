using System.Collections.Generic;

namespace Creating_XML.core
{
    abstract class Store<T>
    {
        protected static IEnumerable<T> _list;
        
        public static IEnumerable<T> List
        {
            get => _list;
            set => _list = value;
        }
    }
}
