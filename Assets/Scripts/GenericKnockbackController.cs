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

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (!Knockbackable) return;
        Rb.velocity = Direction * KnockbackSpeed;
        Knockbackable = false;
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
}