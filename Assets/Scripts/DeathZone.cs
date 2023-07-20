using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLives.Instance.CanDie = true;
            PlayerLives.Instance.LoseLife();
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Get points from the enemy and kill it
            if (collision.gameObject.TryGetComponent(out BasicEnemyAI wolfer))
            {
                EnemySpawner.Instance?.RequestNewEnemy(wolfer);
            }
            if (collision.gameObject.TryGetComponent(out Paul paul))
            {
                EnemySpawner.Instance?.RequestNewEnemy(paul: paul);
            }
        }
    }
}
