using UnityEngine;

public class TT : Gun
{
    void Start()
    {
        damage = 5f;
        range = 50f;
        fireRate = 0.6f; // скорострельность
        clipSize = 8f; // размер обоймы
    }
    public override void Reload()
    {
        
    }

    public override void Shoot()
    {

    }
}
