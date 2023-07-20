using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class gameOverReset : MonoBehaviour
{
    [SerializeField] private KeyCode input;

    private void Update()
    {
        if (Input.GetKeyDown(input))
        {
            SceneManager.LoadScene(0);
            AudioManagerGameOver.Instance.audioSource.Pause();
        }

        if (Input.GetKey("escape"))
        {
            AudioManagerGameOver.Instance.audioSource.Pause();
            Application.Quit();

        }

    }
}
