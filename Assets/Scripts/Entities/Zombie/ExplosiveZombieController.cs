using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class ExplosiveZombieController : ZombieController
{
    [SerializeField]private ExplodeController _explosiveRadius;
    public float timeToExplode = 2f;   // ����� �� ������
    public float pulseSpeed = 10f;     // �������� ���������
    public float pulseAmount = 0.2f;   // ��������� ������������� ������� ��� ������
    private bool _IsAlive = true;
    private Vector3 originalScale;
    private float timer;
    [Server]
    public override void Start()
    {
        _state = AIState.Chase;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        originalScale = transform.localScale;
        timer = timeToExplode;

        if (!isServer) _agent.enabled = false;
    }
    [Server]
    public override void Death()
    {
        _state = AIState.Disabled;

        /*Collider[] colls = GetComponents<Collider>();
        foreach (var coll in colls) coll.enabled = false;*/

        _agent.enabled = false;
        //TODO: death anim
        /*GameObject rag = Instantiate(
            ragdoll,
            transform.position,
            transform.rotation
        );

        ragdoll.transform.localScale = transform.localScale;
        CopyTransform(transform, rag.transform);*/
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        
        _explosiveRadius.Explode(attackDamage);
    }
    [Server]
    public override void FixedUpdate()
    {
        
        if (!isServer) return;
        if (_targetToChase != null && players.Contains(_targetToChase) && _targetToChase.IsAlive && _IsAlive) 
        {
            
            _IsAlive = false;
            
            //_state = AIState.Attack;
            //Debug.Log("������ �����!");

        }
        if(!_IsAlive)
        {
            timer -= Time.deltaTime;
            float t = (timeToExplode - timer) / timeToExplode;
            float speedMultiplier = Mathf.Lerp(1f, 5f, t);

            // ���������� � ������� sin
            float scaleOffset = Mathf.Sin(Time.time * pulseSpeed * speedMultiplier) * pulseAmount * t;
            transform.localScale = originalScale * (1f + scaleOffset);
        }
        if(timer<=0f)
        {
            Death();
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
