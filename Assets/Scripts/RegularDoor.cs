using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularDoor : Door
{
    private bool speedable = false;
    private bool enemyKnockable;

    protected override void Update()
    {
        base.Update();

        //TODO: Fix this
        if (speedable && StateChangedThisFrame)
        {
            //Speed Up Player
            Debug.Log("Speed Up Player");
        }

        //TODO: Fix this
        if (enemyKnockable && StateChangedThisFrame)
        {
            //Knockback Enemy
            Debug.Log("Knock Back Enemy");
        }
    }

    public void KnockEnemyInFront(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && DoorOpened == false)
        {
            //Knock back enemy
            Debug.Log("Knock Back Enemy");
        }
    }

    public void EnemyOpenInBack(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && DoorOpened == false)
        {
            //Enemy Open In Back
            Debug.Log("Enemy Open In Back");
            ActivatePlayerKnockable(collision);
        }
    }

    public void ActivateSpeedable(Collider2D collision)
    {
        speedable = !speedable;
    }

    public void ActivateEnemyKnockable(Collider2D collision)
    {
        enemyKnockable = !enemyKnockable;
    }
}
