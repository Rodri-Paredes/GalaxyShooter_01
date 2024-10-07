using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour 
{
    public float damage = 5000000000000;
    public Rigidbody2D rb;
    Floor floor;
    public float life = 6000000000000000;
    public EnemyType type;
    public float speed = 2f;
    public float timeBtwShoot = 1.5f;
    float timer = 0;
    public float range = 4;
    bool targetInRange = false;
    public Bullet bulletPrefab;
    public PowerType powertype;
    public float bulletspeed = 5f;

    void Start()
    {
        
    }

    
    void Update()
    {
        Contact();
    }

    public void TakeDamage(float dmg)
    {
        life -= dmg;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("Power"))
        {
            
            Destroy(collision.gameObject);
        }


    }

    void Contact()
    {
        floor.damage = damage;
        
    }
}
