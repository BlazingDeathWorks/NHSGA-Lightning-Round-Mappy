using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularDoor : Door
{
    private bool speedable = false;
    private bool enemyKnockable;
    private bool playerKnockableBack;

    protected override void Update()
    {
        base.Update();

        if (speedable && StateChangedThisFrame && PreviousDoorOpenedState == true)
        {
            //Speed Up Player
            Debug.Log("Speed Up Player");
            speedable = false;
        }

        if (enemyKnockable && StateChangedThisFrame && PreviousDoorOpenedState == true)
        {
            //Knockback Enemy
            Debug.Log("Knock Back Enemy");
            enemyKnockable = false;
        }

        if (playerKnockableBack && StateChangedThisFrame && PreviousDoorOpenedState == true)
        {
            //Knockback Player Back Door
            Debug.Log("Knock Back Player Back Door");
            playerKnockableBack = false;
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
            Debug.Log("Knock Back Enemy");
        }
    }

    //Back - Enter
    public void EnemyOpenInBack(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && DoorOpened == false)
        {
            //Enemy Open In Back
            Debug.Log("Enemy Open In Back");
        }
    }

    //Speedy Front - Enter
    public void ActivateSpeedable(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && DoorOpened == true)
        {
            speedable = true;
        }
    }

    //Speedy Front - Exit
    public void DeActivateSpeedable(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            speedable = false;
        }
    }

    //Back - Enter
    public void ActivateEnemyKnockable(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && DoorOpened == true)
        {
            enemyKnockable = true;
        }
    }

    //Back - Exit
    public void DeActivateEnemyKnockable(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        { 
            enemyKnockable = false;
        }
    }

    //Back - Enter
    public void ActivatePlayerKnockableBack(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && DoorOpened == true)
        {
            playerKnockableBack = true;
        }
    }

    //Back - Exit
    public void DeActivatePlayerKnockableBack(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerKnockableBack = false;
        }
    }
}
