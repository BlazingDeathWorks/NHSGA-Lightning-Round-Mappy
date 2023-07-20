using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Attach the Image layers in order under Canvas
public class ComicBookManager : MonoBehaviour
{
    [SerializeField] private Image[] comicLayers;
    private int index = 0;

    private void Awake()
    {
        foreach (Image layer in comicLayers)
        {
            layer.enabled = false;
        }
        NextLayer();
    }

    private void Start()
    {
        Debug.Log("did it work?");
        AudioManagerFirst.Instance.audioSource.Pause();
        InvokeRepeating("NextLayer", 2, 2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return))
        {
            NextLayer();
        }
    }

    private void NextLayer()
    {
        if (index >= comicLayers.Length)
        {
            SceneController.Instance.NextScene();
            return;
        }
        comicLayers[index].enabled = true;
        index++;
    }
}
