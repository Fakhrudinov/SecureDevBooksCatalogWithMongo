namespace BookObjectsGenerator
{
    internal class BaseWords
    {
        private const string baseWords = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Nullam dictum felis eu pede mollis pretium. " +
                "Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, " +
                "ultricies nec, pellentesque eu, pretium quis, sem. Aenean commodo ligula eget dolor. Nulla consequat massa quis enim. " +
                "Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis " +
                "vitae, justo. Integer tincidunt. Cras dapibus. Aenean vulputate eleifend tellus. Vivamus elementum semper nisi. " +
                "Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, " +
                "feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Donec vitae sapien ut libero venenatis faucibus. " +
                "Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. " +
                "Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque " +
                "sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus.";

        internal string[] BaseWordsArray { get; set; }
        internal Random Random { get; set; }
        internal string[] BaseNamesArray { get; set; } = new string [] 
        { 
            "Александр", "Ашот", "Петр", "Абрам", "Сидор", "Федор", "Карапет", "Пахом",
            "Иван", "Максим", "Федот", "Виктор", "Герасим", "Артем", "Семен", "Кондрат"
        };
        internal string[] BaseNameSuffixesArray { get; set; } = new string[]
        {
            "ов", "енко", "ишвили", "ян", "ович"
        };
        internal string[] CategorySections { get; set; } = new string[]
        {
            "Животные","Астрономия","Строительство","Красота и здоровье","Блокноты","Тетради","Календари","Культура","Словари",
            "Электронные книги","Энциклопедии","Домоводство","География, туризм, путеводители, страноведение, путешествия","История",
            "Компьютеры","Коммерческая литература","Юридическая литература, право, закон, юридика","Военная литература",
            "Раритеты и букинистическая литература","Подарочные издания","Книги финских авторов в переводе на русский язык",
            "Книги для детей и подростков","Дом, семья, свободное время","Учебники и учебные пособия","Медицина","Филология, лингвистика",
            "Общественные и гуманитарные науки","Растения","Научно-популярная литература","Религия, теология, теософия",
            "Художественная литература","Естественные науки","Техническая литература","Транспорт",
        };

        internal BaseWords()
        {
            BaseWordsArray = baseWords.Split(" ");
            Random = new Random();
        }
    }
}
