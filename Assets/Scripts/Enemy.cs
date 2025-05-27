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

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < attackDistance) {
            if(Time.time > attackTimer) {
                Attack();
            }
        }
        else {
            transform.position += Time.deltaTime * transform.forward * speed;
        }
        //Vertical movement
    }

    public void Death() {
        Destroy(this.gameObject);
    }

    private void Attack() {
        Vector3 direction = player.position - gunOrigin.position;
        Physics.Raycast(gunOrigin.position, direction, out RaycastHit hit);
        if(hit.transform.CompareTag("Shield")) {
            return;
        }
        player.parent.GetComponent<Player>().health -= 1;
        attackTimer = Time.time + attackInterval;
    }
}
