using System;
using UnityEngine;

[Serializable]
public abstract class Gun : Weapon
{
    [SerializeField] protected float clipSize; // размер обоймы
    protected float _currentAmmo; // текущее количество патронов
    [SerializeField] protected float totalAmmo; // общее количество патронов

    public virtual void Reload()
    {
        if (totalAmmo <= 0)
        {
            Debug.Log("No ammo to reload");
            return;
        }
        totalAmmo -= clipSize - _currentAmmo; // уменьшаем общее количество патронов на количество, которое было использовано
        Debug.Log("Gun Reloaded");
        _currentAmmo = clipSize; // восстановление текущего количества патронов
    }

    public override void Attack()
    {
        if (_currentAmmo <= 0)
        {
            Reload();
        }

        _currentAmmo--;
    }
}
