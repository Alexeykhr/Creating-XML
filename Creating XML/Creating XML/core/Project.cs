namespace Creating_XML.core
{
    abstract class Project
    {
        private static string _file_uri;

        public static string FileUri
        {
            get => _file_uri;
            set => _file_uri = value;
        }
    }
}
