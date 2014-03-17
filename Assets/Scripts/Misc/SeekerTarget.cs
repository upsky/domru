using UnityEngine;
using System.Collections;

public class SeekerTarget : MonoBehaviour
{
    private void Start()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = ColorUtils.CreateWithAlpha(Color.blue, 0.5f);
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
