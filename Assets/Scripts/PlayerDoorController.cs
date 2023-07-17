using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoorController : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 1;
    [SerializeField] private LayerMask whatIsDoorCollision;
    private RaycastHit2D hitInfo;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        if (playerMovement.CanMove)
        {
            hitInfo = Physics2D.Raycast(transform.position, new Vector2(Mathf.Sign(transform.parent.localScale.x) * -1, 0), raycastDistance, whatIsDoorCollision);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + new Vector2(Mathf.Sign(transform.parent.localScale.x) * -1 * raycastDistance, 0));
    }
}
