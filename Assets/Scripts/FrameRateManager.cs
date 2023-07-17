using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    public static FrameRateManager Instance { get; private set; }
    [SerializeField] private int targetFrameRate = 120;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Application.targetFrameRate = targetFrameRate;
    }
}
