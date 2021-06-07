using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kelo.Combat{

public class KnockBackOnCollision : MonoBehaviour
{
    [SerializeField] float knockBackPower = 20f;
    [SerializeField] string tagToKnock = "Enemy";

    private void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if( rb != null && other.gameObject.CompareTag(tagToKnock))
        {
            Vector3 dir = (other.transform.position - this.transform.position);
            dir.y = 0;
            rb.AddForce(dir.normalized*knockBackPower,ForceMode.Impulse);
        }
    }
}

}