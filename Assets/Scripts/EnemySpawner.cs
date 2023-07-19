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

    private IEnumerator RespawnTimer(BasicEnemyAI wolfer = null, Paul paul = null)
    {
        yield return new WaitForSecondsRealtime(respawnTime);
        if (wolfer != null)
        {
            wolfer.gameObject.SetActive(true);
            //rotate or flip
            //
        }
    }

    public void RequestNewEnemy(BasicEnemyAI wolfer = null, Paul paul = null)
    {
        StartCoroutine(RespawnTimer());
    }
}
