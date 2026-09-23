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
