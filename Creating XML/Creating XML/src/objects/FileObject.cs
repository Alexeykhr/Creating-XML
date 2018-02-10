using System;

namespace Creating_XML.src.objects
{
    [Serializable()]
    class FileObject
    {
        public string Uri { get; set; }

        public DateTime OpenedAt { get; set; }
    }
}
