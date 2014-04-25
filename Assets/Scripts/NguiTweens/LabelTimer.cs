using UnityEngine;
using System.Collections;

public class LabelTimer : MonoBehaviour
{
    [SerializeField]
    private float _startTime = 30f;

    private UILabel _label;

    public float RemainTime { get; private set; }

    //private float remainTimeMS;

    void Start ()
	{
	    _label = this.GetSafeComponent<UILabel>();
	    RemainTime = _startTime;
	}

    void Update()
    {
        if (RemainTime <= 0)
        {
            _label.text = "00";
            return;
        }
        float seconds = Time.timeSinceLevelLoad;

        int ms = Mathf.FloorToInt(seconds * 10.0f) - ((int)seconds * 10);//10-ые доли секунды
        ms= 9-ms;



        RemainTime = _startTime-seconds;
        int remainSec = Mathf.CeilToInt(RemainTime);
        //remainTimeMS = _startTime - seconds;

        //_label.text = ms.ToString();

        //_label.text = string.Format("{0}:{1}", RemainTime < 10 ? "0" + ((int) RemainTime).ToString() : ((int) RemainTime).ToString(), ms);
        _label.text = string.Format("{0}", remainSec < 10 ? "0" + remainSec.ToString() : remainSec.ToString());
    }
}
