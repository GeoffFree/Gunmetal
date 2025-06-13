using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    private bool detonated;
    public float grenadeDestroyDelay;
    private float grenadeTimer = 100000;
    public ParticleSystem partSys;

    void Update()
    {
        if (grenadeTimer < Time.time)
        {
            Debug.Log("Remember me!");
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (detonated)
        {
            return;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, 500f);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Enemy>())
            {
                collider.GetComponent<Enemy>().TurnOff();
            }
        }
        detonated = true;
        partSys.Play();
    }
}
