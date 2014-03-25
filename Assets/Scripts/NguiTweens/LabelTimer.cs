using UnityEngine;
using System.Collections;

public class LabelTimer : MonoBehaviour
{
    private UILabel _label;

	void Start ()
	{
	    _label = this.GetSafeComponent<UILabel>();
	}

    void Update()
    {
        int seconds = (int)Time.timeSinceLevelLoad;
        int minutes = (int)(seconds / 60);
        seconds = seconds - minutes * 60;

        _label.text = string.Format("{0}{1}{2}", minutes, seconds > 9 ? ":" : ":0", seconds);
    }
}
