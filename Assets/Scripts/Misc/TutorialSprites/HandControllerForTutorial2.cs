using UnityEngine;
using System.Collections;

public class HandControllerForTutorial2 : MonoBehaviour
{
    [SerializeField]
    private float _clickInterval = 3f;

    [SerializeField]
    private Camera _uiCamera;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _timeInterval = 2f;

    private Vector4 _startAnchor;

    UIRect _uiItem;
    private float _time = 0;

    private void Start()
    {
        _uiItem = this.GetSafeComponent<UIRect>();


        //_uiItem.leftAnchor.Set(0.5f, -74f);
        //_uiItem.rightAnchor.Set(0.5f,  74f);
        //_uiItem.bottomAnchor.Set(0.5f, -136f);
        //_uiItem.topAnchor.Set(0.5f, 28f);






        Invoke("SetToTarget", 0.1f);
        //SetToTarget();

        EventMessenger.Subscribe(GameEvent.OnTutorialCompleteShowText, this, () => Invoke("ShowSprite", _clickInterval));
        //Invoke("ShowSprite", _clickInterval); //
        //EventMessenger.Subscribe(GameEvent.EngGameProcess, this, () => Invoke("MoveToButtonStart", 0f));
    }


    private void SetToTarget()
    {
        float w = Screen.width;
        float h = Screen.height;
        float res = h / w;
        //-595//-622
        //-630//-590
        Vector3 offset = new Vector3(-0.41f, -1f, 0);
        Vector3 pos = _uiCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(_target.position + offset));
        //pos.z = 0;
        _uiItem.transform.position = pos;
        _uiItem.transform.SetLocalZ(0);
        //if (res > 1.55) //16:10
        //    _startAnchor = new Vector4(-630f, -482f, 426f, 590f);

        //if (res > 1.45 && res < 1.55) //3:2
        //    _startAnchor = new Vector4(-664f, -516f, 417f, 581f);

        //if (res < 1.45f)
        //    _startAnchor = new Vector4(_uiItem.leftAnchor.absolute, _uiItem.rightAnchor.absolute, _uiItem.bottomAnchor.absolute, _uiItem.topAnchor.absolute);

        //_uiItem.leftAnchor.Set(0.5f, _startAnchor.x);
        //_uiItem.rightAnchor.Set(0.5f, _startAnchor.y);
        //_uiItem.bottomAnchor.Set(0.5f, _startAnchor.z);
        //_uiItem.topAnchor.Set(0.5f, _startAnchor.w);

    }

    private IEnumerator MoveToButtonCoroutine()
    {
        while (_time / _timeInterval <= 1f)
        {
            var t = _time / _timeInterval;

            _uiItem.leftAnchor.Set(0.5f, Mathf.Lerp(_startAnchor.x, -74f, t));
            _uiItem.rightAnchor.Set(0.5f, Mathf.Lerp(_startAnchor.y, 74f, t));
            _uiItem.bottomAnchor.Set(0.5f, Mathf.Lerp(_startAnchor.z, -136f, t));
            _uiItem.topAnchor.Set(0.5f, Mathf.Lerp(_startAnchor.w, 28f, t));

            _time += Time.deltaTime;     
            yield return null;
        }
        Invoke("ClickToBtn", 1f);       
    }


    private void ShowSprite()
    {
        var sprite = GetComponent<UISprite>();
        sprite.enabled = true;
        //Invoke("ClickToTarget", 1f);
    }

    private void ClickToTarget()
    {
        var sprite = GetComponent<UISprite>();
        sprite.spriteName = "07_finger_red";

        //var btn = transform.parent.GetComponent<UIButton>();
        _target.SendMessage("OnClick");
        EventMessenger.SendMessage(GameEvent.OnTutorial2_ClickToTarget, this);
    }

    private void MoveToButtonStart()
    {
        var sprite = GetComponent<UISprite>();
        sprite.spriteName = "08_finger_gray";
        StartCoroutine(MoveToButtonCoroutine());
    }


    private void ClickToBtn()
    {
        var sprite = GetComponent<UISprite>();
        sprite.spriteName = "07_finger_red";

        var btn=transform.parent.GetComponent<UIButton>();
        btn.SendMessage("OnClick");
    }
}
