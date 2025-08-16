using UnityEngine;

public class AK : Gun
{
    void Start()
    {
        damage = 10f;
        range = 100f;
        fireRate = 0.4f;
        clipSize = 30f; // размер обоймы
        if(totalAmmo < clipSize)
            _currentAmmo = totalAmmo; // инициализация текущего количества патронов
        else
            _currentAmmo = clipSize;
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
