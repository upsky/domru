using System.Linq;

public static class Utils
{
    /// <summary>
    /// Проверяет, все ли элементы !=null
    /// </summary>
    public static bool IsNullAny<T>(params T[] array)
    {
        if (array == null)
            return true;
        return array.Any(value => value == null);
    }
	
	public static void Swap<T>(ref T lhs, ref T rhs)
	{
	    T temp = lhs;
		lhs = rhs;
		rhs = temp;
	}
}