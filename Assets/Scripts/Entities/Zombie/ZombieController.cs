using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : NetworkBehaviour, IDamageable
{
    protected enum AIState { Disabled, Idle, Chase, Attack }
    [Header("AI Settings")]
    [SerializeField] protected float moveSpeed = 4f;
    [SerializeField] protected float runAnimSpeed = 1f;
    [SerializeField] protected float damageCooldown = 1f;
    [SerializeField] protected float reAggressiveCooldown = 10f;
    [SerializeField] protected float attackDamage = 10f;
    [SerializeField] protected GameObject ragdoll;

    protected NavMeshAgent _agent;
    protected Animator _animator;


    protected AIState _state = AIState.Chase;
    [SerializeField] [SyncVar] protected float _health = 20f;
    protected List<PlayerController> players = new List<PlayerController>();
    protected IDamageable _targetToAttack = null;
    protected PlayerController _targetToChase = null;
    protected float _lastAttackTime = int.MinValue;
    protected float _reAggressiveTime = int.MinValue;
    protected bool isInAttack = false;
    public virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        if (!isServer) _agent.enabled = false;
    }

    protected Transform GetClosestPlayer()
    {
        float minDist = int.MaxValue;
        PlayerController closestPlayer = null;

        _reAggressiveTime = Time.time;
        foreach (var player in GameManager.Instance.AllPlayers)
        {
            if (!player.IsAlive) continue;
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist <= minDist)
            {
                minDist = dist;
                closestPlayer = player;
            }
        }
        _targetToChase = closestPlayer;
        return closestPlayer.transform;
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
        GameObject rag = Instantiate(
            ragdoll,
            transform.position,
            transform.rotation
        );

        ragdoll.transform.localScale = transform.localScale;
        CopyTransform(transform, rag.transform);
        
        Destroy(gameObject);
    }

    protected void CopyTransform(Transform source, Transform target)
    {
        target.position = source.position;
        target.rotation = source.rotation;

        foreach (Transform targetChild in target)
        {
            Transform sourceChild = source.Find(targetChild.name);
            if (sourceChild != null)
                CopyTransform(sourceChild, targetChild);
        }
    }

    [Server]
    protected void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        GameObject obj = other.gameObject;

        if (obj.CompareTag("Player"))
        {
            
            _targetToAttack = obj.GetComponent<IDamageable>();
            players.Add(obj.GetComponent<PlayerController>());
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
            players.Remove(obj.GetComponent<PlayerController>());
        }
    }
    [Server]
    public virtual void FixedUpdate()
    {
        if (!isServer) return;
        if (_targetToChase != null && players.Contains(_targetToChase) && _targetToChase.IsAlive)
        {
            _state = AIState.Attack;
            

        }
        else
        {

            _animator.SetBool("IsInAttack", false);
            isInAttack = false;
            _state = AIState.Chase;
        }
        switch (_state)
        {
            case AIState.Attack:
                if (!isInAttack)
                {
                    _lastAttackTime = Time.time;
                    _animator.SetBool("IsInAttack", true);
                    isInAttack = true;
                }
                else if (Time.time - _lastAttackTime >= damageCooldown)
                {
                    _targetToChase = (_targetToAttack as PlayerController);
                    _targetToAttack.TakeDamage(attackDamage);
                    _animator.SetBool("IsInAttack", false);
                    isInAttack = false;
                }
                //if (players.Contains(_targetToChase) &&
                //Time.time >= _lastAttackTime + damageCooldown)
                //{
                //    Debug.Log("attack");
                //    _lastAttackTime = Time.time;
                //    _reAggressiveTime = Time.time;
                //    _targetToChase = (_targetToAttack as PlayerController);
                //    _targetToAttack.TakeDamage(attackDamage);
                //}
                break;
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
