using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeController : NetworkBehaviour
{
    private List<IDamageable> _damageables = new List<IDamageable>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Server]
    public void Explode(float attackDamage)
    {
        foreach (var item in _damageables)
        {
            item.TakeDamage(attackDamage);
        }
    }
    [Server]
    protected void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Player") || obj.CompareTag("Zombie"))
        {
            _damageables.Add( obj.GetComponent<IDamageable>());

        }
    }
    [Server]
    protected void OnTriggerExit(Collider other)
    {
        if (!isServer) return;
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Player") || obj.CompareTag("Zombie"))
        {
            _damageables.Remove(obj.GetComponent<IDamageable>());
        }
    }
}
