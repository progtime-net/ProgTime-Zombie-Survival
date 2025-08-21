using UnityEngine;

public class Pistol : Gun
{
    public override void Start()
    {
        // обязательно после инициализации других переменных
        base.Start(); // Initialize base class
    }
    public override void Reload()
    {
        // Реализация перезарядки для Pistol
        Debug.Log("Pistol Reloading");

        //TODO: Animation

        if(_currentAmmo == clipSize)
        {
            Debug.Log("No need to reload");
            return;
        }

        _currentAmmo = clipSize; // восстановление текущего количества патронов
        // base.Reload(); // Call the base Reload method to handle ammo logic

        audio.PlayOneShot(reloadClip); // воспроизводим звук перезарядки
        _gunAnimHelper.PlayReloadAnim();
    }

    public override void Attack()
    {
        // Реализация стрельбы для Pistol
        Debug.Log("Pistol Shot Fired");

        //TODO: Animation

        base.Attack(); // Call the base Attack method to decrease ammo
        //audio.PlayOneShot(shootClip); // воспроизводим звук выстрела
    }
}
