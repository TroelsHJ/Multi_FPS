using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            var health = other.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(10);
            }
        }

        Destroy(gameObject);
    }

}
