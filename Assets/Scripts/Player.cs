using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public GameMaster gameMaster;

    [Header("Gun")]
    public int damage;
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
    private Transform cam;
    public Transform leftController;
    public Transform rightController;
    public float shieldThreshold;
    private bool isShieldUp;

    [Header("Audio")]
    public AudioSource shieldSource;
    public AudioClip raiseShieldSFX;
    public AudioClip lowerShieldSFX;
    public AudioSource fireSource;
    public AudioSource overheatSource;
    public AudioSource damagedSFX;

    [Header("Other")]
    [SerializeField] private Transform gunOrigin;
    public AudioMaster audioMaster;

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
        if (cameraRelative.x > shieldThreshold)
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
        // Don't allow firing gun if shield is up
        if (isShieldUp)
        {
            return;
        }

        // If gun is overheated, can't fire
        if (overheated)
        {
            // Only allow firing after player's gun has cooled down enough
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
                hit.transform.GetComponent<Enemy>().Damage(damage);
                gameMaster.deadEnemies += 1;
            }
            fireTimer = Time.time + fireInterval;
            currentHeat += heatPerShot;
            coolingDelayTimer = Time.time + coolingDelay;
            fireSource.Play();
            if (currentHeat > overheatThreshold)
            {
                overheated = true;
                overheatSource.Play();
            }
        }
    }

    public void RaiseShield()
    {
        shield.SetActive(true);
        shieldSource.clip = raiseShieldSFX;
        shieldSource.Play();
    }

    public void LowerShield()
    {
        if (isShieldUp)
        {
            shield.SetActive(false);
            shieldSource.clip = lowerShieldSFX;
            shieldSource.Play();
        }
    }

    public void Damaged(int damage)
    {
        health -= damage;
        damagedSFX.Play();
    }
}