using UnityEngine;
using System.Collections;

public class SignalTestMovement : MonoBehaviour {
    [SerializeField]
    private float _speed = 3.5f;

	void Start () {
	
	}
	
    void Update()
    {
        transform.position += (Vector3.forward * Time.deltaTime * _speed);
	}
}
