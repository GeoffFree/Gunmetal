using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Player player;
    public AudioMaster audioMaster;
    public AudioClip explosion;
    public GameObject brace;
    public GameObject explosionPartSys;
    public bool triggered;

    void OnCollisionEnter(Collision collision)
    {
        if (triggered)
        {
            return;
        }
        player.ArtilleryHit();
        audioMaster.playArtillery(explosion);
        Instantiate(explosionPartSys, transform.position, Quaternion.identity);
        triggered = true;
        brace.SetActive(false);
        Destroy(this.gameObject);
    }
}
