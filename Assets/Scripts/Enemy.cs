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
    public GameObject[] droneBits;


    void Start()
    {
        minY += transform.position.y;
        maxY += transform.position.y;
        Vector3 targetDir = player.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 500, 500);
        transform.rotation = Quaternion.LookRotation(-newDir);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            foreach (GameObject bit in droneBits)
            {
                GameObject newBit = Instantiate(bit, transform.position, Quaternion.identity);
                newBit.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * 100);
            }
            Destroy(gameObject);
        }
    }

    private void Movement()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            Vector3 direction = player.position - gunOrigin.position;
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
            return;
        }
        player.parent.GetComponent<Player>().Damaged(1);
        attackTimer = Time.time + attackInterval;
    }
}
