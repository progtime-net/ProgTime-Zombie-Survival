using UnityEngine;

public class Sniper : Gun
{
    void Start()
    {
        damage = 30f;
        range = 250f;
        fireRate = 1.2f; // скорострельность
        clipSize = 1f; // размер обоймы
    }
    public override void Reload()
    {
        
    }

    public override void Shoot()
    {

    }
}
