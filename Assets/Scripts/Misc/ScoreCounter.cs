using UnityEngine;
using System.Collections;

public static class ScoreCounter 
{
    /// <summary>
    /// Функция принимает значения time в диапазоне от 0 до 30, а также рассчитана на то, что 1000 - максимальное значение
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static int TimeToStore(float time)
    {
        float x = Mathf.Pow(1.2545117989873634979779716841611f, time)+100;// 1.2589254117941672104239541063958f
        return (int)x;
    }

}