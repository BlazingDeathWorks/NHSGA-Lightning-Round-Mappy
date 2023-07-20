using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularDoor : Door
{
    private bool speedable = false;
    private bool enemyKnockable;
    private bool playerKnockableBack;
    private PlayerKnockbackController playerKnockbackController;
    [SerializeField] private GameObject speedUpTransform;

    protected override void Update()
    {
        base.Update();

        if (speedable && StateChangedThisFrame && PreviousDoorOpenedState == true)
        {
            //Speed Up Player
            Debug.Log("Speed Up Player");
            speedable = false;
            if (playerKnockbackController != null)
            {
                //We don't need to use the usual knockback function
                playerKnockbackController.Knockback();
            }
        }

        if (enemyKnockable && StateChangedThisFrame && PreviousDoorOpenedState == true)
        {
            //Knockback Enemy
            enemyKnockable = false;
            KnockbackAll();
        }

        if (playerKnockableBack && StateChangedThisFrame && PreviousDoorOpenedState == true)
        {
            //Knockback Player Back Door
            playerKnockableBack = false;
            KnockbackAll();
        }

        //THIS NEEDS TO BE AT THE END OF UPDATE
        ResetStateChangedThisFrame();
    }

    //Front - Enter
    public void KnockEnemyInFront(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && DoorOpened == false)
        {
            //Knock back enemy
            Debug.Log("KNOCK KNOCK");
            RegisterKnockbackController(collision);
            KnockbackAll();
            RemoveKnockbackController(collision);
        }
    }

    //Back - Enter
    public void EnemyOpenInBack(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && DoorOpened == false)
        {
            //Enemy Open In Back
            FlipDoorOpenState();
        }
    }

    //Speedy Front - Enter
    public void ActivateSpeedable(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            speedable = true;
            PlayerKnockbackController controller = collision.GetComponentInChildren<PlayerKnockbackController>();
            controller.SetDirection(speedUpTransform.transform.right);
            playerKnockbackController = controller;
        }
    }

    //Speedy Front - Exit
    public void DeActivateSpeedable(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            speedable = false;
            playerKnockbackController = null;
        }
    }

    //Back - Enter
    public void ActivateEnemyKnockable(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && DoorOpened == true)
        {
            enemyKnockable = true;
            RegisterKnockbackController(collision, EnemyBack.transform.right);
        }
    }

    //Back - Exit
    public void DeActivateEnemyKnockable(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        { 
            enemyKnockable = false;
            RemoveKnockbackController(collision);
        }
    }

    //Back - Enter
    public void ActivatePlayerKnockableBack(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && DoorOpened == true)
        {
            playerKnockableBack = true;
            RegisterKnockbackController(collision, EnemyBack.transform.right);
        }
    }

    //Back - Exit
    public void DeActivatePlayerKnockableBack(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerKnockableBack = false;
            RemoveKnockbackController(collision);
        }
    }
}
