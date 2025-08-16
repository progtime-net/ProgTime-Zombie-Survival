using UnityEngine;

public class Sniper : Gun
{
    void Start()
    {
        damage = 30f;
        range = 250f;
        fireRate = 1.2f; // скорострельность
        clipSize = 1f; // размер обоймы
        _currentAmmo = clipSize; // инициализация текущего количества патронов
    }
    public override void Reload()
    {
        // Реализация перезарядки для AK
        Debug.Log("AK Reloaded");

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
