using UnityEngine;

public class Shotgun : Gun
{
    void Start()
    {
        damage = 25f;
        range = 10f;
        fireRate = 0.8f; // скорострельность
        clipSize = 2f; // размер обоймы
    }
    public override void Reload()
    {
        
    }

    public override void Shoot()
    {

    }
}
