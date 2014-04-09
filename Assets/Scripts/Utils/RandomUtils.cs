using System.Linq;

namespace UnityEngine
{
    public static class RandomUtils
    {
        public static int RangeWithExclude(int min, int max, params int[] values)
        {
            if (min >= (max - 1))
            {
                Debug.LogError("íå êîğğåêòíûé äèàïàçîí");
                return min;
            }

            int rnd;
            do
            {
                rnd = Random.Range(min, max);
            } while (values.Any(v=>v==rnd));
            return rnd;
        }
    }
}
