using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public GameMaster gameMaster;

    [Header("Gun")]
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

    [Header("Shield")]
    public GameObject shield;
    public Transform cam;
    public Transform leftController;
    public Transform rightController;

    [SerializeField] private Transform gunOrigin;

    void Start()
    {
        cam = Camera.main.transform;
    }

    public void FixedUpdate()
    {
        if (Time.time > coolingDelayTimer)
        {
            currentHeat -= coolingRate;
        }
        Vector3 cameraRelative = cam.InverseTransformPoint(leftController.position - rightController.position);
        if (cameraRelative.x > 0)
        {
            RaiseShield();
        }
        else
        {
            LowerShield();
        }
    }

    public void FireWeapon()
    {
        if (overheated)
        {
            if (currentHeat <= coolingThreshold)
            {
                overheated = false;
            }
            else
            {
                return;
            }
        }

        if (Time.time > fireTimer)
        {
            if (!Physics.Raycast(gunOrigin.position, gunOrigin.forward, out RaycastHit hit, 100))
            {
                return;
            }
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<Enemy>().Death();
                gameMaster.deadEnemies += 1;
            }
            fireTimer = Time.time + fireInterval;
            currentHeat += heatPerShot;
            coolingDelayTimer = Time.time + coolingDelay;
            if (currentHeat > overheatThreshold)
            {
                overheated = true;
            }
        }
    }

    public void RaiseShield()
    {
        shield.SetActive(true);
    }

    public void LowerShield()
    {
        shield.SetActive(false);
    }
}