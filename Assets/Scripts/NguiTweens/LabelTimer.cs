using System;
using UnityEngine;
using System.Collections;

public class LabelTimer : MonoBehaviour
{
    [SerializeField]
    private float _startTime = 30f;

    private UILabel _label;

    public float RemainTime { get; private set; }

    public float StartTime { get { return _startTime; } }

    void Start ()
	{
	    _label = this.GetSafeComponent<UILabel>();
	    RemainTime = _startTime;

        EventMessenger.Subscribe(GameEvent.StartGameProcess, this, () => StartCoroutine(UpdateTimeCoroutine(0.1f)));
        EventMessenger.Subscribe(GameEvent.EngGameProcess, this, StopAllCoroutines);
	}

    IEnumerator UpdateTimeCoroutine(float frequency)//0.1f
    {
        RemainTime = _startTime;
        _label.text = string.Format("{0}:{1}", RemainTime < 10 ? "0" + RemainTime.ToString() : RemainTime.ToString(), 0);

        while (RemainTime>0)
        {
            yield return new WaitForSeconds(frequency);
            RemainTime -= frequency;
            int remainSec = (int)Mathf.Floor(RemainTime + 0.01f);//+0.01f для исправления погрешности float
            int remainMs = MathfUtils.FracDecimalToCeil(RemainTime);
            _label.text = string.Format("{0}:{1}", remainSec < 10 ? "0" + remainSec.ToString() : remainSec.ToString(), remainMs);


            float w = Screen.width;
            float h = Screen.height;

            float res = h / w;
            _label.text = res.ToString();
        }        
         if (RemainTime <= 0)
         {
             _label.text = "00:0";
         }
    }
}
