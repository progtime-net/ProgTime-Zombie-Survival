using Mirror;
using UnityEngine;

public class TurretController : NetworkBehaviour
{
    [Header("Настройка турели")]
    [SerializeField] private Transform turretTower; 
    [SerializeField] private Transform turretVerticalPart; 
    [SerializeField] private float searchRadius = 20f; 
    [SerializeField] private float rotationSpeed = 5f; 
    [SerializeField] private float fireRate = 2f; 
    [SerializeField] private float damage = 10f; 
    [SerializeField] private float range = 50f; 

    [Header("Настройка стрельбы")]
    [SerializeField] private GameObject bulletPrefab; 
    [SerializeField] private Transform[] firePoints; 

    [SyncVar] private Transform _currentTarget;
    [SyncVar] private float _nextFireTime;
    [SyncVar] private int _currentFirePointIndex = 0;

    void Update()
    {
        if (_currentTarget == null)
        {
            FindClosestZombie();
        }
        else
        {
            float distanceToTarget = Vector3.Distance(transform.position, _currentTarget.position);
            if (distanceToTarget > searchRadius)
            {
                _currentTarget = null;
                return;
            }

            RotateTowardsTarget();

            if (CanShoot())
            {
                Shoot();
            }
        }
    }

    private void FindClosestZombie()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        float minDistance = float.MaxValue;
        Transform closestTarget = null;

        foreach (Collider collider in colliders)
        {
            ZombieCollisionScript zombieScript = collider.GetComponent<ZombieCollisionScript>();

            if (zombieScript != null && zombieScript.isHead)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
            
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTarget = collider.transform;
                }
            }
        }

        _currentTarget = closestTarget;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < 0f) angle = 360f + angle;
        if (angle > 180f) return Mathf.Max(angle, 360f + min);
        return Mathf.Min(angle, max);
    }
    
    private void RotateTowardsTarget()
    {
        Vector3 direction = _currentTarget.position - turretTower.position;
        direction.y = 0;
        Quaternion horizontalRotation = Quaternion.LookRotation(direction);
        turretTower.rotation = Quaternion.Slerp(turretTower.rotation, horizontalRotation, Time.deltaTime * rotationSpeed);

        Vector3 directionForVertical = _currentTarget.position - turretTower.position;
        Quaternion verticalRotation = Quaternion.LookRotation(directionForVertical);

        Quaternion clampedVerticalRotation = verticalRotation;
        Vector3 eulerAngles = clampedVerticalRotation.eulerAngles;
        eulerAngles.x = ClampAngle(eulerAngles.x, -15f, 60f);
        clampedVerticalRotation = Quaternion.Euler(eulerAngles);

        turretTower.rotation = Quaternion.Slerp(turretTower.rotation, clampedVerticalRotation, Time.deltaTime * rotationSpeed);
    }
    
    private bool CanShoot()
    {
        bool isTimeToShoot = Time.time >= _nextFireTime;
        
        Vector3 directionToTarget = (_currentTarget.position - turretTower.position).normalized;
        bool isFacingTarget = Vector3.Dot(turretTower.forward, directionToTarget) > 0.95f; 

        return isTimeToShoot && isFacingTarget;
    }
    
    private void Shoot()
    {
        _nextFireTime = Time.time + 1f / fireRate;
        
        // Получаем следующий Transform для выстрела
        Transform firePoint = GetNextFirePoint();
        
        Ray ray = new Ray(firePoint.position, (GetTargetPoint(firePoint) - firePoint.position).normalized);
        RaycastHit hit;
        Vector3 hitPoint;

        if (Physics.Raycast(ray, out hit, range))
        {
            hitPoint = hit.point;
            
            IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
            
            if (damageableObject != null)
            {
                damageableObject.TakeDamage(damage);
            }
        }
        else
        {
            hitPoint = ray.origin + ray.direction * range;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        BulletController bulletController = bullet.GetComponent<BulletController>();

        if (bulletController != null)
        {
            bulletController.Init(hitPoint, 50f);
        }
    }
    
    private Transform GetNextFirePoint()
    {
        // Выбираем текущее дуло
        Transform firePoint = firePoints[_currentFirePointIndex];

        // Увеличиваем индекс для следующего выстрела
        _currentFirePointIndex++;

        // Если индекс выходит за пределы массива, возвращаемся к началу
        if (_currentFirePointIndex >= firePoints.Length)
        {
            _currentFirePointIndex = 0;
        }

        return firePoint;
    }
    
    private Vector3 GetTargetPoint(Transform firePoint)
    {
        if (_currentTarget == null)
        {
            // Если цели нет, пуля летит вперед от этого firePoint
            return firePoint.position + firePoint.forward * range;
        }
    
        // В противном случае, пуля летит к текущей цели
        return _currentTarget.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}