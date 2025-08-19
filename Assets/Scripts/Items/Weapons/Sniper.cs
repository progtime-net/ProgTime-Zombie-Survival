using UnityEngine;

public class Sniper : Gun
{
    public override void Start()
    {
        damage = 30;
        range = 200;
        fireRate = 1.2f;
        clipSize = 1; // размер обоймы
        totalAmmo = 25;

        // обязательно после инициализации других переменных
        base.Start(); // Initialize base class
    }

    public override void Reload()
    {
        // Реализация перезарядки для Sniper
        Debug.Log("Sniper Reloading");

        //TODO: Animation

        base.Reload();

    }

    public override void Attack()
    {
        // Реализация стрельбы для Sniper
        Debug.Log("Sniper Shot Fired");

        //TODO: Animation

        base.Attack(); // Call the base Attack method to decrease ammo

    }
}
