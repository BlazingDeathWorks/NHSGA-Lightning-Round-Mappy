using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorEventCollider : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider2D> onCollided;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCollided.Invoke(collision);
    }
}
