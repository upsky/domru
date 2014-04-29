using UnityEngine;
using System.Collections;

public class TutorialMasterMoving : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    ShowText();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void ShowText()
    {
        var textSprite = transform.GetComponentInChildren<SpriteAlphaChanger>();
        textSprite.StartAlphaChanging();
    }
}
