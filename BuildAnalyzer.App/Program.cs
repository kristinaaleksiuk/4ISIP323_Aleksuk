namespace BuildAnalyzer.Core;

public enum BuildRating
{
    /// <summary>Средний урон менее 1500.</summary>
    Weak,

    /// <summary>Средний урон от 1500 до 2499.</summary>
    Normal,

    /// <summary>Средний урон от 2500 до 3499.</summary>
    Good,

    /// <summary>Средний урон 3500 и более.</summary>
    Excellent
}
public class CharacterBuild
{
    /// <summary>
    /// Создаёт новую сборку персонажа.
    /// </summary>
    /// <param name="name">Имя персонажа.</param>
    /// <param name="baseAttack">Базовая атака (не может быть отрицательной).</param>
    /// <param name="weaponAttack">Атака оружия (не может быть отрицательной).</param>
    /// <param name="critChance">Шанс критического удара в процентах (от 0 до 100).</param>
    /// <param name="critDamagePercent">Критический урон в процентах (не может быть отрицательным).</param>
    /// <exception cref="ArgumentException">Имя пустое.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Одно из чисел вне допустимого диапазона.</exception>
    public CharacterBuild(string name, double baseAttack, double weaponAttack,
                          double critChance, double critDamagePercent)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя персонажа не может быть пустым.", nameof(name));
        if (baseAttack < 0)
            throw new ArgumentOutOfRangeException(nameof(baseAttack), "Базовая атака не может быть отрицательной.");
        if (weaponAttack < 0)
            throw new ArgumentOutOfRangeException(nameof(weaponAttack), "Атака оружия не может быть отрицательной.");
        if (critChance < 0 || critChance > 100)
            throw new ArgumentOutOfRangeException(nameof(critChance), "Шанс крита должен быть от 0 до 100.");
        if (critDamagePercent < 0)
            throw new ArgumentOutOfRangeException(nameof(critDamagePercent), "Критический урон не может быть отрицательным.");

        Name = name.Trim();
        BaseAttack = baseAttack;
        WeaponAttack = weaponAttack;
        CritChance = critChance;
        CritDamagePercent = critDamagePercent;
    }

    /// <summary>Имя персонажа.</summary>
    public string Name { get; }

    /// <summary>Базовая атака персонажа.</summary>
    public double BaseAttack { get; }

    /// <summary>Атака экипированного оружия.</summary>
    public double WeaponAttack { get; }

    /// <summary>Шанс критического удара, в процентах (0–100).</summary>
    public double CritChance { get; }

    /// <summary>Критический урон, в процентах.</summary>
    public double CritDamagePercent { get; }
}
public static class BuildCalculator
{
    /// <summary>
    /// Считает итоговую атаку: базовая атака + атака оружия.
    /// </summary>
    /// <param name="build">Сборка персонажа.</param>
    /// <returns>Итоговая атака.</returns>
    public static double CalculateTotalAttack(CharacterBuild build)
    {
        ArgumentNullException.ThrowIfNull(build);
        return build.BaseAttack + build.WeaponAttack;
    }

    /// <summary>
    /// Считает урон при критическом ударе:
    /// итоговая атака × (1 + критический урон% / 100).
    /// </summary>
    /// <param name="build">Сборка персонажа.</param>
    /// <returns>Урон одного критического удара.</returns>
    public static double CalculateCritDamage(CharacterBuild build)
    {
        double totalAttack = CalculateTotalAttack(build);
        return totalAttack * (1 + build.CritDamagePercent / 100);
    }

    /// <summary>
    /// Считает средний ожидаемый урон:
    /// итоговая атака × (1 + шанс крита% / 100 × критический урон% / 100).
    /// </summary>
    /// <param name="build">Сборка персонажа.</param>
    /// <returns>Средний урон с учётом вероятности критического удара.</returns>
    public static double CalculateAverageDamage(CharacterBuild build)
    {
        double totalAttack = CalculateTotalAttack(build);
        return totalAttack * (1 + build.CritChance / 100 * build.CritDamagePercent / 100);
    }

    /// <summary>
    /// Определяет рейтинг сборки по среднему урону.
    /// </summary>
    /// <param name="averageDamage">Средний урон.</param>
    /// <returns>Рейтинг сборки.</returns>
    public static BuildRating DetermineRating(double averageDamage)
    {
        if (averageDamage < 1500) return BuildRating.Weak;
        if (averageDamage < 2500) return BuildRating.Normal;
        if (averageDamage < 3500) return BuildRating.Good;
        return BuildRating.Excellent;
    }

    /// <summary>
    /// Возвращает название рейтинга на русском языке.
    /// </summary>
    /// <param name="rating">Рейтинг сборки.</param>
    /// <returns>Текстовое название рейтинга.</returns>
    public static string GetRatingName(BuildRating rating) => rating switch
    {
        BuildRating.Weak => "Слабая",
        BuildRating.Normal => "Нормальная",
        BuildRating.Good => "Хорошая",
        BuildRating.Excellent => "Отличная",
        _ => "Неизвестно"
    };
}