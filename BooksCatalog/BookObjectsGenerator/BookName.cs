namespace BookObjectsGenerator
{
    internal static class BookName
    {
        internal static string GetNewBookName()
        {
            BaseWords baseWords = new BaseWords();
            string result = "";

            for (int i = 0; i < 3; i++)
            {
                result = result + baseWords.BaseWordsArray[baseWords.Random.Next(0, baseWords.BaseWordsArray.Length - 1)] + " ";
            }

            result = result.Substring(0, 1).ToUpper() + result.Substring(1).Trim();

            return result;
        }
    }
}
