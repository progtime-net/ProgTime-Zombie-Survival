using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class ExplosiveZombieController : ZombieController
{
    private GameObject _explosiveRadius;
    [Server]
    public override void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _explosiveRadius = GetComponentInChildren<GameObject>();
        if (!isServer) _agent.enabled = false;
    }
    [Server]
    public override void Death()
    {
        _state = AIState.Disabled;

        Collider[] colls = GetComponents<Collider>();
        foreach (var coll in colls) coll.enabled = false;

        _agent.enabled = false;
        //TODO: death anim
        _explosiveRadius.GetComponent<ExplodeController>().Explode(attackDamage);
        Destroy(gameObject);
    }
    [Server]
    public override void FixedUpdate()
    {
        /*if (!isServer) return;
        switch (_state)
        {
            case AIState.Chase:
                _animator.speed = runAnimSpeed;
                _agent.speed = moveSpeed;
                if (Time.time >= _reAggressiveTime + reAggressiveCooldown)
                {
                    Transform targetPlayer = GetClosestPlayer();
                    _agent.SetDestination(targetPlayer.position);
                }
                else
                {
                    Transform targetPlayer = _targetToChase.transform;
                    if (targetPlayer != null)
                    {
                        _agent.SetDestination(targetPlayer.position);
                    }
                }

                break;
        }

        if (_state != AIState.Disabled)
        {
            if (_targetToAttack != null &&
                Time.time >= _lastAttackTime + damageCooldown)
            {
                //TODO: add timer later
                Death();
            }
        }*/
        if (!isServer) return;
        if (_targetToChase != null && players.Contains(_targetToChase) && _targetToChase.IsAlive)
        {
            Death();
            //_state = AIState.Attack;
            //Debug.Log("Начало атаки!");

        }
        switch (_state)
        {
            
            case AIState.Chase:
                _animator.speed = runAnimSpeed;

                _agent.speed = moveSpeed;
                if (Time.time >= _reAggressiveTime + reAggressiveCooldown)
                {
                    Transform targetPlayer = GetClosestPlayer();

                    _agent.SetDestination(targetPlayer.position);
                }
                else
                {
                    Transform targetPlayer = _targetToChase.transform;
                    if (targetPlayer != null)
                    {
                        _agent.SetDestination(targetPlayer.position);
                    }
                }


                break;
        }
    }
}
