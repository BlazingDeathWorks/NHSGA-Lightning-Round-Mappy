using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundedObjectType
{
    Platform, None
}

//Attach this script on a child GameObject of an entity (i.e. Player, Enemy, etc.)
public class GroundScanner : MonoBehaviour
{
    public GroundedObjectType CurrentGroundedObject { get; private set; } = GroundedObjectType.Platform;
    public GroundedObjectType PreviousGroundedObject { get; private set; } = GroundedObjectType.None;
    public bool ShootRaycast { private get; set; } = true;
    [SerializeField] private float raycastDistance;
    private bool stateChanged = true;
    private RaycastHit2D raycastHit;

    private void FixedUpdate()
    {
        raycastHit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance);

        //TODO: Might have to make a guard clause here to return if tag is Enemy because Layer Matrix is wonky

        //Make CurrentGroundedObject None if we are not hitting anything (we are in air)
        if (!raycastHit)
        {
            PreviousGroundedObject = CurrentGroundedObject;
            CurrentGroundedObject = GroundedObjectType.None;
            stateChanged = false;
            return;
        }

        if (stateChanged) return;

        if (raycastHit.transform.CompareTag("Platform"))
        {
            PreviousGroundedObject = CurrentGroundedObject;
            CurrentGroundedObject = GroundedObjectType.Platform;
            stateChanged = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (CurrentGroundedObject == GroundedObjectType.None)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * raycastDistance);
    }

    public void Reset()
    {
        CurrentGroundedObject = GroundedObjectType.Platform;
        PreviousGroundedObject = GroundedObjectType.None;
        stateChanged = true;
    }

    public void SetCurrentGroundedNone()
    {
        CurrentGroundedObject = GroundedObjectType.None;
    }
}
