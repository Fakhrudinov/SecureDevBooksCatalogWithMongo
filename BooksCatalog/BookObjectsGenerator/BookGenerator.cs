using BooksCatalog;

namespace BookObjectsGenerator
{
    public static class BookGenerator
    {
        //Добавьте метод поиска по названию и описанию
        public static Book [] GetNewBooksArray(int count)
        {
            Book [] books = new Book [count];

            for (int i = 0; i < count; i++)
            {
                books [i] = GetNewBook();
            }

            return books;
        }

        private static Book GetNewBook()
        {
            return new Book
            {
                Author = AuthorName.GetNewAuthorName(),
                Name = BookName.GetNewBookName(),
                Category = BookCategory.GetNewCategory(),
                Price = BookPrice.GetNewBookPrice(),
                Description = BookDescription.GetNewDescription()
            };
        }
    }
}