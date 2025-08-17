using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float range;
    [SerializeField] protected float fireRate; // скорострельность
    protected float lastShotTime = int.MinValue; // время последнего выстрела
    public abstract void Attack();

}