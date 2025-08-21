using Mirror;
using UnityEngine;

public class ZombieRunnerController : ZombieController
{

    [SerializeField] private bool isInCharge = false;
    [SerializeField] private float chargeCooldown = 10f;
    [SerializeField] private float lastChargeTime = 0f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargeTime = 5f;
    [SerializeField] private float chargeAnimSpeed = 2f;
    public virtual void FixedUpdate()
    {
        if (!isServer) return;
        if (_targetToChase != null && players.Contains(_targetToChase) && _targetToChase.IsAlive)
        {
            _state = AIState.Attack;
            Debug.Log("������ �����!");

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
                    _targetToChase.CmdTakeDamage(attackDamage); // Use Command instead of direct call
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
                if (!isInCharge && Time.time > lastChargeTime + chargeCooldown)
                {
                    isInCharge = true;
                    _animator.SetBool("IsRunning", true);
                    lastChargeTime = Time.time;
                }
                else if(isInCharge && Time.time > lastChargeTime + chargeTime)
                {
                    isInCharge = false;
                    _animator.SetBool("IsRunning", false);
                }

                if (isInCharge)
                {
                    _animator.speed = chargeAnimSpeed;
                    _agent.speed = chargeSpeed;
                }
                else
                {
                    _animator.speed = runAnimSpeed;
                    _agent.speed = moveSpeed;
                }

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
