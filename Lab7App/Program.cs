using DoublyLinkedListLib;

internal class Program
{
    private static DoublyLinkedList _list = new();

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        PrintHeader();
        PreFillList();
        RunMenuLoop();
    }

    private static void RunMenuLoop()
    {
        bool running = true;

        while (running)
        {
            PrintMenu();
            string? input = Console.ReadLine()?.Trim();

            Console.WriteLine();

            switch (input)
            {
                case "1": ShowList();                   break;
                case "2": AddElement();                 break;
                case "3": RemoveByIndex();              break;
                case "4": ReadByIndex();                break;
                case "5": IterateWithForeach();         break;
                case "6": FindFirstOccurrence();        break;
                case "7": ShowSumAtOddPositions();      break;
                case "8": CreateFilteredList();         break;
                case "9": RemoveAboveAverage();         break;
                case "0": running = false;              break;
                default:
                    PrintWarning("Невідома команда. Спробуйте ще раз.");
                    break;
            }

            if (running)
            {
                Console.WriteLine();
                Console.Write("Натисніть Enter для продовження...");
                Console.ReadLine();
            }
        }

        PrintColored("\n  До побачення!\n", ConsoleColor.Cyan);
    }

    private static void ShowList()
    {
        PrintSectionTitle("Поточний стан списку");
        Console.WriteLine($"  Кількість елементів: {_list.Count}");
        Console.WriteLine($"  Список: {_list}");
    }

    private static void AddElement()
    {
        PrintSectionTitle("Додати елемент у кінець списку");

        int? value = ReadInt("  Введіть ціле число: ");
        if (value is null) return;

        _list.AddToEnd(value.Value);
        PrintSuccess($"Елемент {value} додано у кінець списку.");
        Console.WriteLine($"  Список: {_list}");
    }

    private static void RemoveByIndex()
    {
        PrintSectionTitle("Видалити елемент за індексом");

        if (CheckEmpty()) return;

        Console.WriteLine($"  Список: {_list}  (кількість: {_list.Count})");
        int? index = ReadInt($"  Введіть індекс [0..{_list.Count - 1}]: ");
        if (index is null) return;

        try
        {
            int removedValue = _list[index.Value];
            _list.RemoveAt(index.Value);
            PrintSuccess($"Елемент {removedValue} за індексом {index} видалено.");
            Console.WriteLine($"  Список: {_list}");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            PrintError(ex.Message);
        }
    }

    private static void ReadByIndex()
    {
        PrintSectionTitle("Зчитати елемент за індексом");

        if (CheckEmpty()) return;

        Console.WriteLine($"  Список: {_list}  (кількість: {_list.Count})");
        int? index = ReadInt($"  Введіть індекс [0..{_list.Count - 1}]: ");
        if (index is null) return;

        try
        {
            int value = _list[index.Value];
            PrintSuccess($"Елемент за індексом {index}: {value}");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            PrintError(ex.Message);
        }
    }

    private static void IterateWithForeach()
    {
        PrintSectionTitle("Ітерація списку за допомогою foreach");

        if (CheckEmpty()) return;

        int position = 1;
        Console.WriteLine("  Позиція | Значення");
        Console.WriteLine("  " + new string('─', 20));

        foreach (int item in _list)
        {
            Console.WriteLine($"     {position,3}  |  {item}");
            position++;
        }
    }

    private static void FindFirstOccurrence()
    {
        PrintSectionTitle("Знайти перше входження заданого елементу");

        if (CheckEmpty()) return;

        Console.WriteLine($"  Список: {_list}");
        int? value = ReadInt("  Введіть значення для пошуку: ");
        if (value is null) return;

        int index = _list.FindFirstOccurrence(value.Value);

        if (index == -1)
            PrintWarning($"Елемент {value} не знайдено у списку.");
        else
            PrintSuccess($"Перше входження {value}: позиція {index + 1} (індекс {index}).");
    }

    private static void ShowSumAtOddPositions()
    {
        PrintSectionTitle("Сума елементів на непарних позиціях (1, 3, 5, …)");

        if (CheckEmpty()) return;

        Console.WriteLine($"  Список (позиція | значення):");
        int pos = 1;
        foreach (int item in _list)
        {
            string marker = pos % 2 != 0 ? " ←  непарна" : "";
            Console.WriteLine($"     {pos,3}  |  {item}{marker}");
            pos++;
        }

        double sum = _list.SumAtOddPositions();
        Console.WriteLine();
        PrintSuccess($"Сума елементів на непарних позиціях: {sum}");
    }

    private static void CreateFilteredList()
    {
        PrintSectionTitle("Новий список з елементів, більших за задане значення");

        if (CheckEmpty()) return;

        Console.WriteLine($"  Вихідний список: {_list}");
        int? threshold = ReadInt("  Введіть порогове значення: ");
        if (threshold is null) return;

        DoublyLinkedList filtered = _list.GetListGreaterThan(threshold.Value);

        if (filtered.IsEmpty)
            PrintWarning($"Жодного елемента, більшого за {threshold}, не знайдено.");
        else
        {
            PrintSuccess($"Новий список (елементи > {threshold}):");
            Console.WriteLine($"  {filtered}");
            Console.WriteLine($"  Кількість елементів: {filtered.Count}");
        }
    }

    private static void RemoveAboveAverage()
    {
        PrintSectionTitle("Видалити елементи, більші за середнє значення");

        if (CheckEmpty()) return;

        Console.WriteLine($"  Список до видалення: {_list}");

        try
        {
            double realSum = 0;
            foreach (int item in _list) realSum += item;
            double realAvg = realSum / _list.Count;

            Console.WriteLine($"  Середнє значення: {realAvg:F2}");

            _list.RemoveGreaterThanAverage();

            PrintSuccess("Елементи, більші за середнє, видалено.");
            Console.WriteLine($"  Список після видалення: {_list}");
        }
        catch (InvalidOperationException ex)
        {
            PrintError(ex.Message);
        }
    }

    private static void PreFillList()
    {
        int[] demoValues = { 12, 7, 25, 3, 18, 7, 42, 9, 15, 7 };

        foreach (int value in demoValues)
            _list.AddToEnd(value);

        PrintColored($"  Список заповнено тестовими значеннями: {_list}\n", ConsoleColor.DarkGray);
    }

    private static bool CheckEmpty()
    {
        if (!_list.IsEmpty) return false;
        PrintWarning("Список порожній. Спочатку додайте елементи (пункт 2).");
        return true;
    }

    private static int? ReadInt(string prompt)
    {
        Console.Write(prompt);
        string? raw = Console.ReadLine();

        if (int.TryParse(raw?.Trim(), out int result))
            return result;

        PrintError($"'{raw}' не є цілим числом.");
        return null;
    }

    private static void PrintHeader()
    {
        Console.Clear();
        PrintColored("╔══════════════════════════════════════════════════════╗", ConsoleColor.Cyan);
        PrintColored("║     Лабораторна робота 7 — Двоспрямований список     ║", ConsoleColor.Cyan);
        PrintColored("║          Варіант 8 | Тип: int | Додавання в кінець   ║", ConsoleColor.Cyan);
        PrintColored("╚══════════════════════════════════════════════════════╝", ConsoleColor.Cyan);
        Console.WriteLine();
    }

    private static void PrintMenu()
    {
        Console.WriteLine();
        PrintColored("  ═══════════════ ГОЛОВНЕ МЕНЮ ═══════════════", ConsoleColor.Yellow);
        Console.WriteLine("  [1] Показати поточний список");
        Console.WriteLine("  [2] Додати елемент у кінець");
        Console.WriteLine("  [3] Видалити елемент за індексом");
        Console.WriteLine("  [4] Зчитати елемент за індексом");
        Console.WriteLine("  [5] Ітерувати список (foreach)");
        PrintColored("  ─── Операції за варіантом ──────────────────", ConsoleColor.DarkYellow);
        Console.WriteLine("  [6] Знайти перше входження елементу      (Оп. 1)");
        Console.WriteLine("  [7] Сума елементів на непарних позиціях  (Оп. 2)");
        Console.WriteLine("  [8] Новий список: елементи > значення    (Оп. 3)");
        Console.WriteLine("  [9] Видалити елементи > середнього       (Оп. 4)");
        PrintColored("  ─────────────────────────────────────────────", ConsoleColor.DarkYellow);
        Console.WriteLine("  [0] Вийти");
        Console.Write("\n  Ваш вибір: ");
    }

    private static void PrintSectionTitle(string title)
    {
        PrintColored($"\n  ── {title} ──", ConsoleColor.Cyan);
    }

    private static void PrintSuccess(string message)
    {
        PrintColored($"  ✓ {message}", ConsoleColor.Green);
    }

    private static void PrintError(string message)
    {
        PrintColored($"  ✗ Помилка: {message}", ConsoleColor.Red);
    }

    private static void PrintWarning(string message)
    {
        PrintColored($"  ⚠ {message}", ConsoleColor.Yellow);
    }

    private static void PrintColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}
