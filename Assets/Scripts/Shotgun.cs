using UnityEngine;

public class Shotgun : Gun
{
    void Start()
    {
        damage = 25f;
        range = 10f;
        fireRate = 0.8f; // скорострельность
        clipSize = 2f; // размер обоймы
        _currentAmmo = clipSize; // инициализация текущего количества патронов
    }
    public override void Reload()
    {
        // Реализация перезарядки для Shotgun
        Debug.Log("Shotgun Reloaded");

        //TODO: Animation

        base.Reload();
    }

    public override void Attack()
    {
        // Реализация стрельбы для Shotgun
        Debug.Log("Shotgun Shot Fired");

        //TODO: Animation

        base.Attack(); // Call the base Attack method to decrease ammo

    }
}
