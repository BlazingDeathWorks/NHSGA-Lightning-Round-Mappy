using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    public static PlayerLives Instance { get; private set; }
    public bool CanDie { private get; set; } = true;
    private static int s_lives = 3;

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
            //Game Over
            return;
        }
        SceneController.Instance.ReloadScene();
    }

    public void LoseLife()
    {
        if (!CanDie) return;
        s_lives--;
        CheckLives();
    }
}
