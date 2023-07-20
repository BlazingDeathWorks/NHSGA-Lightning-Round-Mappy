using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneTransitionInputController : MonoBehaviour
{
    [SerializeField] private KeyCode input;

    private void Update()
    {
        if (Input.GetKeyDown(input))
        {
            SceneController.Instance.NextScene();
        }

    }
}
