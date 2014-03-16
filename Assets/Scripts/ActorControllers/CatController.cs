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
    private float _firstWaitInterval = 3f;
    
    [SerializeField]
    private float _waitIntervalAfterWalk = 3f;

    [SerializeField]
    private float _waitIntervalAfterClick = 7f;


    private IPathFinderMovement _pathFinderMovement;
    private ISimpleMovement _directMovement;

    private int _lastTargetIndex = -1;
    private Animator _animator;

    private ActivityType _currentActivityType;
    private bool _isStoppedAnyActivity;

    private float _waitTime;

    private void Start()
    {
        _pathFinderMovement = this.GetInterfaceComponent<IPathFinderMovement>();
        if (_pathFinderMovement == null)
        {
            Debug.LogError("_pathFinderMovement=null", this);
            return;
        }
        _pathFinderMovement.OnWaypointChange = RotateShape;

        _directMovement = this.GetInterfaceComponent<ISimpleMovement>();
        if (_directMovement == null)
        {
            Debug.LogError("_directMovement=null", this);
            return;
        }

        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
        {
            Debug.LogError("_animator=null", this);
            return;
        }

        _waitTime = _firstWaitInterval;
    }

    private void Update()
    {        
        if (_waitTime > 0)
        {
            _waitTime -= Time.deltaTime;
            return;
        }

        if (!_isStoppedAnyActivity && _currentActivityType == ActivityType.None)
        {
            StartWalk();
        }
    }

    /// <summary>
    /// Завершение текущего действия и затем прекращение любой дальнейшей деятельности.  
    /// </summary>
    public void StopAnyActivity()
    {
        if (!_isStoppedAnyActivity && _currentActivityType == ActivityType.PathFinderMovement)
        {
            _pathFinderMovement.CancelMovement();
            StartDirectionMovement();
        }
        _isStoppedAnyActivity = true;
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
        _waitTime = _waitIntervalAfterWalk;
    }

    private void StartDirectionMovement()
    {
        _currentActivityType = ActivityType.DirectMovement;
        var currentTarget = SelectTarget();
        _directMovement.StartMovement(currentTarget, () =>
        {
            StopWalk();
            _waitTime = _waitIntervalAfterClick;
        });
    }

    private void OnClick()
    {
        if (_isStoppedAnyActivity || _currentActivityType == ActivityType.DirectMovement || _waitTime>0)
            return;

        if (_currentActivityType == ActivityType.PathFinderMovement)
            _pathFinderMovement.CancelMovement();

        StartDirectionMovement();
    }

    private void RotateShape()
    {
        if (_isStoppedAnyActivity || _currentActivityType == ActivityType.DirectMovement || _waitTime > 0)
            return;

        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.z);
        
        var shape=ShapesGrid.Grid[y, x];
        if (shape != null)
        {
            shape.RotateCommand();
        }
    }

    private Transform SelectTarget()
    {
        _lastTargetIndex = RandomUtils.RangeWithExclude(0, SceneContainers.SeekerTargets.childCount, _lastTargetIndex);
        return SceneContainers.SeekerTargets.GetChild(_lastTargetIndex);
    }

}
