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
        Debug.Log("EXPLODE");
        Debug.Log("objs:"+_damageables.Count);
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
        Debug.Log(obj);
        if (obj.CompareTag("Player")   )
        {
            _damageables.Add( obj.GetComponent<IDamageable>());
            return;
        }
        ZombieCollisionScript zcs = obj.GetComponent<ZombieCollisionScript>();
        Debug.Log(zcs.name, obj);
        if (zcs != null && !_damageables.Contains(zcs.owner))
        {
            _damageables.Add((zcs.owner));
        }
    }
    [Server]
    protected void OnTriggerExit(Collider other)
    {
        if (!isServer) return;
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Player") || obj.GetComponent<ZombieCollisionScript>() != null)
        {
            _damageables.Remove(obj.GetComponent<IDamageable>());
        }
        ZombieCollisionScript zcs = obj.GetComponent<ZombieCollisionScript>();
        if (zcs != null && _damageables.Contains(zcs.owner))
        {
            _damageables.Remove((zcs.owner));
        }
    }
}
