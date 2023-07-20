using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
