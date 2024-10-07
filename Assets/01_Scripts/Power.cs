using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public PowerType type;
    public float amount; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                ApplyPowerUp(player);
                Destroy(gameObject); // Destroy power-up after applying
            }
        }
    }

    private void ApplyPowerUp(Player player)
    {
        switch (type)
        {
            case PowerType.Speed:
                player.IncreaseMovementSpeed(amount);
                break;
            case PowerType.Shield:
                player.ActivateShield(amount);
                break;
            case PowerType.MoreDamage:
                player.IncreaseDamage(amount);
                break;
            case PowerType.Shooting:
                player.IncreaseFireRate(amount);
                break;
            case PowerType.MoreVelocity:
                player.IncreaseBulletSpeed(amount);
                break;
            case PowerType.ShootSpeed:
                player.IncreaseFireRate(amount);
                break;
        }
    }
}

public enum PowerType
{
    Speed,
    ShootSpeed,
    Shooting,
    MoreVelocity,
    MoreDamage,
    Shield
}
