using System.Linq;

public static class PrintUtils
{
    public static string BetterToString<T>(this T[] array)
    {
        string result = "[";

        array.Select(elem => result += elem.ToString() + ",");
        result += "]";
        return result;
    }

}

