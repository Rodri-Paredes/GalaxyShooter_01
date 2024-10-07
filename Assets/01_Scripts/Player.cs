using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject particle;
    public Image lifebar;
    public float maxlife = 3;
    float life = 3;
    public float speed = 2f;
    public float timeBtwShoot = 1.5f;
    public int bullets = 5;
    int currentBullets;
    float timer = 0;
    bool canShoot = true;
    public Rigidbody2D rb;
    public Transform firePoint;
    public Bullet bulletPrefab;
    public float damage = 1f;
    public float bulletspeed = 5f;

    // New variables for power-ups
    private float extraDamage = 0;
    private float extraSpeed = 0;
    private float bulletSpeedMultiplier = 1;
    private float fireRateMultiplier = 1;
    private bool hasShield = false;
    private float shieldDuration = 0;
    private float shieldEndTime = 0;
    public Text lifetext;

    void Start()
    {
        Debug.Log("Inició el juego");
        currentBullets = bullets;
        lifetext.text = "Life =" + life;
        life = maxlife;
        lifebar.fillAmount = life / maxlife;
    }

    void Update()
    {
        if (hasShield && Time.time > shieldEndTime)
        {
            DeactivateShield();
        }

        Debug.Log("Juego en progreso");
        Movement();
        Reload();
        CheckIfShoot();
        Shoot();
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(x, y) * (speed + extraSpeed);
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canShoot && currentBullets > 0)
        {
            Bullet b = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            b.damage = damage + extraDamage;
            b.speed = bulletspeed * bulletSpeedMultiplier;
            canShoot = false;
            currentBullets--;
        }
    }

    void Reload()
    {
        if (currentBullets == 0 && Input.GetKeyDown(KeyCode.R))
        {
            currentBullets = bullets;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (hasShield) return; 

        life -= dmg;
        lifetext.text = "Life =" + life;
        lifebar.fillAmount = life / maxlife;
        if (life <= 0)
        {

            SceneManager.LoadScene("Game");

        }
        else 
        {
            
        }
        


    }


    void CheckIfShoot()
    {
        if (!canShoot)
        {
            if (timer < timeBtwShoot / fireRateMultiplier)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                canShoot = true;
            }
        }
    }

    public void IncreaseMovementSpeed(float amount)
    {
        extraSpeed = amount;
    }

    public void IncreaseDamage(float amount)
    {
        extraDamage = amount;
    }

    public void IncreaseBulletSpeed(float amount)
    {
        bulletSpeedMultiplier = amount;
    }

    public void IncreaseFireRate(float amount)
    {
        fireRateMultiplier = amount;
    }

    public void ActivateShield(float duration)
    {
        hasShield = true;
        shieldDuration = duration;
        shieldEndTime = Time.time + duration;
        
    }

    private void DeactivateShield()
    {
        hasShield = false;
        
    }
}

