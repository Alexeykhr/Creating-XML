using System.Collections.Generic;

namespace Creating_XML.src.db.models
{
    public interface IModel
    {
        IEnumerable<T> List<T>();
        IEnumerable<T> One<T>();
    }
}
