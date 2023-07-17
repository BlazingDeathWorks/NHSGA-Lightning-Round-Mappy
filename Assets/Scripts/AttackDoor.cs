using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDoor : Door
{
    [SerializeField] private SoundWave soundWave;
    [SerializeField] private Door regularDoor;

    protected override void OnActivate()
    {
        base.OnActivate();
        InstantiateSoundWave();
    }

    private void InstantiateSoundWave()
    {
        SoundWave instance = Instantiate(soundWave, FrontKnockBack.transform.position, Quaternion.Euler(0, 0, -90));
        if (Mathf.Sign(transform.localScale.x) == 1) 
        {
            instance.transform.localEulerAngles = new Vector3(0, 0, 90);
        }

        Door doorInstance = Instantiate(regularDoor, transform.position, Quaternion.identity);
        doorInstance.DoorOpened = true;
        Destroy(gameObject);
    }
}
