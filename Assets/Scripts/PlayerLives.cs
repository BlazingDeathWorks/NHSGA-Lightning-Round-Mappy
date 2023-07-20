using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    public static PlayerLives Instance { get; private set; }
    public bool CanDie { private get; set; } = true;
    private static int s_lives = 3;

    [SerializeField] public List<GameObject> hearts;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("Get called everytime");

        if (s_lives == 2)
        {
            Destroy(hearts[0]);
        }
        if (s_lives == 1)
        {
            Destroy(hearts[0]);
            Destroy(hearts[1]);
        }
    }

    private void CheckLives()
    {
        if (s_lives <= 0)
        {
            Debug.Log("life = " + s_lives);

            //Game Over (return in the future)
            SceneController.Instance.NextScene();
            PlayerLivesManager.Instance.AlreadyDied = false;
            s_lives = 3;
            Instance = null;
            //AudioManager.Instance.Play("LifeLossSound");
        }
        else
        {
            PlayerLivesManager.Instance.AlreadyDied = false;
            SceneController.Instance.ReloadScene();
            Instance = null;
        }


    }

    public void LoseLife()
    {
        if (!CanDie) return;
        if (PlayerLivesManager.Instance.AlreadyDied) return;
        s_lives--;
        PlayerLivesManager.Instance.AlreadyDied = true;
        CheckLives();
    }


}
