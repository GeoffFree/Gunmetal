using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryShip : MonoBehaviour
{
    public float speed;
    public float artilleryInterval;
    private float artilleryTimer;
    private bool hasFired;
    public Transform artilleryTransform;
    public Transform player;
    public AudioClip explosion;
    [HideInInspector] public AudioMaster audioMaster;

    void Start() {
        artilleryTimer = artilleryInterval + Time.time;
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * speed;
        if (artilleryTimer > Time.time && !hasFired)
        {
            if (Physics.Raycast(artilleryTransform.position, artilleryTransform.forward, out RaycastHit hit, 100))
            {
                if (hit.transform)
                {
                    GameObject whatWasHit = hit.transform.gameObject;
                    if (whatWasHit.CompareTag("Shield"))
                    {
                        return;
                    }
                    else if (whatWasHit.CompareTag("Player"))
                    {
                        whatWasHit.GetComponent<Player>().ArtilleryHit();
                    }
                }
            }
            hasFired = true;
        }
    }
}
