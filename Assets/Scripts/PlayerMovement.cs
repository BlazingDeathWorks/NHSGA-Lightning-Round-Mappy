using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float hopPower = 1.2f;
    [SerializeField] private float hopMovingSpeed = 0.2f;
    private float x;
    private bool canMove = true;
    private bool isHopping = false;
    private bool groundedPlayerController = true;
    private float moveTowardsX;
    private Rigidbody2D rb;
    private GroundScanner groundScanner;
    private FallHandler fallHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundScanner = GetComponentInChildren<GroundScanner>();
        fallHandler = GetComponent<FallHandler>();
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");

        //Have different controls when bouncing on trampoline
        if (!groundedPlayerController)
        {
            return;
        }

        if (groundScanner.CurrentGroundedObject == GroundedObjectType.None && canMove == true)
        {
            canMove = false;
            isHopping = true;
        }
        else if (groundScanner.CurrentGroundedObject == GroundedObjectType.None && !canMove && Mathf.Sign(rb.velocity.y) == -1)
        {
            if (!(Vector2.Distance(transform.position, new Vector2(moveTowardsX, transform.position.y)) <= 0.1f))
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(moveTowardsX, transform.position.y), hopMovingSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector2(moveTowardsX, transform.position.y);
                groundedPlayerController = false;
                fallHandler.StartFall();
            }
        }


        if (x == 0 || !canMove) return;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * x, transform.localScale.y, transform.localScale.z);
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(x * speed, rb.velocity.y);
        }

        if (isHopping)
        {
            rb.velocity = new Vector2(speed * x / 3.5f, hopPower);
            isHopping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Invinsible Trampoline"))
        {
            moveTowardsX = collision.transform.parent.position.x;
        }
    }
}
