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

    private enum CatAnimState
    {
        Sit = 0,
        Move = 1
    }

    [SerializeField]
    private float _firstWaitInterval = 3f;
    
    [SerializeField]
    private float _waitIntervalAfterWalk = 3f;

    [SerializeField]
    private float _waitIntervalAfterClick = 7f;


    private IPathFinderMovement _pathFinderMovement;
    private ISimpleMovement _directMovement;
    private RandomPlayAudio _rndPlayAudio;

    private int _lastTargetIndex;
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

        _rndPlayAudio = GetComponentInChildren<RandomPlayAudio>();
        if (_rndPlayAudio == null)
        {
            Debug.LogError("_rndPlayAudio=null", this);
            return;
        }        

        //установка ближашей цели в качестве стартовой позиции для исключения при следующем поиске целей 
        _lastTargetIndex = GetNearestTargetIndex();

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

    private void OnClick()
    {
        if (_isStoppedAnyActivity || _currentActivityType == ActivityType.DirectMovement || _waitTime > 0)
            return;

        if (_currentActivityType == ActivityType.PathFinderMovement)
            _pathFinderMovement.CancelMovement();

        StartDirectionMovement();
    }

    private void StartWalk()
    {
        _currentActivityType = ActivityType.PathFinderMovement;
        var currentTarget = SelectTarget();
        _pathFinderMovement.StartMovement(currentTarget, () => _animator.SetInteger("state", (int)CatAnimState.Move), StopWalk);
    }

    private void StopWalk()
    {
        if (_animator.GetInteger("state") == (int)CatAnimState.Move)
        {
            _animator.SetInteger("state", (int) CatAnimState.Sit);
        }
        _currentActivityType = ActivityType.None;
        _waitTime = _waitIntervalAfterWalk;
    }

    private void StartDirectionMovement()
    {
        _rndPlayAudio.Play();
        _currentActivityType = ActivityType.DirectMovement;
        _animator.SetInteger("state", (int)CatAnimState.Move);

        var currentTarget = SelectTarget();
        _directMovement.StartMovement(currentTarget, () =>
        {
            StopWalk();
            _waitTime = _waitIntervalAfterClick;
        });
    }

    private void RotateShape()
    {
        if (_isStoppedAnyActivity || _currentActivityType == ActivityType.DirectMovement || _waitTime > 0)
            return;

        VectorInt2 nodeIndex = transform.position;
        var shape = NodesGrid.Grid[nodeIndex.x, nodeIndex.y].Shape;
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

    private int GetNearestTargetIndex()
    {
        int nearestTargetIndex = 0;
        float minDist = Vector3.Distance(transform.position, SceneContainers.SeekerTargets.GetChild(0).position);
        for (int i = 1; i < SceneContainers.SeekerTargets.childCount; i++)
        {
            float dist = Vector3.Distance(transform.position, SceneContainers.SeekerTargets.GetChild(i).position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestTargetIndex = i;
            }
        }
        return nearestTargetIndex;
    }

}
