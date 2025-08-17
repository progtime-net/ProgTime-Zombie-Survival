using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : NetworkBehaviour,IDamageable
{
    protected enum AIState { Disabled, Idle, Chase}
    [Header("AI Settings")]
    [SerializeField] protected float moveSpeed=4f;
    [SerializeField] protected float runAnimSpeed=1f;
    [SerializeField] protected float damageCooldown=1f;
    [SerializeField] protected float reAggressiveCooldown=10f;
    [SerializeField] protected float attackDamage=10f;

    protected NavMeshAgent _agent;
    protected Animator _animator;

   
    [SyncVar] protected AIState _state = AIState.Chase;
    [SerializeField] [SyncVar] protected float _health = 20f;
    protected IDamageable _targetToAttack = null;
    protected Transform _targetToChase = null;
    protected float _lastAttackTime = int.MinValue;
    protected float _reAggressiveTime = int.MinValue;
    public virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        if (!isServer) _agent.enabled = false;
    }

    protected Transform GetClosestPlayer()
    {
        float minDist = int.MaxValue;
        Transform closestPlayer = null;
        _reAggressiveTime = Time.time;
        foreach (var player in GameManager.Instance.AllPlayers)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist <= minDist)
            {
                minDist = dist;
                closestPlayer = player.transform;
            }
        }
        _targetToChase = closestPlayer;
        return closestPlayer;
    }
    [Server]
    public virtual void TakeDamage(float damage)
    {
        if (_state == AIState.Disabled) return;
        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
            Death();
        }
    }
    [Server]
    public virtual void Death()
    {
        _state = AIState.Disabled;

        Collider[] colls = GetComponents<Collider>();
        foreach (var coll in colls) coll.enabled = false;

        _agent.enabled = false;
        //a������� ������

        Destroy(gameObject, 15f);
    }
    
    [Server]
    protected void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Player"))
        {
            _targetToAttack = obj.GetComponent<IDamageable>();

        }
    }
    [Server]
    protected void OnTriggerExit(Collider other)
    {
        if (!isServer) return;
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Player"))
        {
            _targetToAttack = null;
        }
    }
    [Server]
    public virtual void FixedUpdate()
    {
        if (!isServer) return;
        switch (_state)
        {
            case AIState.Chase:
                _animator.speed = runAnimSpeed;
                _agent.speed = moveSpeed;
                if(Time.time >=_reAggressiveTime+ reAggressiveCooldown)
                {
                    Transform targetPlayer = GetClosestPlayer();
                    _agent.SetDestination(targetPlayer.position);
                }
                else
                {
                    Transform targetPlayer = _targetToChase;
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
                _lastAttackTime = Time.time;
                _reAggressiveTime = Time.time;
                _targetToChase = (_targetToAttack as PlayerController).transform;
                _targetToAttack.TakeDamage(attackDamage);
            }
        }
    }
}
