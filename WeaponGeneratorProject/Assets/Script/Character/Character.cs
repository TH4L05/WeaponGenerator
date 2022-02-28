using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour , IDamageable
{
    #region Fields

    [SerializeField] protected CharacterData data;
    [SerializeField] protected Health health;
    [Range(0.1f, 5f)] [SerializeField] protected float deleteTime = 1.0f;
    protected bool isDead;   
    protected Timer timer;    

    #endregion

    #region UnityFunctions

    private void Start()
    {
        StartSetup();
        ConnectEvents();
    }


    private void OnDestroy()
    {
        if (health != null) health.OnNoHealthLeft -= Death;
        if (timer != null) timer.OnTimerFinished -= DeathSetup;
    }

    #endregion


    protected virtual void StartSetup()
    {
        timer = GetComponent<Timer>();
    }

    private void ConnectEvents()
    {
        if (health != null) health.OnNoHealthLeft += Death;
        if (timer != null) timer.OnTimerFinished += DeathSetup;
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log($"{transform.name} get hit and takes damage of {damage}");
        health.ReduceHealth(damage);
    }

    public virtual void Death()
    {
        if (isDead) return;
        isDead = true;
        timer.StartTimer(deleteTime, false);
    }

    public virtual void DeathSetup()
    {
        if (!isDead) return;
        Destroy(gameObject);
    }
}
