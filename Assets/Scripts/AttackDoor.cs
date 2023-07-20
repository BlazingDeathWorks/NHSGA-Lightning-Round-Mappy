using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDoor : Door
{
    [SerializeField] private SoundWave soundWave;
    [SerializeField] private Door regularDoor;

    protected override void Update()
    {
        base.Update();
        if (StateChangedThisFrame)
        {
            InstantiateSoundWave();
        }

        ResetStateChangedThisFrame();
    }

    private void InstantiateSoundWave()
    {
        SoundWave instance = Instantiate(soundWave, FrontKnockBack.transform.position, Quaternion.Euler(0, 0, 0));
        AudioManager.Instance.Play("SoundWaveSound");
        if (Mathf.Sign(transform.localScale.x) == 1) 
        {
            instance.transform.localEulerAngles = new Vector3(0, 0, 180);
        }

        Door doorInstance = Instantiate(regularDoor, transform.position, Quaternion.identity);
        doorInstance.DoorOpened = true;
        doorInstance.transform.localScale = new Vector3(Mathf.Abs(doorInstance.transform.localScale.x) * Mathf.Sign(transform.localScale.x), doorInstance.transform.localScale.y, doorInstance.transform.localScale.z);
        doorInstance.Initialize();
        Destroy(gameObject);
    }
}
