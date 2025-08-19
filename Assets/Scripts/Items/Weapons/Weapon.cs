using UnityEngine;

public abstract class Weapon : Item
{
    [SerializeField] protected int damage;
    [SerializeField] protected int range;
    [SerializeField] protected float fireRate; // скорострельность
    protected float lastShotTime = int.MinValue; // время последнего выстрела
    public abstract void Attack();

}