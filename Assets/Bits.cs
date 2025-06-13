using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bits : MonoBehaviour
{
    public float destroyInterval;
    private float destroyTimer;

    void OnCollisionEnter(Collision collision)
    {
        destroyTimer = destroyInterval + Time.time;
    }

    void Update()
    {
        if (destroyTimer > Time.time)
        {
            Destroy(gameObject);
        }
    }
}
