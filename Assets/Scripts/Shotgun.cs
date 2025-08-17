using UnityEngine;

public class Shotgun : Gun
{
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
