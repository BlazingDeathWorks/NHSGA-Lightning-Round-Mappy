using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            collision.gameObject.GetComponentInChildren<GroundScanner>().SetCurrentGroundedNone();
            playerMovement.ResetHopCapabilities();
        }
    }
}
