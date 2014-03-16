using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour
{
    [SerializeField]
    private float _firstWalkStartInterval = 3f;
    
    [SerializeField]
    private float _walkInterval = 3f;

    [SerializeField]
    private float _walkAfterClickInterval = 3f;


    private IPathFinderMovement _movementController;

    //private Transform _currentTarget;
    private int _lastTargetIndex = -1;
    private Animator _animator;

    private bool _hasActivity;
    private bool _IsStopedAnyActivity;

    private void Start()
    {
        _movementController = GetComponent<AstarMovementController>();
        if (_movementController == null)
        {
            Debug.LogWarning("_movementController=null", this);
            return;
        }

        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
        {
            Debug.LogWarning("_animator=null", this);
            return;
        }

        StartWalk();
    }

    private void Update()
    {
        if (_hasActivity)

        Invoke("StartWalk", 5f);
    }

    private void StartWalk()
    {
        var currentTarget = SelectTarget();
        _movementController.StartMovement(currentTarget, () => _animator.SetTrigger("StartWalk"), StopWalk);
    }

    private void StopWalk()
    {
        _animator.SetTrigger("StopWalk");
    }


    /// <summary>
    /// Завершение текущего действия и затем прекращение любой дальнейшей деятельности.  
    /// </summary>
    public void StopAnyActivity()
    {
        _IsStopedAnyActivity = true;
    }

    private void OnClick()
    {
        Debug.LogWarning("CAT");
    }

    private void DirectMove(Vector2 targetPoint)
    {
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
}
