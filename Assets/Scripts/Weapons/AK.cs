using UnityEngine;

public class AK : Gun
{
    void Start()
    {
        damage = 10f;
        range = 100f;
        fireRate = 0.4f;
        clipSize = 30f; // размер обоймы
    }
    public override void Reload()
    {
        // Реализация перезарядки для AK
        Debug.Log("AK Reloaded");

        //TODO: Animation


    }

    public override void Shoot()
    {
        // Реализация стрельбы для AK
        Debug.Log("AK Shot Fired");

        //TODO: Animation


        
    }
}
