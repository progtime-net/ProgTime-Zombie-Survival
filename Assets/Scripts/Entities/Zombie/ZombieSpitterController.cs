using Mirror;
using UnityEngine;

public class ZombieSpitterController : ZombieController
{
    [SerializeField]private float shootDistance = 20f;
    [SerializeField] private float scatterAngle = 10f;
    [SerializeField]private LayerMask shootMask;
    [SerializeField] private GameObject point;
    [SerializeField] private GameObject spitPrefab;
    [SerializeField] private float projectileSpeed = 30f;
    private Ray ray;
    private Vector3 GetScatterDirection(Vector3 direction)
    {


        float scatterX = Random.Range(-scatterAngle, scatterAngle);
        float scatterY = Random.Range(-scatterAngle, scatterAngle);

        Quaternion scatterRotation = Quaternion.Euler(scatterX, scatterY, 0);
        return scatterRotation * direction;
    }
    
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

                _agent.speed =0f;
                if (!isInAttack)
                {
                    _lastAttackTime = Time.time;
                    _animator.SetBool("IsInAttack", true);
                    isInAttack = true;
                }
                else if (Time.time - _lastAttackTime >= damageCooldown)
                {
                    Vector3 direction = GetScatterDirection((_targetToChase as PlayerController).transform.position - point.transform.position);

                    ray = new Ray(point.transform.position, direction);
                    RaycastHit hit;
                    Vector3 hitPoint;
                    if (Physics.Raycast(ray, out hit, shootDistance, shootMask))
                    {
                       
                        hitPoint = hit.point;
                        _targetToChase = (_targetToAttack as PlayerController);
                        GameObject hitObj = hit.collider.gameObject;
                        if (hitObj.CompareTag("Player"))
                        {
                            hitObj.GetComponent<IDamageable>().TakeDamage(attackDamage);
                        }
                    }
                    else
                    {
                        
                        hitPoint = ray.origin + ray.direction * shootDistance;
                    }
                    _animator.SetBool("IsInAttack", false);
                    isInAttack = false;
                    GameObject bullet = Instantiate(spitPrefab, point.transform.position, Quaternion.identity);
                    
                    bullet.GetComponent<BulletController>().Init(hitPoint, projectileSpeed);
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
