using UnityEngine;
using System.Collections;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _sprites;

    [SerializeField]
    private float _changeInterval;

    private float _waitTime;
    private int _currentSpriteIndex;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer==null)
            Debug.LogError("SpriteRenderer not found", this);
        enabled = false;
    }

    private void Update()
    {
        if (_waitTime > 0)
        {
            _waitTime -= Time.deltaTime;
            return;
        }

        _waitTime = _changeInterval;
        _currentSpriteIndex = RandomUtils.RangeWithExclude(0, _sprites.Length, _currentSpriteIndex);
        _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
    }

    private void OnDisable()
    {
        _spriteRenderer.sprite = null;
        _waitTime = 0f;
    }

}