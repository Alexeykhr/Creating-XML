using SQLite;

namespace XML.classes.db.config
{
    class ConfigTable
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }


    }
}
