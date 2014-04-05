
namespace UnityEngine
{
    public static class RandomUtils
    {
        public static int RangeWithExclude(int min, int max, int value)
        {
            if (min >= (max - 1))
            {
                Debug.LogError("не корректный диапазон");
                return min;
            }

            int rnd;
            do
            {
                rnd = Random.Range(min, max);
            } while (rnd == value);
            return rnd;
        }
    }
}
