using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private int bounceCount;
    private SpriteRenderer sr;
    private Collider2D boxCollider;
    private Animator animator;
    private Sprite[] sprites;
    

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Trampoline_animation");
        boxCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        animator.SetBool("isTramMoving", true);
        Invoke("SetBoolBack", 0.4f);
        


        if (collision.gameObject.CompareTag("Player"))
        {
            bounceCount++;
            if (bounceCount == 1) sr.color = new Color(0.4156f, 0.6117f, 0.7019f, 1f);
            if (bounceCount == 2) sr.color = new Color(0.9622f, 0.9357f, 0.3691f, 1f);
            if (bounceCount == 3) sr.color = new Color(0.96f, 0.17f, 0.17f, 1f);
            if (bounceCount >= 4)
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
        AudioManager.Instance.Play("TrampolineSound");
    }

    private void SetBoolBack()
    {
        animator.SetBool("isTramMoving", false);
    }

    public void ResetBounceCount()
    {   
        bounceCount = 0;
        sr.color = new Color(1f, 1f, 1f, 1f);
    }
}
