namespace BookObjectsGenerator
{
    internal static class BookPrice
    {
        internal static decimal GetNewBookPrice()
        {
            Random random = new Random();
            double fraction = random.NextDouble();
            int integer = random.Next(1, 9999);

            return (decimal) Math.Round((fraction + integer), 2);
        }
    }
}
