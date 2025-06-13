using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public void OnCollisionEnter(Collision other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Enemy>())
            {
                collider.GetComponent<Enemy>().TurnOff();
            }
        }
        Destroy(gameObject);
    }
}
