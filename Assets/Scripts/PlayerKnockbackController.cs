using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Should be on child of Player 
public class PlayerKnockbackController : GenericKnockbackController
{
    [SerializeField] private BoxCollider2D collisionCollider;
    [SerializeField] private CircleCollider2D attackEnemyCollider;

    protected override void Awake()
    {
        base.Awake();
        attackEnemyCollider.enabled = false;
        Rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Knockbackable)
        {
            collisionCollider.enabled = false;
            attackEnemyCollider.enabled = true;
            Rb.gravityScale = 0;
            transform.parent.transform.Translate(Direction * KnockbackSpeed * Time.deltaTime);
            Knockbackable = true;
            StartCoroutine(DisableKnockbackable());
        }
    }

    //protected override void FixedUpdate()
    //{
    //    if (Knockbackable)
    //    {
    //        collisionCollider.enabled = false;
    //        attackEnemyCollider.enabled = true;
    //        wasKnockbackableThisFrame = true;
    //        Direction *= -1;
    //    }
    //    base.FixedUpdate();
    //    if (wasKnockbackableThisFrame)
    //    {
    //        Knockbackable = true;
    //        wasKnockbackableThisFrame = false;
    //        StartCoroutine(DisableKnockbackable());
    //    }
    //}

    private IEnumerator DisableKnockbackable()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Debug.Log("After");
        Knockbackable = false;
        attackEnemyCollider.enabled = false;
        collisionCollider.enabled = true;
        Rb.gravityScale = 1;
    }
}
