using System.Collections;
using Mirror;
using UnityEngine;

public class ZombieJumperController : ZombieController
{
    [SerializeField] private bool isInJump = false;
    [SerializeField] private float jumpCooldown = 10f;
    [SerializeField] private float lastJumpTime = 0f;
    [SerializeField] private float jumpRange = 10f;
    [SerializeField] private float jumpTime = 2f;

    public override void FixedUpdate()
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


                if (!isInJump && Time.time > lastJumpTime + jumpCooldown && _agent.remainingDistance >= jumpRange)
                {
                    isInJump = true;
                    _animator.SetBool("IsJump", true);
                    lastJumpTime = Time.time;
                    Jump(_agent.velocity.normalized * jumpRange, 2);
                }
                


                if (!isInJump)
                {
                    _animator.speed = runAnimSpeed;
                    _agent.speed = moveSpeed;
                }
                else
                {
                    _agent.speed = 0;
                    _animator.speed = 1;
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


    void Jump(Vector3 target, float duration)
    {
        StartCoroutine(JumpRoutine(target, duration));
    }

    private IEnumerator JumpRoutine(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float time = 0f;
        float height = 6f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            Vector3 pos = Vector3.Lerp(start, start + target, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * height;

            transform.position = pos;
            yield return null;
        }

        isInJump = false;
        _animator.SetBool("IsJump", false);
    }
}
