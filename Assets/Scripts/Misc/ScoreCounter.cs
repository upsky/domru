using UnityEngine;
using System.Collections;

public static class ScoreCounter
{
    /// <summary>
    /// Функция рассчитана на то, что 1000 - максимальное значение очков
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static int TimeToStore(float startTime, float remainTime)
    {
        const float baseStartTime = 30f;//величина времени, относительно которой рассчитывается во сколько раз отличается стартовое время уровня
        float k = startTime / baseStartTime;
        float a = 1f / k / k;

        float v = a * remainTime * remainTime;
        return (int)(v + 100);
    }
}