using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlate : MonoBehaviour
{
    public List<GameObject> objectsInTrigger = new List<GameObject>();
    public float forceMultiplier = 1f;

    private void BounceBack()
    {           
        var rb = objectsInTrigger[0].GetComponentInParent<Rigidbody>();
        if (rb == null) return;
        rb.AddForce(-rb.velocity * forceMultiplier, ForceMode.Impulse);
    }
}
