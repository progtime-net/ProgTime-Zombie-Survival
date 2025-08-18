using System;
using UnityEngine;

[Serializable]
public abstract class Gun : Weapon
{
    [SerializeField] protected float clipSize; // размер обоймы
    protected float _currentAmmo; // текущее количество патронов
    [SerializeField] protected float totalAmmo; // общее количество патронов
    [SerializeField] private float scatterAngle = 2f; // угол разброса
    [SerializeField] private LayerMask shootMask; // попадания только для заданных слоев
    [Header("Bullet Settings")]
    [SerializeField] protected GameObject bulletPrefab; // сущность пули
    [SerializeField] protected Transform muzzlePoint; // точка появления пули
    [SerializeField] protected float bulletSpeed = 50f; // скорость пули

    public virtual void Reload()
    {
        float currentAmmo = _currentAmmo;

        if (totalAmmo < clipSize)
            _currentAmmo = totalAmmo; // инициализация текущего количества патронов
        else
            _currentAmmo = clipSize;

        totalAmmo -= clipSize - currentAmmo; // уменьшаем общее количество патронов на количество, которое было использовано

        if (totalAmmo <= 0)
        {
            totalAmmo = 0;
            Debug.Log("No ammo to reload");
            return;
        }

        Debug.Log("Gun Reloaded");
    }

    public override void Attack()
    {
        if (_currentAmmo <= 0)
        {
            Debug.Log("Out of ammo");
            Reload();
        }

        lastShotTime = Time.time; // обновляем время последнего выстрела
        _currentAmmo--;
        // sound: shootAudio.PlayOneShot(shootClip);

        Camera cam = Camera.main;
        Vector3 direction = GetScatterDirection(cam);

        Ray ray = new Ray(cam.transform.position, direction);
        RaycastHit hit;
        Vector3 hitPoint;

        if (Physics.Raycast(ray, out hit, range, shootMask))
        {
            Debug.Log("Hit: " + hit.collider.name);
            hitPoint = hit.point;

            // TAKE DAMAGE
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Enemy"))
            {
                hitObject.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("HAHAHA YOU MISSED");
            hitPoint = ray.origin + ray.direction * range; // если не попало, то берем конечную точку
            // ray.origin - точка начала луча
        }

        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.identity);
        // Quaternion.identity - это поворот, который не изменяет ориентацию объекта
        bullet.GetComponent<BulletController>().Init(hitPoint, bulletSpeed);

        // animation: ShootAnimation

        // player.AddRecoil(0.25f, 0.25f); // добавление отдачи
    }

    private Vector3 GetScatterDirection(Camera cam)
    {
        // Реализация разброса пули
        Vector3 direction = cam.transform.forward;

        float x = UnityEngine.Random.Range(-scatterAngle, scatterAngle);
        float y = UnityEngine.Random.Range(-scatterAngle, scatterAngle);
        // Vector3 direction = cam.transform.forward + new Vector3(x, y, 0);
        // return direction.normalized;
        Quaternion scatterRotation = Quaternion.Euler(x, y, 0);
        return scatterRotation * direction;
    }
}
