using UnityEngine;
using System.Collections;

public class SpriteAlphaChanger : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;

    private const string _colorPropertyName = "_Color";

    private void Awake()
    {
        var c = renderer.material.GetColor(_colorPropertyName);
        c.a = 0f;
        renderer.material.SetColor(_colorPropertyName, c);
    }


    // Use this for initialization
	void Start () {
	
	}


    public void StartAlphaChanging()
    {
        StartCoroutine(ChangeAlphaCoroutine());
    }

    //private void ChangeAlpha()
    //{
    //    var c = renderer.material.GetColor(_colorPropertyName);
    //    if (c.a>=255)
    //        return;

    //    c.a += _speed * Time.deltaTime;
    //    renderer.material.SetColor(_colorPropertyName, c);
    //}

    private IEnumerator ChangeAlphaCoroutine()
    {
        var c = renderer.material.GetColor(_colorPropertyName);
        while (c.a < 255)
        {
            c.a += _speed * Time.deltaTime;
            renderer.material.SetColor(_colorPropertyName, c);
            yield return null;
        }
    }

}
