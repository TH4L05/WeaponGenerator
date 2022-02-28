using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool isActive;
    public Color activeColor;


    public void TargetGetHit()
    {
        if (!isActive)
        {
            var mr = GetComponent<MeshRenderer>();
            mr.material.color = activeColor;

            isActive = true;
            var parent = GetComponentInParent<ShootTargets>();
            parent.UpdateHitCount();
        }
    }
}
