using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : MonoBehaviour
{
    public float speed;
    public Transform player;
    public AudioClip whizz;
    public AudioClip explosion;
    [HideInInspector] public AudioMaster audioMaster;

    void Start()
    {
        // audioMaster.playArtillery(whizz);
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed);
    }

    void OnCollisionStay(Collision other)
    {
        // audioMaster.playArtillery(explosion);
        if (other.gameObject.CompareTag("Shield"))
        {
            Destroy(this.gameObject);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<Player>().ArtilleryHit();
            Destroy(this.gameObject);
        }
    }
}
