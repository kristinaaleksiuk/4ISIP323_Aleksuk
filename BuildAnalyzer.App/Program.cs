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