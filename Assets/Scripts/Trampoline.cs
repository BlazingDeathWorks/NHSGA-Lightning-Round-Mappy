using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private int bounceCount;
    private SpriteRenderer sr;
    private Collider2D boxCollider;
    private Animator animator;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        animator.SetBool("isTramMoving", true);
        Invoke("SetBoolBack", 0.4f);


        if (collision.gameObject.CompareTag("Player"))
        {
            

            if (++bounceCount >= 4)
            {
                //"Destroy" Trampoline but keep it's children
                sr.enabled = false;
                boxCollider.enabled = false;
                return;
            }
        }
        //Has to be of tag Player or Enemy (Both of which has a FallHandler script)
        if (collision.gameObject.TryGetComponent(out FallHandler fallHandler))
        {
            //Reverse Fall Direction
            fallHandler.ReverseDirection();
        }
    }

    private void SetBoolBack()
    {
        animator.SetBool("isTramMoving", false);
    }

    public void ResetBounceCount()
    {
        bounceCount = 0;
    }
}
