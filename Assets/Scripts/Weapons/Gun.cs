using System;
using UnityEngine;

[Serializable]
public abstract class Gun : MonoBehaviour
{
    protected float damage;
    protected float range;
    protected float fireRate; // скорострельность
    protected float clipSize; // размер обоймы
    protected float currentAmmo; // 

    public abstract void Shoot();
    public abstract void Reload();
}
