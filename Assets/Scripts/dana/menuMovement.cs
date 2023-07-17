using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuMovement : MonoBehaviour
{
    
    [SerializeField] public float moveSpeed = 2;
    
    void Update()
    {
        if (transform.position.x > -9.41)
        {
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
