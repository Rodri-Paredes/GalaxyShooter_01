using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject particle;
    public Image LifeBar;
    public GameObject[] powerUpPrefabs;
    public EnemyType type;
    public float maxlife = 5;
    private float life;
    public int scorePoints = 1;
    public float damage = 1;
    public float speed = 2f;
    public float timeBtwShoot = 1.5f;
    private float timer = 0;
    public float range = 4;
    private bool targetInRange = false;
    private Transform target;
    public Transform firePoint;
    public Bullet bulletPrefab;
    public float bulletspeed = 5f;

    [Range(0f, 1f)]
    public float powerUpDropChance = 0.3f;

    void Start()
    {
        GameObject ship = GameObject.FindGameObjectWithTag("Player");
        if (ship != null)
        {
            target = ship.transform;
        }
        life = maxlife;
        LifeBar.fillAmount = life / maxlife;
    }

    void Update()
    {
        switch (type)
        {
            case EnemyType.Normal:
                MoveForward();
                break;
            case EnemyType.NormalShoot:
                MoveForward();
                Shoot();
                break;
            case EnemyType.Kamikase:
                if (targetInRange)
                {
                    RotateToTarget();
                    MoveForward(2);
                }
                else
                {
                    MoveForward();
                    SearchTarget();
                }
                break;
            case EnemyType.Sniper:
                if (targetInRange)
                {
                    RotateToTarget();
                    Shoot();
                }
                else
                {
                    MoveForward();
                    SearchTarget();
                }
                break;
            case EnemyType.Radio:
                MoveForward();
                break;
            case EnemyType.Boss:
                BossBehavior();
                break;
        }
    }

    public void TakeDamage(float dmg)
    {
        life -= dmg;
        LifeBar.fillAmount = life / maxlife;
        if (life <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        // Instantiate particle effect
        if (particle != null)
        {
            GameObject particleEffect = Instantiate(particle, transform.position, Quaternion.identity);
            ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Destroy(particleEffect, ps.main.duration);
            }
            else
            {
                Debug.LogWarning("Particle System component not found on prefab!");
                Destroy(particleEffect);
            }
        }
        else
        {
            Debug.LogWarning("Particle prefab is not assigned!");
        }

        DropPowerUp();
        Spawner.instance.AddScore(scorePoints);
        Spawner.instance.OnEnemyDefeated();
        Destroy(gameObject);
    }

    void MoveForward()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void MoveForward(float m)
    {
        transform.Translate(Vector2.up * speed * m * Time.deltaTime);
    }

    void RotateToTarget()
    {
        Vector2 dir = target.position - transform.position;
        float angleZ = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -angleZ);
    }

    void SearchTarget()
    {
        float distance = Vector2.Distance(target.position, transform.position);
        targetInRange = distance <= range;
    }
    void ShootIrregularly()
    {
        int numberOfShots = 5;
        float spreadAngle = 10f;

        for (int i = 0; i < numberOfShots; i++)
        {
            Vector2 baseDirection = (target.position - firePoint.position).normalized;
            float randomAngle = Random.Range(-spreadAngle, spreadAngle);
            Vector2 shootDirection = Quaternion.Euler(0, 0, randomAngle) * baseDirection;

            Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(Vector3.forward, shootDirection));
            bullet.speed = bulletspeed;
            bullet.damage = damage;
        }
    }


    void Shoot()
    {
        if (timer < timeBtwShoot)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            Bullet b = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            b.damage = damage;
            b.speed = bulletspeed;
        }
    }

    void DropPowerUp()
    {
        if (powerUpPrefabs.Length == 0) return;

        float dropChance = powerUpDropChance; 

        if (Random.value <= dropChance)
        {
            int powerUpIndex = Random.Range(0, powerUpPrefabs.Length);
            Instantiate(powerUpPrefabs[powerUpIndex], transform.position, Quaternion.identity);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    void BossBehavior()
    {
        if (life > maxlife * 0.75f)
        {
            Phase1();
        }
        else if (life > maxlife * 0.5f)
        {
            Phase2();
        }
        else if (life > maxlife * 0.25f)
        {
            Phase3();
        }
        else
        {
            Phase4();
        }
    }

    void Phase1()
    {
        Debug.Log("Primera fase");
        RotateToTarget();
        MoveForward(2);
    }

    void Phase2()
    {
        Debug.Log("Segunda fase");

        speed = 3f;
        timeBtwShoot = 1.0f;
        RotateToTarget();
        Shoot();
    }

    void Phase3()
    {
        Debug.Log("Tercera fase");

        ShootRadio();
        RotateToTarget();
        MoveForward();
    }

    void Phase4()
    {
        Debug.Log("Cuarta fase");

        RotateToTarget();
        ShootIrregularly();
    }
    void ShootRadio()
    {
        if (timer < timeBtwShoot)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            int numberOfShots = 8;
            for (int i = 0; i < numberOfShots; i++)
            {
                float angle = i * (360f / numberOfShots);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Instantiate(bulletPrefab, firePoint.position, rotation);
            }
        }
    }


}

public enum EnemyType
{
    Normal,
    NormalShoot,
    Kamikase,
    Sniper,
    Radio,
    ShotsFive,
    Boss
}
