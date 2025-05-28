using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public bool destroyed;

    [Header("Attacks")]
    public float attackInterval;
    private float attackTimer;
    public float attackDistance;
    [SerializeField] private Transform gunOrigin;

    [Header("Movement")]
    [HideInInspector] public Transform player; 
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float verticalSpeed = 1.0f;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    private bool movingUp;

    [Header("Audio")]
    public AudioSource ambientSource;
    public AudioSource generalSource;
    public AudioClip chargingSFX;
    public AudioClip fireSFX;
    public AudioSource damageSource;
    public AudioClip hitSFX;
    public AudioClip destroyedSFX;

    [Header("Other")]
    public float deathTime; // How long after death will this despawn
    private float deathTimer;


    void Start()
    {
        minY += transform.position.y;
        maxY += transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (destroyed && Time.time > deathTimer)
        {
            Death();
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            destroyed = true;
            deathTimer = Time.time + deathTime;
        }
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    private void Movement()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            Vector3 direction = player.position - gunOrigin.position;
            Debug.DrawRay(gunOrigin.position, direction);
            if (Time.time > attackTimer)
            {
                Attack();
            }
        }
        else
        {
            transform.position += Time.deltaTime * transform.forward * speed;
        }

        if (movingUp)
        {
            transform.position += Time.deltaTime * transform.up * verticalSpeed;
            if (transform.position.y > maxY)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position += Time.deltaTime * transform.up * -verticalSpeed;
            if (transform.position.y < minY)
            {
                movingUp = true;
            }
        }
    }

    private void Attack()
    {
        Vector3 direction = player.position - gunOrigin.position;
        Physics.Raycast(gunOrigin.position, direction, out RaycastHit hit);
        if (hit.transform.CompareTag("Shield"))
        {
            Debug.Log("Blocked");
            return;
        }
        Debug.Log("Hit");
        player.parent.GetComponent<Player>().health -= 1;
        attackTimer = Time.time + attackInterval;
    }
}
