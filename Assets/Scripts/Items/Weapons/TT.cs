using UnityEngine;

public class TT : Gun
{
    public override void Reload()
    {
        // Реализация перезарядки для TT
        Debug.Log("TT Reloading");

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
