namespace Creating_XML.src
{
    class Settings
    {
        private static string _last_file_uri;

        private const string LAST_FILE_URI = "last_file_uri";

        public static string Domain
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_last_file_uri))
                    return _last_file_uri;

                _last_file_uri = Get(LAST_FILE_URI).ToString();

                return _last_file_uri;
            }
            set
            {
                Set(LAST_FILE_URI, value);
                _last_file_uri = value;
            }
        }

        private static object Get(string name)
        {
            return Properties.Settings.Default[name];
        }

        private static void Set(string name, object value)
        {
            Properties.Settings.Default[name] = value;
            Properties.Settings.Default.Save();
        }
    }
}
