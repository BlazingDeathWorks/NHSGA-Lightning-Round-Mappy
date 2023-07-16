using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float hopPower = 1.2f;
    [SerializeField] private float hopMovingSpeed = 0.2f;
    [SerializeField] private float raycastDistance = 1;
    [SerializeField] private LayerMask whatIsFloorBox;
    private float originalSpeed;
    private float x;
    private float savedX;
    private bool canMove = true;
    private bool isHopping = false;
    private bool groundedPlayerController = true;
    private Vector2 moveTowardsPos;
    private RaycastHit2D rightRaycastHit;
    private RaycastHit2D leftRaycastHit;
    private Rigidbody2D rb;
    private GroundScanner groundScanner;
    private FallHandler fallHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundScanner = GetComponentInChildren<GroundScanner>();
        fallHandler = GetComponent<FallHandler>();
        originalSpeed = speed;
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        savedX = x;
        FlipPlayer();

        //Restrict Movement if on trampoline
        if (!groundedPlayerController)
        {
            if (x == 0) return;
            if (Mathf.Sign(rb.velocity.y) == -1)
            {
                savedX = 0;
                return;
            }
            return;
        }

        //Start of your "jump"
        if (groundScanner.CurrentGroundedObject == GroundedObjectType.None && canMove == true)
        {
            canMove = false;
            isHopping = true;
            PlayerLives.Instance.CanDie = false;
        }
        else if (groundScanner.CurrentGroundedObject == GroundedObjectType.None && !canMove && Mathf.Sign(rb.velocity.y) == -1)
        {
            if (!(Vector2.Distance(transform.position, new Vector2(moveTowardsPos.x, transform.position.y)) <= 0.01f))
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(moveTowardsPos.x, transform.position.y), hopMovingSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector2(moveTowardsPos.x, transform.position.y);
                groundedPlayerController = false;
                fallHandler.StartFall();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!groundedPlayerController)
        {
            leftRaycastHit = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, whatIsFloorBox);
            rightRaycastHit = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, whatIsFloorBox);

            if (leftRaycastHit && savedX == -1)
            {
                fallHandler.StopFall();
                isHopping = true;
            }
            if (rightRaycastHit && savedX == 1)
            {
                fallHandler.StopFall();
                isHopping = true;
            }
        }

        if (canMove)
        {
            rb.velocity = new Vector2(x * speed, rb.velocity.y);
        }

        if (isHopping)
        {
            rb.velocity = new Vector2(speed * savedX / 2f, hopPower);
            isHopping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Invinsible Trampoline"))
        {
            moveTowardsPos = collision.transform.parent.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && !groundedPlayerController)
        {
            PlayerLives.Instance.CanDie = true;
            speed = originalSpeed;
            groundedPlayerController = true;
            groundScanner.ShootRaycast = true;
            canMove = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * raycastDistance);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * raycastDistance);
    }

    private void FlipPlayer()
    {
        if (x == 0 || !canMove) return;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * x, transform.localScale.y, transform.localScale.z);
    }
}
