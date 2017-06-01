using SQLite;

namespace XML.classes.db.parametrs
{
    class ParametrsTable
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        [Unique]
        public string CategoryTitle { get; set; }

        public string Parametrs { get; set; }
    }
}
