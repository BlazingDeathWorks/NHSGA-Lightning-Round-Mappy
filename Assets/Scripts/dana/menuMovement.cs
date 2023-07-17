using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuMovement : MonoBehaviour
{
    
    [SerializeField] public float moveSpeed = 2;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > -3.6)
        {
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
