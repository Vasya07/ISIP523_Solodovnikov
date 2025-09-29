class Program
{
    static List<Dictionary<string, object>> statisticsHistory = new List<Dictionary<string, object>>();

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Программа для анализа текста");
            Console.WriteLine("1. Анализ нового текста");
            Console.WriteLine("2. Просмотр истории анализов");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AnalyzeNewText();
                    break;
                case "2":
                    ShowStatisticsHistory();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Нет такого пункта в меню");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void AnalyzeNewText()
    {
        Console.Clear();
        Console.WriteLine("Анализ нового текста");
        Console.WriteLine("Введите текст, состоящий как минимум, из 100 символов:");
        string text = Console.ReadLine();

        // Проверка длины текста
        if (text.Length < 100)
        {
            Console.WriteLine("Текст должен содержать как минимум 100 символов, введите другой!");
            Console.ReadKey();
            return;
        }

        // Записываем время начала анализа
        DateTime analysisTime = DateTime.Now;

        Dictionary<string, object> stats = PerformCompleteTextAnalysis(text);

        // Добавляем время анализа в статистику
        stats["AnalysisTime"] = analysisTime;
        statisticsHistory.Add(stats);

        DisplayCurrentAnalysis(stats);
        Console.ReadKey();
    }

    static Dictionary<string, object> PerformCompleteTextAnalysis(string text)
    {
        Dictionary<string, object> stats = new Dictionary<string, object>();

        // Сохраняем исходный текст
        stats["Text"] = text;

        // 1) Подсчёт количества слов
        int wordCounter = 1;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == ' ')
            {
                wordCounter++;
            }
        }
        stats["WordCount"] = wordCounter;

        // 2 и 3) Поиск самого короткого и длинного слова
        string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string shortestWord = words[0];
        string longestWord = words[0];

        foreach (string word in words)
        {
            if (word.Length < shortestWord.Length)
                shortestWord = word;

            if (word.Length > longestWord.Length)
                longestWord = word;
        }
        stats["ShortestWord"] = shortestWord;
        stats["LongestWord"] = longestWord;

        // 4) Подсчёт количества предложений
        string[] sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        stats["SentenceCount"] = sentences.Length;

        // 5) Подсчёт гласных и согласных
        string russian_Vowels = "аеёиоуыэюяАЕЁИОУЫЭЮЯ";
        string english_Vowels = "aeiouyAEIOUY";
        string russian_Consonants = "бвгджзйклмнпрстфхцчшщБВГДЖЗЙКЛМНПРСТФХЦЧШЩ";
        string english_Consonants = "bcdfghjklmnpqrstvwxzBCDFGHJKLMNPQRSTVWXZ";

        int vowels = 0;
        int consonants = 0;

        foreach (char c in text)
        {
            if (russian_Vowels.Contains(c) || english_Vowels.Contains(c))
            {
                vowels++;
            }
            else if (english_Consonants.Contains(c) || russian_Consonants.Contains(c))
            {
                consonants++;
            }
        }
        stats["VowelsCount"] = vowels;
        stats["ConsonantsCount"] = consonants;

        // 6) Статистика букв
        Dictionary<char, int> letterFrequency = new Dictionary<char, int>();

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                char lowerChar = char.ToLower(c);

                if (letterFrequency.ContainsKey(lowerChar))
                    letterFrequency[lowerChar]++;
                else
                    letterFrequency[lowerChar] = 1;
            }
        }
        stats["LetterFrequency"] = letterFrequency;

        return stats;
    }

    static void DisplayCurrentAnalysis(Dictionary<string, object> stats)
    {
        Console.WriteLine("\nВот результат анализа введённого Вами текста:\n");

        // Выводим время анализа
        DateTime analysisTime = (DateTime)stats["AnalysisTime"];
        Console.WriteLine($"Время анализа: {analysisTime:dd.MM.yyyy HH:mm:ss}");

        Console.WriteLine($"Количество слов: {stats["WordCount"]}");
        Console.WriteLine($"Самое короткое слово: '{stats["ShortestWord"]}'");
        Console.WriteLine($"Самое длинное слово: '{stats["LongestWord"]}'");
        Console.WriteLine($"Количество предложений: {stats["SentenceCount"]}");
        Console.WriteLine($"Гласные: {stats["VowelsCount"]}, Согласные: {stats["ConsonantsCount"]}");

        Console.WriteLine("\nСтатистика букв:");
        var letterFrequency = (Dictionary<char, int>)stats["LetterFrequency"];
        foreach (var pair in letterFrequency)
        {
            Console.WriteLine($"  Буква '{pair.Key}': {pair.Value} раз");
        }
    }

    static void ShowStatisticsHistory()
    {
        Console.Clear();
        Console.WriteLine("История анализов текста");

        if (statisticsHistory.Count == 0)
        {
            Console.WriteLine("История анализов пуста!");
            Console.ReadKey();
            return;
        }

        // Вывод списка анализов с временем
        Console.WriteLine("\nСписок анализов:");
        for (int i = 0; i < statisticsHistory.Count; i++)
        {
            var stats = statisticsHistory[i];
            string text = (string)stats["Text"];
            DateTime analysisTime = (DateTime)stats["AnalysisTime"];
            string preview = text.Length > 50 ? text.Substring(0, 50) + "..." : text;

            // Выводим номер, время и превью текста
            Console.WriteLine($"{i + 1}. [{analysisTime:dd.MM.yyyy HH:mm}] {preview}");
        }

        Console.Write("\nВведите номер анализа для просмотра (0 - вернуться): ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= statisticsHistory.Count)
        {
            DisplayHistoryAnalysis(statisticsHistory[choice - 1]);
        }
    }

    static void DisplayHistoryAnalysis(Dictionary<string, object> stats)
    {
        Console.Clear();
        Console.WriteLine("Анализ из истории");
        DisplayCurrentAnalysis(stats);
        Console.ReadKey();
    }
}