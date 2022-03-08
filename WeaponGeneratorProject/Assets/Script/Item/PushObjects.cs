using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjects : MonoBehaviour
{
    private float pushForce = 1f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var item = hit.collider.GetComponent<Item>();

        if (item == null) return;
        if (!item.Data.IsMovable) return;

        PushObject(hit);
    }

    private void PushObject(ControllerColliderHit hit)
    {
        var rb = hit.collider.attachedRigidbody;
        if (rb == null) return;

        Vector3 direction = hit.gameObject.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        pushForce = rb.mass / 3.33f;
        rb.AddForceAtPosition(direction * pushForce, transform.position, ForceMode.Impulse);
    }
}
