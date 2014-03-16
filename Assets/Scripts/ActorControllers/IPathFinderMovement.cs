using System;
using UnityEngine;


/// <summary>
/// Описывает методы для контроллеров перемещения с наличием поиска пути.
/// </summary>
interface IPathFinderMovement
{
    void StartMovement(Transform target, Action onMovementStart, Action onPathTraversed);
    void CancelMovement();
}
