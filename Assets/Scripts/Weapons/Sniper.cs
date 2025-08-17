using UnityEngine;

public class Sniper : Gun
{
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
