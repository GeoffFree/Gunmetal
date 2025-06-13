using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public bool destroyed;
    public bool disabled;
    private float disabledTimer;

    [Header("Attacks")]
    public float attackInterval;
    private float attackTimer;
    public float attackDistance;
    [SerializeField] private Transform gunOrigin;

    [Header("Movement")]
    [HideInInspector] public Transform target;
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
    public ParticleSystem disabledParticles;

    void Start()
    {
        minY += transform.position.y;
        maxY += transform.position.y;
        Vector3 targetDir = target.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 500, 500);
        transform.rotation = Quaternion.LookRotation(-newDir);
    }

    // Update is called once per frame
    void Update()
    {
        if (disabledTimer > Time.time)
        {
            disabledParticles.Stop();
            disabled = false;
        }

        if (disabled)
        {
            disabledParticles.Play();
            return;
        }

        Movement();
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            foreach (GameObject bit in droneBits)
            {
                GameObject newBit = Instantiate(bit, transform.position, transform.rotation);
                newBit.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.insideUnitSphere * 100);
            }
            Destroy(gameObject);
        }
    }

    private void Movement()
    {
        Vector2 currentPos = new(transform.position.x, transform.position.z);
        Vector2 targetPos = new(target.position.x, target.position.z);
        if (Vector2.Distance(currentPos, targetPos) < 0.25)
        {
            if (target.childCount > 0)
            {
                int randomChild = UnityEngine.Random.Range(0, target.childCount);
                target = target.GetChild(randomChild);
            }
            else
            {
                target = player;
            }
            Vector3 targetDir = target.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 500, 500);
            transform.rotation = Quaternion.LookRotation(-newDir);
        }

        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            if (Time.time > attackTimer)
            {
                Attack();
            }
        }
        else
        {
            transform.position += speed * Time.deltaTime * transform.forward;
        }

        if (movingUp)
        {
            transform.position += Time.deltaTime * verticalSpeed * transform.up;
            if (transform.position.y > maxY)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position += -verticalSpeed * Time.deltaTime * transform.up;
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
            generalSource.clip = fireSFX;
            generalSource.Play();
            return;
        }
        player.parent.GetComponent<Player>().Damaged(1);
        attackTimer = Time.time + attackInterval;
    }

    public void TurnOff()
    {
        disabled = true;
        disabledTimer = Time.time + 5;
    }
}
