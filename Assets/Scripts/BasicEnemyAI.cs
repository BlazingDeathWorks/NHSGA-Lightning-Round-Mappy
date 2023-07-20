using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Need to update this later to go other way when colliding with door
public class BasicEnemyAI : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask whatIsFloorBox;
    [SerializeField] private float raycastYOffset;
    private int targetFloor;
    private int floorCount;
    private bool canMove = true;
    private bool isHopping = false;
    private bool canShootRaycast = false;
    private bool raycastThroughFloor = false;
    private bool hopNextFloor = false;
    private bool leftPlatform = false;
    private Vector2 moveTowardsPos;
    private Transform floorTransform;
    private RaycastHit2D raycastHit;

    private Rigidbody2D rb;
    private FallHandler fallHandler;
    private FloorManager playerFloorManager;
    private Transform playerTransform;
    private GroundScanner groundScanner;
    private Animator animator;
    private GenericKnockbackController genericKnockbackController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundScanner = GetComponentInChildren<GroundScanner>();
        fallHandler = GetComponent<FallHandler>();
        animator = GetComponent<Animator>();
        genericKnockbackController = GetComponent<GenericKnockbackController>();
    }

    private void Start()
    {
        GameObject player = HierarchyManager.Instance.GetHierarchyObject("Player");
        playerTransform = player?.transform;
        playerFloorManager = player?.GetComponent<FloorManager>();

        floorTransform = HierarchyManager.Instance.GetHierarchyObject("Floor 1").transform;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        if (genericKnockbackController.Knockbackable)
        {
            gameObject.layer = LayerMask.NameToLayer("Air Enemy");
            rb.velocity = Vector2.zero;
            canMove = false;
            return;
        }

        if (targetFloor == floorCount && canShootRaycast)
        {
            hopNextFloor = true;
            animator.SetBool("isFalling", false);
            animator.SetBool("isRunning", true);
        }

        //Start of your "jump"
        if (groundScanner.CurrentGroundedObject == GroundedObjectType.None && canMove == true && !leftPlatform)
        {
            canMove = false;
            isHopping = true;
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
                targetFloor = playerFloorManager.Floor;
                fallHandler.StartFall();
                animator.SetBool("isRunning", false);
                animator.SetBool("isFalling", true);
            }
        }
    }

    private void FixedUpdate()
    { 
        if (canShootRaycast)
        {
            raycastHit = Physics2D.Raycast((Vector2)transform.position + new Vector2(0, raycastYOffset), Vector2.right * transform.localScale.x, raycastDistance, whatIsFloorBox);

            if (!raycastHit && !hopNextFloor)
            {
                raycastThroughFloor = false;
            }
            if (raycastHit && !raycastThroughFloor && !hopNextFloor)
            {
                floorCount++;
                raycastThroughFloor = true;
            }
            if (hopNextFloor)
            {
                hopNextFloor = false;
                canShootRaycast = false;
                fallHandler.StopFall();
                isHopping = true;
            }
        }

        if (canMove && !genericKnockbackController.Knockbackable)
        {
            rb.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * speed, rb.velocity.y);
        }

        if (isHopping)
        {
            rb.velocity = new Vector2(speed * Mathf.Sign(transform.localScale.x) / 2f, HopDataManager.Instance.HopPower + 0.5f);
            isHopping = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            leftPlatform = false;
            groundScanner.ShootRaycast = true;
            raycastThroughFloor = false;
            canMove = true;
            groundScanner.Reset();
            canShootRaycast = false;
        }

        if (collision.gameObject.CompareTag("Attack Door"))
        {
            FlipEnemy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trampoline"))
        {
            float distanceX = playerTransform.position.x - transform.position.x;
            if (distanceX != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(distanceX) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Sign(floorTransform.position.x - transform.position.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            
            targetFloor = playerFloorManager.Floor;
            floorCount = 0;
            canShootRaycast = true;
        }

        if (collision.gameObject.CompareTag("Trampoline Rebounder"))
        {
            float distanceX = playerTransform.position.x - transform.position.x;
            if (distanceX != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(distanceX) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Sign(floorTransform.position.x - transform.position.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            targetFloor = playerFloorManager.Floor;
            floorCount = 0;
            canShootRaycast = false;
        }

        if (collision.gameObject.CompareTag("Invinsible Trampoline"))
        {
            gameObject.layer = LayerMask.NameToLayer("Air Enemy");
            moveTowardsPos = collision.transform.parent.position;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLives.Instance.LoseLife();
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Invinsible Trampoline"))
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
    }

    private void OnDrawGizmos()
    {
        if (canShootRaycast)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.white;
        }
        Gizmos.DrawLine((Vector2)transform.position + new Vector2(0, raycastYOffset), (Vector2)transform.position + new Vector2(0, raycastYOffset) + Vector2.right * Mathf.Sign(transform.localScale.x) * raycastDistance);
    }

    //Use this when we get to door interactions
    private void FlipEnemy()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
