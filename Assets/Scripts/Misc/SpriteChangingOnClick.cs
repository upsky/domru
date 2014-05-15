using UnityEngine;
using System.Collections;

public class SpriteChangingOnClick : MonoBehaviour
{
    [SerializeField]
    private Texture[] _sprites;

    [SerializeField]
    private Renderer _targetRenderer;

    [SerializeField]
    private float _duration = 1f;

    private Texture _defaultSprite;

    private void Start()
    {
        if (_targetRenderer == null)
            Debug.LogError("targetRenderer not found", this);

        _defaultSprite = _targetRenderer.material.mainTexture;
    }


    private void OnClick()
    {
        StartCoroutine(ChangeAlphaCoroutine());
    }

    private IEnumerator ChangeAlphaCoroutine()
    {
        var startTime = Time.time;
        int currentSpriteIndex = 0;
        while (_duration>(Time.time-startTime))
        {
            _targetRenderer.material.mainTexture = _sprites[currentSpriteIndex];
            
            currentSpriteIndex++;
            if (currentSpriteIndex >= _sprites.Length)
                currentSpriteIndex = 0;

            yield return new WaitForSeconds(0.1f);
        }

        _targetRenderer.material.mainTexture = _defaultSprite;
    }

}
