using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Has to be of tag Player or Enemy (Both of which has a FallHandler script)
        if (collision.gameObject.TryGetComponent(out FallHandler fallHandler))
        {
            //Reverse Fall Direction
            fallHandler.ReverseDirection();
        }
    }
}
