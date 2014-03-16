using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour
{
    private enum ActivityType
    {
        None,
        DirectMovement,
        PathFinderMovement
    }

    [SerializeField]
    private float _firstWalkStartInterval = 3f;
    
    [SerializeField]
    private float _walAfterWalkInterval = 3f;

    [SerializeField]
    private float _walkAfterClickInterval = 7f;


    private IPathFinderMovement _pathFinderMovement;
    private ISimpleMovement _directMovement;

    //private Transform _currentTarget;
    private int _lastTargetIndex = -1;
    private Animator _animator;

    private ActivityType _currentActivityType;
    private bool _isStopedAnyActivity;

    private float _walkInterval;

    private void Start()
    {
        _pathFinderMovement = this.GetInterfaceComponent<IPathFinderMovement>();
        if (_pathFinderMovement == null)
        {
            Debug.LogWarning("_pathFinderMovement=null", this);
            return;
        }

        _directMovement = this.GetInterfaceComponent<ISimpleMovement>();
        if (_directMovement == null)
        {
            Debug.LogWarning("_directMovement=null", this);
            return;
        }

        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
        {
            Debug.LogWarning("_animator=null", this);
            return;
        }

        _walkInterval = _firstWalkStartInterval;
    }

    private void Update()
    {
        if (!_isStopedAnyActivity && _currentActivityType == ActivityType.None)
        {
            _currentActivityType = ActivityType.PathFinderMovement;
            Invoke("StartWalk", _walkInterval);
        }
    }

    /// <summary>
    /// Завершение текущего действия и затем прекращение любой дальнейшей деятельности.  
    /// </summary>
    public void StopAnyActivity()
    {
        _isStopedAnyActivity = true;
    }

    private void StartWalk()
    {
        _currentActivityType = ActivityType.PathFinderMovement;
        var currentTarget = SelectTarget();
        _pathFinderMovement.StartMovement(currentTarget, () => _animator.SetTrigger("StartWalk"), StopWalk);
    }

    private void StopWalk()
    {
        _animator.SetTrigger("StopWalk");
        _currentActivityType = ActivityType.None;
    }

    private void OnClick()
    {
        if (_isStopedAnyActivity || _currentActivityType == ActivityType.DirectMovement)
            return;

        if (_currentActivityType == ActivityType.PathFinderMovement)
            _pathFinderMovement.CancelMovement();

        //start directionMovement
        _currentActivityType = ActivityType.DirectMovement;
        var currentTarget = SelectTarget();
        _directMovement.StartMovement(currentTarget, StopWalk);

        Debug.LogWarning("CAT");
    }

    private Transform SelectTarget()
    {
        int targetIndex;
        do
        {
            targetIndex = Random.Range(0, SceneContainers.SeekerTargets.childCount);
        } while (targetIndex == _lastTargetIndex); //цикл для исключения попытки повторного перемещения к цели, где кот уже находиться.
        
        _lastTargetIndex = targetIndex;     
        return SceneContainers.SeekerTargets.GetChild(targetIndex);
    }

    //private ActivityType GetCurrentActivityType()
    //{
    //    if (_currentActivity == null)
    //        return ActivityType.None;
    //    return _currentActivity is IPathFinderMovement ? ActivityType.PathFinderMovement : ActivityType.DirectMovement;
    //}
}
