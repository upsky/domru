
namespace UnityEngine
{
    public static class RandomUtils
    {
        public static int RangeWithExclude(int min, int max, int value)
        {
            int rnd;
            do
            {
                rnd = Random.Range(min, max);
            } while (rnd == value);
            return rnd;
        }
    }
}
