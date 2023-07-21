using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }
    [SerializeField] private float respawnTime = 3;
    [SerializeField] private BasicEnemyAI wolferPrefab;
    [SerializeField] private Paul paulPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    private IEnumerator RespawnTimer(BasicEnemyAI wolfer, Paul paul)
    {
        yield return new WaitForSecondsRealtime(respawnTime);
        if (wolfer != null)
        {
            wolfer = Instantiate(wolferPrefab);
            wolfer.gameObject.SetActive(true);
            wolfer.transform.position = transform.position;
            wolfer.transform.localScale = new Vector3(wolfer.transform.localScale.x * -1, wolfer.transform.localScale.y, wolfer.transform.localScale.z);
        }
        if (paul != null)
        {
            paul = Instantiate(paulPrefab);
            paul.gameObject.SetActive(true);
            paul.transform.localScale = new Vector3(paul.transform.localScale.x * -1, paul.transform.localScale.y, paul.transform.localScale.z);
            paul.transform.position = transform.position;
        }
    }

    public void RequestNewEnemy(BasicEnemyAI wolfer = null, Paul paul = null)
    {
        wolfer?.gameObject.SetActive(false);
        paul?.gameObject.SetActive(false);
        StartCoroutine(RespawnTimer(wolfer, paul));
    }
}
