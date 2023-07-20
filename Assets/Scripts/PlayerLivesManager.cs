using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLivesManager : MonoBehaviour
{
    public static PlayerLivesManager Instance { get; private set; }

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
