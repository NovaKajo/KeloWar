using System.Collections;
using System.Collections.Generic;
using Kelo.AI;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    [SerializeField] Color color = new Color(1, 1, 0, 0.75F);

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, GetComponent<AIController>().getRange());

    }
}
