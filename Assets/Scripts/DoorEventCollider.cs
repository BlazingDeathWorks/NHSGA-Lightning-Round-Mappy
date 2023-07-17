using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorEventCollider : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider2D> onCollided;
    [SerializeField] private UnityEvent<Collider2D> onInCollider;
    [SerializeField] private UnityEvent<Collider2D> onDeCollided;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCollided.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        onInCollider.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onDeCollided.Invoke(collision);
    }
}
