using System;
using UnityEngine;

[Serializable]
public abstract class Gun : Weapon
{
    public event Action<int, int> OnAmmoChanged;
    
    [Header("Gun Settings")]
    [SerializeField] protected int clipSize; // размер обоймы
    [SerializeField] public int totalAmmo; // общее количество патронов
    [SerializeField] protected float scatterAngle = 2f; // угол разброса
    [SerializeField] private LayerMask shootMask; // попадания только для заданных слоев
    [Header("Audio Settings")]
    [SerializeField] protected AudioClip shootClip; // звук выстрела
    [SerializeField] protected AudioClip reloadClip; // звук перезарядки
    [Header("Animaton Settings")]
    protected GunAnimHelper _gunAnimHelper;

    [Header("Bullet Settings")]
    [SerializeField] protected GameObject bulletPrefab; // сущность пули
    [SerializeField] protected Transform muzzlePoint; // точка появления пули
    [SerializeField] protected float bulletSpeed = 50f; // скорость пули

    protected int _currentAmmo; // текущее количество патронов
    protected AudioSource audio; // воспроизводчик звуков
    private bool _canShoot => _currentAmmo > 0 && Time.time > fireRate + lastShotTime; // флаг стрелять
    private bool _isReloading;
    
    public int CurrentAmmo => _currentAmmo; 
    public int ClipSize => clipSize; // размер обоймы
    public int TotalAmmo => totalAmmo;

    public void Awake()
    {
        _currentAmmo = clipSize;
    }
    
    public virtual void Start()
    {
        audio = GetComponent<AudioSource>();
        _gunAnimHelper = GetComponent<GunAnimHelper>();

        // Debug.Log("CurrentAmmo: " + _currentAmmo);
        // Debug.Log("Time before shooting: " + (fireRate + lastShotTime - Time.time));
    }

    public virtual void Reload()
    {
        if (_currentAmmo == clipSize)
        {
            Debug.Log("No need to reload");
            throw new CustomException("Clip is full");
        }
        int currentAmmo = _currentAmmo;

        _currentAmmo = Mathf.Min(clipSize, totalAmmo); // устанавливаем текущее количество патронов в обойме

        totalAmmo -= clipSize - currentAmmo; // уменьшаем общее количество патронов на количество, которое было использовано

        if (totalAmmo <= 0)
        {
            totalAmmo = 0;
            Debug.Log("No ammo to reload");
            return;
        }
        audio.PlayOneShot(reloadClip); // воспроизводим звук перезарядки
        AmmoChangedNotify(_currentAmmo, totalAmmo);
        //_gunAnimHelper.PlayReloadAnim();
    }
    
    public void AmmoChangedNotify(int currentAmmo, int total)
    {
        OnAmmoChanged?.Invoke(currentAmmo, total);
    }

    public override void Attack()
    {
        //Debug.Log($"Attack called: _canShoot={_canShoot}, _currentAmmo={_currentAmmo}, Time={Time.time}, lastShotTime={lastShotTime}, _clipSize ={clipSize}");
        if (!_canShoot)
        {
            //if (Time.time > fireRate + lastShotTime)
            //    Debug.Log("Cannot shoot yet");
            //if (_currentAmmo <= 0)
            //    Debug.Log("Out of ammo, cannot shoot");
            //throw new CustomException("You can't shoot now"); // если не может стрелять, выходим
            return;
        }

        audio.PlayOneShot(shootClip); // воспроизводим звук выстрела

        lastShotTime = Time.time; // обновляем время последнего выстрела
        _currentAmmo--; // уменьшаем количество патронов в обойме
        OnAmmoChanged?.Invoke(_currentAmmo, totalAmmo);

        Camera cam = Camera.main;
        Vector3 direction = GetScatterDirection(cam);

        Ray ray = new Ray(cam.transform.position, direction);
        RaycastHit hit;
        Vector3 hitPoint;

        if (Physics.Raycast(ray, out hit, range, shootMask))
        {
            Debug.Log("Hit: " + hit.collider.name);
            hitPoint = hit.point;

            ZombieController hitObject = hit.collider.GetComponent<ZombieController>();
            if (hitObject != null)
            {
                Debug.Log("Take damage in count: " + damage);
                int potentialScore = hitObject.score;
                hitObject.CmdTakeDamage(damage); // Use Command instead of direct call
                // Note: Score will be handled when zombie actually dies on server
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

        // анимация выстрела
        //_gunAnimHelper.PlayShootAnim();

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

    public bool AppendCartriges(int count)
    {
        if (_isReloading)
        {
            // throw new CustomException("Gun is reloading");
            Debug.Log("Gun is reloading");
            return false;
        }
        else if (!_canShoot)
        {
            // throw new CustomException("Pause between shots");
            Debug.Log("Pause between shots");
            return false;
        }
        else
        {
            totalAmmo += count;
            return true;
        }
    }
}
