using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineInvinsibleBox : MonoBehaviour
{
    private Trampoline trampoline;

    private void Awake()
    {
        trampoline = GetComponentInParent<Trampoline>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            trampoline.ResetBounceCount();
        }
    }
}
