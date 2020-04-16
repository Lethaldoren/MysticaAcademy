using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
       if (other.tag == "Enemy")
        {
            Health health = other.GetComponent<Health>();

            if (health)
                health.Damage(100);
        }
    }
}
