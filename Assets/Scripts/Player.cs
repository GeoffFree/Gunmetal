using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;

    private float currentHeat;
    public float overheatThreshold;
    private bool overheated; // Check if gun is overheated
    public float heatPerShot;

    public float coolingThreshold; // Heat level for the gun to fire again
    public float coolingRate; // Speed of cooling
    public float coolingDelay; // How long it takes after shooting for cooling to restore
    private float coolingDelayTimer;

    public float fireInterval;
    private float fireTimer;

    [SerializeField] private Transform gunOrigin;

    public void FixedUpdate()
    {
        if(Time.time > coolingDelayTimer) {
            currentHeat -= coolingRate;
        }
    }

    public void FireWeapon() {
        if(overheated) {
            if(currentHeat <= coolingThreshold) {
                overheated = false;
            }
            else {
                return;
            }
        }

        if(Time.time > fireTimer) {
            if(!Physics.Raycast(gunOrigin.position, gunOrigin.forward, out RaycastHit hit, 100)) {
                return;
            }
            if(hit.transform.gameObject.CompareTag("Enemy")) {
                hit.transform.GetComponent<Enemy>().Death();
            }
            fireTimer = Time.time + fireInterval;
            currentHeat += heatPerShot;
            coolingDelayTimer = Time.time + coolingDelay;
            if(currentHeat > overheatThreshold) {
                overheated = true;
            }
        }
    }
}