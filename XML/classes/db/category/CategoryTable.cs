using SQLite;

namespace XML.classes.db.category
{
    class CategoryTable
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        [Unique]
        public int CategoryId { get; set; }

        [Unique]
        public string Title { get; set; }
    }
}
