using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour
{
    private const float _clickInterval = 3f;

	void Start ()
	{
        EventMessenger.Subscribe(GameEvent.OnTutorialCompleteShowText, this, () => Invoke("ShowSprite", _clickInterval));
	}
	
	void Update () {}

    private void ShowSprite()
    {
        var sprite = GetComponent<UISprite>();
        sprite.enabled = true;
        Invoke("Click", 1f);
    }

    private void Click()
    {
        var sprite = GetComponent<UISprite>();
        sprite.spriteName = "07_finger_red";

        var btn=transform.parent.GetComponent<UIButton>();
        btn.SendMessage("OnClick");
    }
}
