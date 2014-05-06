using UnityEngine;
using System.Collections;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField]
    private float _changeInterval;

    [SerializeField]
    private Renderer _targetRenderer;

    [SerializeField]
    private Texture[] _sprites;

    private float _waitTime;
    private int _currentSpriteIndex;


    private void Start()
    {
        //_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_targetRenderer == null)
            Debug.LogError("targetRenderer not found", this);
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
        _targetRenderer.material.mainTexture = _sprites[_currentSpriteIndex];
    }

    private void OnDisable()
    {
        _targetRenderer.material.mainTexture = null;
        _waitTime = 0f;
    }

}