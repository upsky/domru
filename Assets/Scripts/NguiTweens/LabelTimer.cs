using UnityEngine;
using System.Collections;

public class LabelTimer : MonoBehaviour
{
    private UILabel _label;
    private int _startTime = 30;
    public int RemainTime { get; private set; }

    void Start ()
	{
	    _label = this.GetSafeComponent<UILabel>();
	    RemainTime = _startTime;
	}

    void Update()
    {
        if (RemainTime <= 0)
            return;

        int seconds = (int)Time.timeSinceLevelLoad;
        RemainTime = _startTime-seconds;
        _label.text = string.Format("{0}", RemainTime < 10 ? "0" + RemainTime.ToString() : RemainTime.ToString());
    }
}
