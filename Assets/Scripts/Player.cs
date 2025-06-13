using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    public int health;
    public GameMaster gameMaster;
    public float repairInterval;
    private float repairTimer;

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
    private ParticleSystem gunParticle;

    public ActionBasedController rightController;

    [Header("Grenade")]
    public int grenadeDamage;
    public int grenadeSpeed;
    public float grenadeInterval;
    private float grenadeTimer;
    public GameObject grenade;
    public Transform grenadeOrigin;
    public ActionBasedController leftController;

    [Header("Shield")]
    public GameObject shield;
    private Transform cam;
    public Transform leftControllerTransform;
    public Transform rightControllerTransform;
    public float shieldThreshold;
    private bool isShieldUp;
    private bool shieldDisabled;
    public float shieldDisableInterval;
    private float shieldDisabledTimer;
    private float playerHeight;
    public float artilleryBraceHeight = 0.6f;

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
    private Slider slider;
    public InputDevice device;
    private int totalKilled;
    public TMP_Text score;
    public TMP_Text healthText;
    public TMP_Text braceText;

    void Start()
    {
        cam = Camera.main.transform;
        playerHeight = cam.position.y;
    }

    private float HeatSliderValue() {
        return currentHeat / overheatThreshold;
    }

    public void FixedUpdate()
    {
        if (leftController.activateActionValue.action.IsPressed())
        {
            FireGrenade();
        }
        if (rightController.activateActionValue.action.IsPressed())
        {
            FireWeapon();
        }

        if (cam.position.y > playerHeight)
        {
            playerHeight = cam.position.y;
        }

        if (Time.time > coolingDelayTimer)
        {
            currentHeat -= coolingRate;
            if (currentHeat < 0)
            {
                currentHeat = 0;
            }

            if (!slider)
            {
                slider = GameObject.Find("HeatSlider").GetComponent<Slider>();
                gunParticle = GameObject.Find("GunParticle").GetComponent<ParticleSystem>();
            }
            slider.value = HeatSliderValue();
        }

        if (Time.time > repairTimer)
        {
            repairTimer = repairInterval + Time.time;
            health += 1;
            if (health > 5)
            {
                health = 5;
            }
            healthText.text = "Health: " + health.ToString();
        }

        Vector3 cameraRelative = cam.InverseTransformPoint(leftControllerTransform.position - rightControllerTransform.position);
        if (shieldDisabled)
        {
            if (shieldDisabledTimer > Time.time)
            {
                shieldDisabled = false;
            }
            else
            {
                return;
            }
        }

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
            fireTimer = Time.time + fireInterval;
            currentHeat += heatPerShot;
            coolingDelayTimer = Time.time + coolingDelay;
            fireSource.Play();
            gunParticle.Play();
            if (currentHeat >= overheatThreshold)
            {
                overheated = true;
                overheatSource.Play();
            }

            slider.value = HeatSliderValue();

            if (!Physics.Raycast(gunOrigin.position, gunOrigin.forward, out RaycastHit hit, 100))
            {
                return;
            }

            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<Enemy>().Damage(damage);
                gameMaster.deadEnemies += 1;
                totalKilled += 1;
                score.text = "Score: " + totalKilled;
            }
        }
    }

    public void FireGrenade()
    {
        // Don't allow firing grenade if shield is up
        if (isShieldUp)
        {
            return;
        }

        if (Time.time > grenadeTimer)
        {
            grenadeTimer = Time.time + grenadeInterval;
            fireSource.Play();
            GameObject newGrenade = Instantiate(grenade, grenadeOrigin.position, grenadeOrigin.rotation);
            newGrenade.GetComponent<Rigidbody>().AddForce(grenadeOrigin.forward * grenadeSpeed);
        }
    }

    public void RaiseShield()
    {
        if (!isShieldUp)
        {
            isShieldUp = true;
            shield.SetActive(true);
            shieldSource.clip = raiseShieldSFX;
            shieldSource.Play();
        }
    }

    public void LowerShield()
    {
        if (isShieldUp)
        {
            isShieldUp = false;
            shield.SetActive(false);
            shieldSource.clip = lowerShieldSFX;
            shieldSource.Play();
        }
    }

    public void Damaged(int damage)
    {
        health -= damage;
        repairTimer = Time.time + repairInterval;
        damagedSFX.Play();
        healthText.text = "Health: " + health.ToString();

        if (health <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    public void ArtilleryHit()
    {
        if (transform.position.y < playerHeight * artilleryBraceHeight && isShieldUp)
        {
            return;
        }
        else
        {
            shieldDisabled = true;
            shieldDisabledTimer = Time.time + shieldDisableInterval;
            Damaged(3);
        }
    }
}