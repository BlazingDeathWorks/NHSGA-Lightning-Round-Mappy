using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    public static PlayerLives Instance { get; private set; }
    public bool CanDie { private get; set; } = true;
    private static int s_lives = 3;
    [SerializeField] private static int count = 0;

    [SerializeField] public List<GameObject> hearts;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void CheckLives()
    {
        if (s_lives <= 0)
        {
            Debug.Log("life = 0");
            //Game Over (return in the future)
        }
        SceneController.Instance.ReloadScene();
    }

    public void LoseLife()
    {
        if (!CanDie) return;
        s_lives--;
        if (count <= 1)
        {
            //hearts[count].SetActive(false);
            Destroy(hearts[count]);
            count++;
        }
        CheckLives();
    }
}
