using UnityEngine;
using System.Collections;

public class SignalTestMovement : MonoBehaviour {
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private bool _useFixedUpdate ;

	void Start () {
	
	}
	
    void Update()
    {
        if (!_useFixedUpdate)
            transform.position += (Vector3.forward * Time.deltaTime * _speed);
	}

    void FixedUpdate()
    {
        if (_useFixedUpdate)
            transform.position += (Vector3.forward * Time.fixedDeltaTime * _speed);
    }
}
