using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    public static PlayerLives Instance { get; private set; }
    private int lives = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = null;
    }

    private void CheckLives()
    {
        if (lives <= 0)
        {
            //Game Over
        }
    }

    public void LoseLife()
    {
        lives--;
        CheckLives();
    }
}
