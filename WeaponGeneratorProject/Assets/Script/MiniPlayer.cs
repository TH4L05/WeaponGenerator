using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPlayer : MonoBehaviour
{
    public Transform raycastStart;
    public Material hitMaterial;

    void Update()
    {
        
    }

    public void CreateRay()
    {
        Vector3 rayOrigin = raycastStart.position;
        Vector3 rayDirection = raycastStart.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            var material = hit.collider.GetComponent<MeshRenderer>().material;
            if (material == null) return;

            material = hitMaterial;

        }
    }
}
