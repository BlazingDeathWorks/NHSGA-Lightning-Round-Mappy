using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private float x;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");

        if (x == 0) return;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * x, transform.localScale.y, transform.localScale.z);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(x, rb.velocity.y) * speed;
    }
}
