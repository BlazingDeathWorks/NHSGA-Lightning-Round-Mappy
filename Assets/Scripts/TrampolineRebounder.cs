using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineRebounder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Has to be of tag Player or Enemy (Both of which has a FallHandler script)
        if (collision.TryGetComponent(out FallHandler fallHandler))
        {
            //Reverse Fall Direction
            fallHandler.ReverseDirection();
        }
    }
}
