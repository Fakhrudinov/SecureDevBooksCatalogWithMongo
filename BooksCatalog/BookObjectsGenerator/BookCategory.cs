namespace BookObjectsGenerator
{
    internal class BookCategory
    {
        internal static string GetNewCategory()
        {
            BaseWords baseWords = new BaseWords();
            string result = baseWords.CategorySections[baseWords.Random.Next(0, baseWords.CategorySections.Length - 1)];

            return result;
        }
    }
}
