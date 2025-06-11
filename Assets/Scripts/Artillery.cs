using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryShip : MonoBehaviour
{
    public float speed;
    public float artilleryInterval;
    private float artilleryTimer;
    private bool hasFired;
    public Player player;
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
            player.ArtilleryHit();
            hasFired = true;
        }
    }
}
