using BuildAnalyzer.Core;
using System.Globalization;

internal class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Анализатор игровой сборки ===");
        Console.WriteLine();

        string name = ReadName("Введите имя персонажа: ");
        double baseAttack = ReadDouble("Введите базовую атаку: ", 0, double.MaxValue);
        double weaponAttack = ReadDouble("Введите атаку оружия: ", 0, double.MaxValue);
        double critChance = ReadDouble("Введите шанс критического удара (0-100, %): ", 0, 100);
        double critDamage = ReadDouble("Введите критический урон (%): ", 0, double.MaxValue);

        try
        {
            var build = new CharacterBuild(name, baseAttack, weaponAttack, critChance, critDamage);

            double totalAttack = BuildCalculator.CalculateTotalAttack(build);
            double critDamageValue = BuildCalculator.CalculateCritDamage(build);
            double averageDamage = BuildCalculator.CalculateAverageDamage(build);
            BuildRating rating = BuildCalculator.DetermineRating(averageDamage);

            Console.WriteLine();
            Console.WriteLine($"--- Результат для персонажа «{build.Name}» ---");
            Console.WriteLine($"Итоговая атака:           {totalAttack:F2}");
            Console.WriteLine($"Средний урон:             {averageDamage:F2}");
            Console.WriteLine($"Шанс критического удара:  {build.CritChance:F2}%");
            Console.WriteLine($"Критический урон:         {build.CritDamagePercent:F2}% (удар crit = {critDamageValue:F2})");
            Console.WriteLine($"Рейтинг сборки:           {BuildCalculator.GetRatingName(rating)}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка данных: {ex.Message}");
        }
    }

    /// <summary>
    /// Запрашивает у пользователя непустую строку.
    /// </summary>
    private static string ReadName(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.WriteLine("Имя не может быть пустым. Попробуйте ещё раз.");
        }
    }

    /// <summary>
    /// Запрашивает число в заданном диапазоне, пока не будет введено корректное значение.
    /// Принимает и запятую, и точку как разделитель.
    /// </summary>
    private static double ReadDouble(string prompt, double min, double max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (input != null)
            {
                input = input.Replace(',', '.');

                if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
                    && value >= min && value <= max)
                {
                    return value;
                }
            }

            Console.WriteLine("Некорректное значение. Введите число в допустимом диапазоне.");
        }
    }
}