using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallHandler : MonoBehaviour
{
    public bool CanFall { get; private set; } = false;
    public float FallingSpeed { get; private set; }
    [SerializeField] private float fallingSpeed = 1;
    private GroundScanner groundScanner;
    private Rigidbody2D rb;

    private void Awake()
    {
        groundScanner = GetComponentInChildren<GroundScanner>();
        rb = GetComponent<Rigidbody2D>();

        //Falling speed should be a negative number at the start of play
        fallingSpeed *= -1;
    }

    private void Update()
    {
        if (groundScanner.PreviousGroundedObject == GroundedObjectType.Platform && groundScanner.CurrentGroundedObject == GroundedObjectType.Platform)
        {
            Debug.Log("YOU SHOULD DIE");
        }
    }

    private void FixedUpdate()
    {
        if (CanFall)
        {
            rb.velocity = new Vector2(0, fallingSpeed);
        }
    }

    public void StartFall()
    {
        CanFall = true;
        //Set Gravity to zero
        rb.gravityScale = 0;
        //Disable Ground Raycast (this part is a bit flawed rn because we might still need raycast to detect for collision with trampoline
        groundScanner.ShootRaycast = false;
        groundScanner.Reset();
    }

    public void ReverseDirection()
    {
        fallingSpeed *= -1;
    }

    public void StopFall()
    {
        CanFall = false;
        rb.gravityScale = 1;
        rb.velocity = Vector2.zero;
        fallingSpeed = Mathf.Abs(fallingSpeed) * -1;
    }
}
