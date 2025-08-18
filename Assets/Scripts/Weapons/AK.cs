using UnityEngine;

public class AK : Gun
{
    public override void Start()
    {
        damage = 10;
        range = 100;
        fireRate = 0.4f;
        clipSize = 30; // размер обоймы
        totalAmmo = 100;

        // обязательно после инициализации других переменных
        base.Start(); // Initialize base class
    }
    public override void Reload()
    {
        // Реализация перезарядки для AK
        Debug.Log("AK Reloading");

        //TODO: Animation

        base.Reload();
    }

    public override void Attack()
    {
        // Реализация стрельбы для AK
        Debug.Log("AK Shot Fired");

        //TODO: Animation

        base.Attack(); // Call the base Attack method to decrease ammo
    }
}
