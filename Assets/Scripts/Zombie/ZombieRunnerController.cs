using UnityEngine;

public class ZombieRunnerController : ZombieController
{

    [SerializeField] private bool isInCharge = false;
    [SerializeField] private float chargeCooldown = 10f;
    [SerializeField] private float lastChargeTime = 0f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargeTime = 5f;
    [SerializeField] private float chargeAnimSpeed = 2f;
    public override void FixedUpdate()
    {
        if (!isServer) return;
        switch (_state)
        {
            case AIState.Chase:
                if (!isInCharge && Time.time > lastChargeTime + chargeCooldown) {
                    isInCharge = true;
                    lastChargeTime = Time.time;
                }
                else if (isInCharge && Time.time > lastChargeTime + chargeTime) {
                    isInCharge = false;
                }
                if (isInCharge) {
                    _animator.speed = chargeAnimSpeed;
                    _agent.speed = chargeSpeed;
                } else {
                    _animator.speed = runAnimSpeed;
                    _agent.speed = moveSpeed;
                }
                   
                if(Time.time >=_reAggressiveTime+ reAggressiveCooldown)
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
                _lastAttackTime = Time.time;
                _reAggressiveTime = Time.time;
                _targetToChase = (_targetToAttack as PlayerController);
                _targetToAttack.TakeDamage(attackDamage);
            }
        }
    }
}
