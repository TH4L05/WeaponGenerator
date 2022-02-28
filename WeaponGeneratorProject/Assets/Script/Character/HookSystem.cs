using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSystem : MonoBehaviour
{
    public List<GameObject> hookNodes = new List<GameObject>();
    public int maxHookNodeCount = 2;
    public int hookNodeCount = 0;
    public GameObject hookNodeTemplate;
    public Transform raycastStart;
    public float forceMultiplier = 2f;
    private LineRenderer lineRenderer;
    public Material lineMaterial;

    private void LateUpdate()
    {
        DrawLine();
    }

    public void CreateHookNode()
    {
        Vector3 rayOrigin = raycastStart.position;
        Vector3 rayDirection = raycastStart.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit hit;

        if (hookNodeCount == maxHookNodeCount) return;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            var hookTarget = Instantiate(hookNodeTemplate, hit.point, Quaternion.identity);
            hookTarget.transform.parent = hit.collider.transform;

            hookNodeCount++;
            hookNodes.Add(hookTarget);

            if (hookNodeCount > 1 | hookNodeCount == 0) return;            
            lineRenderer = hookNodes[0].AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
        }
    }

    public void PullObjectToCharakter()
    {
        if (hookNodeCount == 0) return;
        if (hookNodeCount > 1) return;

        Vector3 direction = hookNodes[0].transform.position - Game.instance.player.transform.position;
        var rb = hookNodes[0].transform.GetComponentInParent<Rigidbody>();

        if (rb == null) return;
        rb.AddForceAtPosition(direction * rb.mass * forceMultiplier, hookNodes[0].transform.parent.position, ForceMode.Impulse);
    }

    public void AddForceToHookedObjects()
    {
        if (hookNodeCount == 1)
        {
            AddForceToCharacter(hookNodes[0].transform);
            hookNodes.Clear();
            hookNodeCount = 0;
            return;
        }

        if (hookNodeCount == 0) return;
 
        InvokeRepeating("HookObjectsTogether", 0, 0.2f);  
    }

    private void HookObjectsTogether()
    {
        var distance = Vector3.Distance(hookNodes[0].transform.position, hookNodes[1].transform.position);

        if (distance > 2)
        {
            for (int i = 0; i < hookNodeCount; i++)
            {
                Vector3 direction = Vector3.zero;

                if (i + 1 > hookNodeCount - 1)
                {
                    direction = hookNodes[0].transform.position - hookNodes[i].transform.position;
                }
                else
                {
                    direction = hookNodes[i + 1].transform.position - hookNodes[i].transform.position;
                }
                direction.Normalize();

                var rb = hookNodes[i].transform.GetComponentInParent<Rigidbody>();
                if (rb == null) continue;

                forceMultiplier = Mathf.Clamp(rb.mass * 2, 1, 900);

                rb.AddForceAtPosition(direction * forceMultiplier, hookNodes[i].transform.position, ForceMode.Impulse);
            }
        }
        else
        {
            CancelInvoke("HookObjectsTogether");
            foreach (var hook in hookNodes)
            {
                Destroy(hook);
            }

            hookNodes.Clear();
            hookNodeCount = 0;
        }
    }

    public void AddForceToCharacter(Transform hooknode)
    {
        Game.instance.player.HandleHookShotMovement(hooknode, forceMultiplier);
    }

    private void DrawLine()
    {
        if (hookNodeCount == 0) return;

        lineRenderer.SetPosition(0, hookNodes[0].transform.position);
        if (hookNodeCount == 1)
        {
            var position = new Vector3(Game.instance.player.transform.position.x, Game.instance.player.transform.position.y + 0.5f, Game.instance.player.transform.position.z);
            lineRenderer.SetPosition(1, position);
            return;
        }

        for (int i = 1; i < hookNodeCount; i++)
        {
            lineRenderer.SetPosition(i, hookNodes[i].transform.position);
        }
    }
}
