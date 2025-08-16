using UnityEngine;

public class TT : Gun
{
    void Start()
    {
        damage = 5f;
        range = 50f;
        fireRate = 0.6f; // скорострельность
        clipSize = 8f; // размер обоймы
        if(totalAmmo < clipSize)
            _currentAmmo = totalAmmo; // инициализация текущего количества патронов
        else
            _currentAmmo = clipSize;
    }
    public override void Reload()
    {
        // Реализация перезарядки для TT
        Debug.Log("TT Reloaded");

        //TODO: Animation

        _currentAmmo = clipSize; // восстановление текущего количества патронов

    }

    public override void Attack()
    {
        // Реализация стрельбы для TT
        Debug.Log("TT Shot Fired");

        //TODO: Animation

        base.Attack(); // Call the base Attack method to decrease ammo

    }
}
