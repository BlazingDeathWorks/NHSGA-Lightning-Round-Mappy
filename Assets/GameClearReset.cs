using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameClearReset : MonoBehaviour
{
    [SerializeField] private KeyCode input;

    private void Update()
    {
        if (Input.GetKeyDown(input))
        {
            SceneManager.LoadScene(0);
            AudioManagerGameClear.Instance.audioSource.Pause();
        }

        if (Input.GetKey("escape"))
        {
            AudioManagerGameClear.Instance.audioSource.Pause();
            Application.Quit();
        }
    }
}