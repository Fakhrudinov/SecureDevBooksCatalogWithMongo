namespace BookObjectsGenerator
{
    internal static class AuthorName
    {
        internal static string GetNewAuthorName()
        {
            BaseWords baseWords = new BaseWords();
            string result = baseWords.BaseNamesArray[baseWords.Random.Next(0, baseWords.BaseNamesArray.Length - 1)] + " "; // First name

            result = result + baseWords.BaseNamesArray[baseWords.Random.Next(0, baseWords.BaseNamesArray.Length - 1)] +
                baseWords.BaseNameSuffixesArray[baseWords.Random.Next(0, baseWords.BaseNameSuffixesArray.Length - 1)]; // Last name

            return result;
        }
    }
}
