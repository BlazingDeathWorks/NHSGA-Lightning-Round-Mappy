using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }
    [SerializeField] private float respawnTime = 3;
    private BasicEnemyAI wolfer;
    private Paul paul;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    private IEnumerator RespawnTimer()
    {
        yield return new WaitForSecondsRealtime(respawnTime);
        if (wolfer != null)
        {
            
            wolfer.transform.position = transform.position;
            wolfer.transform.localScale = new Vector3(wolfer.transform.localScale.x * -1, wolfer.transform.localScale.y, wolfer.transform.localScale.z);
            wolfer.gameObject.SetActive(true);
        }
        if (paul != null)
        {
            paul.gameObject.SetActive(true);
            paul.transform.localScale = new Vector3(paul.transform.localScale.x * -1, paul.transform.localScale.y, paul.transform.localScale.z);
            paul.transform.position = transform.position;
        }
    }

    public void RequestNewEnemy(BasicEnemyAI wolfer = null, Paul paul = null)
    {
        this.wolfer = wolfer;
        this.paul = paul;
        wolfer?.gameObject.SetActive(false);
        paul?.gameObject.SetActive(false);
        StartCoroutine(RespawnTimer());
    }
}
