using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallHandler : MonoBehaviour
{
    [SerializeField] private float fallingSpeed = 1;
    private bool canFall = false;
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
        if (groundScanner.CurrentGroundedObject == GroundedObjectType.None) return;
    }

    private void FixedUpdate()
    {
        if (canFall)
        {
            rb.velocity = new Vector2(0, fallingSpeed);
        }
    }

    public void StartFall()
    {
        canFall = true;
        //Set Gravity to zero
        rb.gravityScale = 0;
        //Disable Ground Raycast (this part is a bit flawed rn because we might still need raycast to detect for collision with trampoline
        groundScanner.ShootRaycast = false;
    }

    public void ReverseDirection()
    {
        fallingSpeed *= -1;
    }

    public void StopFall()
    {
        canFall = false;
        fallingSpeed = Mathf.Abs(fallingSpeed);
    }
}
