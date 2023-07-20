using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//Generic knockback will be based off of trigger box's transform.right
public class GenericKnockbackController : MonoBehaviour
{
    public bool Knockbackable { get; protected set; } = false;
    protected Vector2 Direction;
    [SerializeField] [FormerlySerializedAs("knockbackSpeed")] protected float KnockbackSpeed = 1;
    protected Rigidbody2D Rb;
    protected Animator animator;

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        if (CompareTag("Player") || CompareTag("Enemy")) animator = GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        if (!Knockbackable) return;
        Rb.velocity = Vector2.zero;
        Rb.velocity = Direction * KnockbackSpeed;
        Knockbackable = false;
        if (CompareTag("Player") || CompareTag("Enemy")) 
        {
            animator.SetBool("isSplat", true);
            StartCoroutine(Unsplat());
        }
    }

    //Might need to add a parameter to flip later
    public void Knockback()
    {
        Knockbackable = true;
    }

    public void SetDirection(Vector2 dir)
    {
        Direction = dir;
    }

    private IEnumerator Unsplat()
    {
        if (CompareTag("Player")) 
        {
            yield return new WaitForSecondsRealtime(0.3f);
            animator.SetBool("isSplat", false);
        }
        else 
        {
            yield return new WaitForSecondsRealtime(2.5f);
            animator.SetBool("isSplat", false);
        }
    }
}
