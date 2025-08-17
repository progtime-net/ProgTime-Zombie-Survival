using Mirror;
using UnityEngine;

public class SpitterController : ZombieController
{
    protected enum AIState { Disabled, Idle, Chase,Attack }
    
    [SerializeField]private float shootDistance = 10f;
    [SerializeField] private float scatterAngle = 10f;
    private LayerMask shootMask;
    [Server]
    public override void FixedUpdate()
    {
        if (!isServer) return;
        switch (_state)
        {
            case (ZombieController.AIState)AIState.Attack:
                _animator.speed = runAnimSpeed;
                _agent.speed = 0f;
                if (_targetToAttack != null &&
               Time.time >= _lastAttackTime + damageCooldown)
                {
                    _lastAttackTime = Time.time;
                    Vector3 direction = GetScatterDirection((_targetToAttack as PlayerController).transform.position - transform.position);

                    Ray ray = new Ray(transform.position, direction);
                    RaycastHit hit;
                    Vector3 hitPoint;

                    if (Physics.Raycast(ray, out hit, shootDistance, shootMask))
                    {
                        Debug.Log("Hit: " + hit.collider.name);
                        hitPoint = hit.point;

                        GameObject hitObj = hit.collider.gameObject;
                        if (hitObj.CompareTag("Player"))
                        {
                            hitObj.GetComponent<IDamageable>().TakeDamage(attackDamage);
                        }
                    }
                    else
                    {
                        Debug.Log("miss");
                        hitPoint = ray.origin + ray.direction * shootDistance;
                    }
                    _targetToAttack.TakeDamage(attackDamage);
                }
                break;
            case (ZombieController.AIState)AIState.Chase:
                _animator.speed = runAnimSpeed;
                _agent.speed = moveSpeed;
                if (Time.time >= _reAggressiveTime + reAggressiveCooldown)
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

       
    }
    private Vector3 GetScatterDirection(Vector3 direction)
    {
       

        float scatterX = Random.Range(-scatterAngle, scatterAngle);
        float scatterY = Random.Range(-scatterAngle, scatterAngle);

        Quaternion scatterRotation = Quaternion.Euler(scatterX, scatterY, 0);
        return scatterRotation * direction;
    }
}
