using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Door : MonoBehaviour
{
    [SerializeField] protected bool DoorOpened = false;
    [SerializeField] [field: FormerlySerializedAs("doorClosedCollision")] protected GameObject DoorClosedCollision;
    [SerializeField] [field: FormerlySerializedAs("enemyBack")] protected GameObject EnemyBack;
    [SerializeField] [field: FormerlySerializedAs("frontKnockBack")] protected GameObject FrontKnockBack;
    [SerializeField] [field: FormerlySerializedAs("regularDoorOpened")] protected GameObject RegularDoorOpened;
    private bool previousDoorOpenedState = false;

    protected virtual void Update()
    {
        if (DoorOpened != previousDoorOpenedState)
        {
            if (DoorOpened)
            {
                OnDoorOpen();
            }
            else
            {
                OnDoorClose();
            }
            previousDoorOpenedState = DoorOpened;
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

    public void FlipDoorOpenState()
    {
        DoorOpened = !DoorOpened;
    }
}
