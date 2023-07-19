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
    private bool activated;

    protected virtual void Update()
    {
        if (activated)
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
            activated = false;
            OnActivate();
        }

        if (playerKnockable && StateChangedThisFrame && PreviousDoorOpenedState == false)
        {
            //Knock Player
            Debug.Log("Knock Back Player");
            playerKnockable = false;
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

    protected void ResetStateChangedThisFrame()
    {
        StateChangedThisFrame = false;
    }

    //Front - Enter
    public void ActivatePlayerKnockable(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        playerKnockable = true;
    }

    //Front - Exit
    public void DeActivatePlayerKnockable(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerKnockable = false;
        }
    }

    public void FlipDoorOpenState()
    {
        PreviousDoorOpenedState = DoorOpened;
        DoorOpened = !DoorOpened;
        activated = true;
    }

    public void Initialize()
    {
        if (DoorOpened)
        {
            OnDoorOpen();
        }
        else
        {
            OnDoorClose();
        }
    }
}
