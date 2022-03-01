using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Behaviour : MonoBehaviour
{
    protected enum EnemyState
    {
        Idle,
        MoveToTarget,
        MoveToPosition,
        Attack,
        Alert,
        Death,
    }

    [SerializeField] protected NavMeshAgent navAgent;
    [SerializeField] protected float attackRange = 3.0f;
    [SerializeField] protected float lookRange = 10f;
    [SerializeField] protected float minimumDectectRange = 4f;
    [SerializeField] protected float idleTime = 3.0f;
    [SerializeField] protected bool manualState;
    [SerializeField] protected Enemy enemy;
    [SerializeField] protected EnemyState state;
    [SerializeField] protected EnemyState laststate;
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform randomTargetTransform;
    private bool moveToPosition;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        target = Game.instance.player.transform;
        randomTargetTransform = Game.instance.RandomTargetPosition;
    }

    void Update()
    {
        UpdateState();
    }

    protected EnemyState StateCheck()
    {
        float distance = DistanceCheck();

        if (distance <= lookRange)
        {
            state = EnemyState.MoveToTarget;
        }
        else
        {
            state = EnemyState.Idle;
        }

        return state;
    }

    protected void UpdateState()
    {
        var activeState = StateCheck();

        if (state == laststate) return;
        laststate = activeState;

        switch (activeState)
        {
            case EnemyState.Idle:
            default:
                StateIdle();
                break;
            case EnemyState.MoveToTarget:
                StateMoveToTarget();
                break;
            case EnemyState.MoveToPosition:
                StateMoveToPosition();
                break;
            case EnemyState.Attack:
                StateAttack();
                break;
            case EnemyState.Alert:
                StateAlert();
                break;
            case EnemyState.Death:
                StateDeath();
                break;
        }
    }

    protected void StateIdle()
    {
    }

    protected void StateMoveToTarget()
    {
        navAgent.isStopped = false;
        /*navAgent.speed = enemy.data.SprintSpeed;
        navAgent.acceleration = enemy.SprintSpeed * 2;*/
        navAgent.stoppingDistance = 1f;
        navAgent.SetDestination(target.position);

        float speedValue = navAgent.velocity.magnitude / navAgent.speed;
    }

    protected void StateMoveToPosition()
    {
        navAgent.isStopped = false;
        /*navAgent.speed = enemy.MoveSpeed;
        navAgent.acceleration = enemy.MoveSpeed * 2;*/
        navAgent.stoppingDistance = 1f;

        if (navAgent.remainingDistance < 0.1f)
        {
            if (!moveToPosition)
            {
                moveToPosition = true;
                SetRandomTarget();
            }
            else
            {
                state = EnemyState.Idle;
            }
        }

        float speedValue = navAgent.velocity.magnitude / navAgent.speed;
    }

    protected void StateAttack()
    {
        throw new NotImplementedException();
    }

    protected void StateAlert()
    {
        throw new NotImplementedException();
    }

    protected void StateDeath()
    {
        throw new NotImplementedException();
    }


    protected float DistanceCheck()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        return distance;
    }

    protected virtual void LookAtTarget(Vector3 targetposition)
    {
        transform.LookAt(targetposition);
    }

    protected virtual void LookAtTargetSmooth(Vector3 targetposition, float ratio)
    {
        Vector3 direction = (targetposition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * ratio);
    }

    protected Vector3 RandomTargetPosition()
    {
        Vector3 position = UnityEngine.Random.insideUnitSphere * 5;
        position += transform.position;

        NavMesh.SamplePosition(position, out NavMeshHit hit, 20, 1);

        return hit.position;
    }

    protected void SetRandomTarget()
    {
        randomTargetTransform.position = RandomTargetPosition();
        navAgent.SetDestination(randomTargetTransform.position);
    }
}
