using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool CanMove => canMove;
    [SerializeField] private float speed = 1;
    [SerializeField] private float raycastDistance = 1;
    [SerializeField] private LayerMask whatIsFloorBox;
    [SerializeField] private float raycastYOffset;
    private float x;
    private float savedX;
    private float savedInX;
    private bool canMove = true;
    private bool isHopping = false;
    private bool groundedPlayerController = true;
    private bool leftPlatform;
    private bool hoppingIn = false;
    private Vector2 moveTowardsPos;
    private RaycastHit2D rightRaycastHit;
    private RaycastHit2D leftRaycastHit;
    private Rigidbody2D rb;
    private GroundScanner groundScanner;
    private FallHandler fallHandler;
    private Animator animator;
    private GenericKnockbackController genericKnockbackController;
    private PlayerKnockbackController playerKnockbackController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundScanner = GetComponentInChildren<GroundScanner>();
        fallHandler = GetComponent<FallHandler>();
        animator = GetComponent<Animator>();
        genericKnockbackController = GetComponent<GenericKnockbackController>();
        playerKnockbackController = GetComponentInChildren<PlayerKnockbackController>();
    }

    private void Update()
    {
        if (animator.GetBool("isDying")) // Disables movement when dead
        {
            rb.gravityScale = 0;
            rb.simulated = false;
            return; 
        }

        x = Input.GetAxisRaw("Horizontal");
        savedX = x;
        FlipPlayer();

        //Restrict Movement if on trampoline
        if (!groundedPlayerController)
        {
            if (x != 0 && Mathf.Sign(rb.velocity.y) == -1)
            {
                savedX = 0;
            }
        }
        else
        {
            if (x == 0)
            {
                animator.SetBool("isRunning", false);
            }
            else
            {
                animator.SetBool("isRunning", true);
            }
        }

        //Start of your "jump"
        if (groundScanner.CurrentGroundedObject == GroundedObjectType.None && canMove == true && !leftPlatform)
        {
            x = 0;
            savedInX = savedX;
            savedX = 0;
            groundScanner.ShootRaycast = false;
            canMove = false;
            isHopping = true;
            hoppingIn = true;
            PlayerLives.Instance.CanDie = false;
        }
        else if (groundScanner.CurrentGroundedObject == GroundedObjectType.None && !canMove && Mathf.Sign(rb.velocity.y) == -1 && !leftPlatform)
        {
            if (!(Vector2.Distance(transform.position, new Vector2(moveTowardsPos.x, transform.position.y)) <= 0.01f))
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(moveTowardsPos.x, transform.position.y), HopDataManager.Instance.HopMovingSpeed * Time.deltaTime);
            }
            else
            {
                leftPlatform = true;
                transform.position = new Vector2(moveTowardsPos.x, transform.position.y);
                groundedPlayerController = false;
                fallHandler.StartFall();
                animator.SetBool("isRunning", false);
                animator.SetBool("isFalling", true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!groundedPlayerController)
        {
            leftRaycastHit = Physics2D.Raycast((Vector2)transform.position + new Vector2(0, raycastYOffset), Vector2.left, raycastDistance, whatIsFloorBox);
            rightRaycastHit = Physics2D.Raycast((Vector2)transform.position + new Vector2(0, raycastYOffset), Vector2.right, raycastDistance, whatIsFloorBox);

            if (leftRaycastHit && savedX == -1)
            {
                savedInX = savedX;
                fallHandler.StopFall();
                animator.SetBool("isFalling", false);
                hoppingIn = false;
                isHopping = true;
            }
            if (rightRaycastHit && savedX == 1)
            {
                savedInX = savedX;
                fallHandler.StopFall();
                animator.SetBool("isFalling", false);
                hoppingIn = false;
                isHopping = true;
            }
        }

        if (canMove && !genericKnockbackController.Knockbackable && !playerKnockbackController.Knockbackable)
        {
            rb.velocity = new Vector2(x * speed, rb.velocity.y);
        }

        if (isHopping)
        {
            if (savedX == 0)
            {
                savedX = Mathf.Sign(moveTowardsPos.x - transform.position.x);
            }
            if (savedInX == 0)
            {
                savedInX = Mathf.Sign(moveTowardsPos.x - transform.position.x);
            }
            //If groundController true then savedX becomes a special calculated value between Mathf.Sign(box.pos.x - pos.x) to guarantee direction
            if (groundedPlayerController) savedX = Mathf.Sign(moveTowardsPos.x - transform.position.x);
            if (hoppingIn)
            {
                savedX = Mathf.Sign(moveTowardsPos.x - transform.position.x);
                rb.velocity = new Vector2(speed * savedX / 2f, HopDataManager.Instance.HopPower);
            }
            else
            {
                rb.velocity = new Vector2(speed * savedInX / 2f, HopDataManager.Instance.HopPower);
            }
            isHopping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Invinsible Trampoline"))
        {
            StopCoroutine("ResetPlayerLayer");
            gameObject.layer = LayerMask.NameToLayer("Air Enemy");
            moveTowardsPos = collision.transform.parent.position;
            groundScanner.SetCurrentGroundedNone();
        }

        if (collision.gameObject.CompareTag("Attack Door"))
        {
            animator.SetBool("isSplat", true);
            StartCoroutine(Unsplat());
        }
    }

    IEnumerator Unsplat()
    {  
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isSplat", false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Invinsible Trampoline"))
        {
            StartCoroutine("ResetPlayerLayer");
            if (rb.velocity == new Vector2(x * speed, rb.velocity.y))
            {
                PlayerLives.Instance.CanDie = true;
            }
        }
    }

    private IEnumerator ResetPlayerLayer()
    {
        yield return new WaitForSecondsRealtime(1);
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && !groundedPlayerController)
        {
            PlayerLives.Instance.CanDie = true;
            groundScanner.ShootRaycast = true;
            leftPlatform = false;
            groundedPlayerController = true;
            canMove = true;
            groundScanner.Reset();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine((Vector2)transform.position + new Vector2(0, raycastYOffset), (Vector2)transform.position + new Vector2(0, raycastYOffset) + Vector2.left * raycastDistance);
        Gizmos.DrawLine((Vector2)transform.position + new Vector2(0, raycastYOffset), (Vector2)transform.position + new Vector2(0, raycastYOffset) + Vector2.right * raycastDistance);
    }

    private void FlipPlayer()
    {
        if (x == 0 || isHopping) return;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void ResetHopCapabilities()
    {
        rb.velocity = new Vector2(rb.velocity.x, -0.1f);
        leftPlatform = false;
    }
}
