using SQLite;

namespace XML.classes.db.parametrs
{
    class ParametrsTable
    {
        [PrimaryKey, Unique, NotNull]
        public string CategoryTitle { get; set; }

        [NotNull]
        public string Parametrs { get; set; }
    }
}
