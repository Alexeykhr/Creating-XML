using SQLite;

namespace XML.classes.db.category
{
    class CategoryTable
    {
        [PrimaryKey, Unique, NotNull]
        public int CategoryId { get; set; }

        [Unique, NotNull]
        public string Title { get; set; }

        [NotNull]
        public int ParCategoryId { get; set; }
    }
}
