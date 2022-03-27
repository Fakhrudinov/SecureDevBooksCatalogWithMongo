namespace BookObjectsGenerator
{
    internal static class BookDescription
    {
        internal static string GetNewDescription()
        {
            BaseWords baseWords = new BaseWords();
            string result = "";            

            while (result.Length < 60)
            {
                result = result + baseWords.BaseWordsArray[baseWords.Random.Next(0, baseWords.BaseWordsArray.Length - 1)] + " ";
            }

            result = result.Substring(0, 1).ToUpper() + result.Substring(1, 59);

            return result;
        }
    }
}
