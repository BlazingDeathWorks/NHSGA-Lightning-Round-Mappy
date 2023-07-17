using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopDataManager : MonoBehaviour
{
    public static HopDataManager Instance { get; private set; }
    public float HopPower => hopPower;
    public float HopMovingSpeed => hopMovingSpeed;
    [SerializeField] private float hopPower;
    [SerializeField] private float hopMovingSpeed;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
