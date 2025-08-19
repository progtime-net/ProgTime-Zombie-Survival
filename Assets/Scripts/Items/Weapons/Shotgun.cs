using UnityEngine;

public class Shotgun : Gun
{
    public override void Start()
    {
        damage = 25;
        range = 10;
        fireRate = 0.8f;
        clipSize = 2; // размер обоймы
        totalAmmo = 30;

        // обязательно после инициализации других переменных
        base.Start(); // Initialize base class
    }

    public override void Reload()
    {
        // Реализация перезарядки для Shotgun
        Debug.Log("Shotgun Reloading");

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
