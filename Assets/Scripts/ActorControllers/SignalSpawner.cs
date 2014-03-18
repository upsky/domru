using UnityEngine;
using System.Collections;

public class SignalSpawner : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = ColorUtils.CreateWithAlpha(Color.green, 0.5f);
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
