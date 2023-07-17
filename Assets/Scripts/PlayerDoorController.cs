using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoorController : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 1;
    [SerializeField] private LayerMask whatIsDoorCollision;
    private bool canActivateDoor = false;
    private RaycastHit2D hitInfo;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            canActivateDoor = true;
        }
    }

    private void FixedUpdate()
    {
        if (playerMovement == null) return;

        if (playerMovement.CanMove)
        {
            hitInfo = Physics2D.Raycast(transform.position, new Vector2(Mathf.Sign(transform.parent.localScale.x) * -1, 0), raycastDistance, whatIsDoorCollision);
        }

        //Do stuff to the door depending on its state
        if (canActivateDoor && hitInfo)
        {
            hitInfo.transform.GetComponentInParent<Door>().FlipDoorOpenState();
            canActivateDoor = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (playerMovement == null) return;
        if (!playerMovement.CanMove) return;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + new Vector2(Mathf.Sign(transform.parent.localScale.x) * -1 * raycastDistance, 0));
    }
}
