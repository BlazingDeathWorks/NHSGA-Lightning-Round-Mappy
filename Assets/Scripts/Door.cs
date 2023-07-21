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
    private List<GenericKnockbackController> genericKnockbackControllers = new List<GenericKnockbackController>();

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
        }

        if (playerKnockable && StateChangedThisFrame && PreviousDoorOpenedState == false)
        {
            //Knock Back Player
            KnockbackAll();
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

    protected void ResetStateChangedThisFrame()
    {
        StateChangedThisFrame = false;
    }

    protected void KnockbackAll()
    {
        for (int i = 0; i < genericKnockbackControllers.Count; i++)
        {
            genericKnockbackControllers[i].Knockback();
        }
    }

    protected void RegisterKnockbackController(Collider2D collision, Vector2? dir = null)
    {
        if (dir == null) dir = FrontKnockBack.transform.right;
        GenericKnockbackController controller = collision.GetComponent<GenericKnockbackController>();
        for (int i = 0; i < genericKnockbackControllers.Count; i++)
        {
            if (genericKnockbackControllers[i] == controller) return;
        }
        controller.SetDirection((Vector2)dir);
        genericKnockbackControllers.Add(controller);
    }

    protected void RemoveKnockbackController(Collider2D collision)
    {
        genericKnockbackControllers.Remove(collision.GetComponent<GenericKnockbackController>());
    }

    //Front - Enter
    public void ActivatePlayerKnockable(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        playerKnockable = true;

        RegisterKnockbackController(collision);
    }

    //Front - Exit
    public void DeActivatePlayerKnockable(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerKnockable = false;

            RemoveKnockbackController(collision);
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
