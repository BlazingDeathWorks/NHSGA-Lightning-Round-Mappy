using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Door : MonoBehaviour
{
    public bool DoorOpened = false;
    [SerializeField] [field: FormerlySerializedAs("doorClosedCollision")] protected GameObject DoorClosedCollision;
    [SerializeField] [field: FormerlySerializedAs("enemyBack")] protected GameObject EnemyBack;
    [SerializeField] [field: FormerlySerializedAs("frontKnockBack")] protected GameObject FrontKnockBack;
    [SerializeField] [field: FormerlySerializedAs("regularDoorOpened")] protected GameObject RegularDoorOpened;
    protected bool StateChangedThisFrame { get; private set; } = false;
    protected bool PreviousDoorOpenedState { get; private set; } = false;
    private bool playerKnockable;

    protected virtual void Update()
    {
        StateChangedThisFrame = false;
        if (DoorOpened != PreviousDoorOpenedState)
        {
            StateChangedThisFrame = true;
            if (DoorOpened)
            {
                OnDoorOpen();
            }
            else
            {
                OnDoorClose();
            }
            PreviousDoorOpenedState = DoorOpened;
        }

        if (playerKnockable && StateChangedThisFrame && DoorOpened == false)
        {
            //Knock Player
            Debug.Log("Knock Back Player");
        }
    }

    protected virtual void OnDoorOpen()
    {
        DoorClosedCollision.SetActive(false);
        RegularDoorOpened.SetActive(true);
    }

    protected virtual void OnDoorClose()
    {
        DoorClosedCollision.SetActive(true);
        RegularDoorOpened.SetActive(false);
    }

    protected virtual void OnActivate()
    {

    }

    //Front - Enter and Exit
    public void ActivatePlayerKnockable(Collider2D collision)
    {
        playerKnockable = !playerKnockable;
    }

    public void FlipDoorOpenState()
    {
        OnActivate();
        DoorOpened = !DoorOpened;
    }
}
