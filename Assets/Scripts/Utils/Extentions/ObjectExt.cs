using System.Linq;

public static class ObjectExt
{
    //Чтобы не писать условия c кучей "||" в enum или string при сравнении
    /// <summary>
    /// Проверяет наличие текущего элемента в переданном перечне значений
    /// </summary>
    public static bool In<T>(this T value, params T[] array)
    {
        return array.Contains(value);
    }
}
