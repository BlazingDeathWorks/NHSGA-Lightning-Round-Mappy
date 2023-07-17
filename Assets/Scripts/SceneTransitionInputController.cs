using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
