using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 2.0f;
    public float attackInterval;
    private float attackTimer;
    public float attackDistance;
    [HideInInspector] public Transform player; 
    [SerializeField] private Transform gunOrigin;

    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float verticalSpeed;
    private bool movingUp;


    void Start()
    {
        minY += transform.position.y;
        maxY += transform.position.y;
    }

    // Update is called once per frame
    void Update()
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

    public void Death() {
        Destroy(this.gameObject);
    }

    private void Attack() {
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
