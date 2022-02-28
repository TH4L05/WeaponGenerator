using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootTargets : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>();
    public UnityEvent OnAllTargetshit;
    int targethitcount;

    private void Start()
    {
        InvokeRepeating("CheckTargetHitStatus", 1f, 1f);
    }

    private void CheckTargetHitStatus()
    {
        if (targethitcount == targets.Count)
        {
            OnAllTargetshit?.Invoke();
            CancelInvoke("CheckTargetHitStatus");
        }
    }

    public void UpdateHitCount()
    {
        targethitcount++;
    }

}
