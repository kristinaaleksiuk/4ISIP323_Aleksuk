using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildAnalyzer.Core;
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

