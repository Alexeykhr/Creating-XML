using SQLite;

namespace XML.classes.db.category
{
    class CategoryTable
    {
        [PrimaryKey, Unique]
        public int CategoryId { get; set; }

        public int ParCategoryId { get; set; }

        [Unique]
        public string Title { get; set; }
    }
}
