using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KnockBackOnCollision : MonoBehaviour
{
    public float knockBackPower = 20f;

    private void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if( rb != null && other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hiting enemy");
            Debug.Log(rb.gameObject.name);
            Vector3 dir = (other.transform.position - this.transform.position);
            dir.y = 0;
            rb.AddForce(dir.normalized*knockBackPower,ForceMode.Impulse);
        }
    }
}
