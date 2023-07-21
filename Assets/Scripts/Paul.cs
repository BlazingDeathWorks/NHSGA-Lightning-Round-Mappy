using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class Paul : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask whatIsFloorBox;
    [SerializeField] private float raycastYOffset;
    //Find the amount of floors in the level
    [SerializeField] private GameObject floorBoxGameObject;
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
    private Collider2D previousColliderItem;
    private BonusItem bonusItem;

    private Rigidbody2D rb;
    private FallHandler fallHandler;
    private Transform playerTransform;
    private GroundScanner groundScanner;
    private Animator animator;
    private GenericKnockbackController genericKnockbackController;
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundScanner = GetComponentInChildren<GroundScanner>();
        fallHandler = GetComponent<FallHandler>();
        animator = GetComponent<Animator>();
        genericKnockbackController = GetComponent<GenericKnockbackController>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        GameObject player = HierarchyManager.Instance.GetHierarchyObject("Player");
        playerTransform = player?.transform;

        floorTransform = HierarchyManager.Instance.GetHierarchyObject("Floor 1").transform;
        floorBoxGameObject = HierarchyManager.Instance.GetHierarchyObject("Floor Box");
    }

    private void Update()
    {
        if (playerTransform == null) return;

        if (genericKnockbackController.Knockbackable)
        {
            gameObject.layer = LayerMask.NameToLayer("Air Enemy");
            rb.velocity = Vector2.zero;
            canMove = false;
            StartCoroutine(CanMoveFixPlease());
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
                targetFloor = Random.Range(0, HierarchyManager.Instance.GetHierarchyObject("Floor Box").transform.childCount);
                fallHandler.StartFall();
                animator.SetBool("isRunning", false);
                animator.SetBool("isFalling", true);
            }
        }
    }

    private IEnumerator CanMoveFixPlease()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (canMove == false && rb.velocity == Vector2.zero)
        {
            canMove = true;
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
            //animator.SetBool("isSplat", true);
            //StartCoroutine(Unsplat());
            FlipEnemy();
        }
    }

    IEnumerator Unsplat()
    {  
        yield return new WaitForSeconds(3f);
        animator.SetBool("isSplat", false);
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

            targetFloor = Random.Range(0, floorBoxGameObject.transform.childCount);
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

            targetFloor = Random.Range(0, floorBoxGameObject.transform.childCount);
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
            PlayerLives.Instance?.LoseLife();
            
        }

        //Check collision with each item tag, give 10% chance to hide behind it
        //Change paul sorting order
        //Add BonusItem script to collided item
        //Disable collider, sprite renderer, and stop moving
        //Start timer - when timer ends, remove BonusItem, resort order, re-enable stuff
        if (collision.gameObject.CompareTag("tvItem") || collision.gameObject.CompareTag("ringItem") || collision.gameObject.CompareTag("gemItem") || collision.gameObject.CompareTag("necklaceItem") || collision.gameObject.CompareTag("crownItem"))
        {
            int rand = Random.Range(1, 11);
            if (rand != 1) return;
            if (previousColliderItem == collision) return;
            previousColliderItem = collision;
            sr.sortingLayerName = "Paul Mode";
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            bonusItem = collision.gameObject.GetComponent<BonusItem>();
            bonusItem.SetPaul(this);
            bonusItem.enabled = true;
            bonusItem.canDoStuff = true;
            boxCollider.enabled = false;
            circleCollider.enabled = false;
            transform.position = collision.transform.position;
            canMove = false;
            gameObject.layer = LayerMask.NameToLayer("Air Enemy");
            animator.speed = 0;
            StartCoroutine("UnHideTimer");
        }
    }

    public void UnHide()
    {
        animator.speed = 1;
        sr.sortingLayerName = "Enemy";
        if (previousColliderItem != null)
        {
            previousColliderItem.gameObject.GetComponent<BonusItem>().enabled = false;
        }
        boxCollider.enabled = true;
        circleCollider.enabled = true;
        sr.enabled = true;
        canMove = true;
        rb.gravityScale = 1;
        bonusItem.canDoStuff = false;
        StartCoroutine("UnAirEnemy");
    }

    private IEnumerator UnAirEnemy()
    {
        yield return new WaitForSecondsRealtime(3);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        previousColliderItem = null;
        bonusItem = null;
    }

    private IEnumerator UnHideTimer()
    {
        yield return new WaitForSecondsRealtime(5f);
        UnHide();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            
            PlayerLives.Instance?.LoseLife();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLives.Instance?.LoseLife();
            AudioManager.Instance.Play("LifeLossSound");
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
