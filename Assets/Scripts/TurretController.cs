using UnityEngine;

public class TurretController : MonoBehaviour
{
    // --- Настраиваемые параметры ---
    [Header("Настройка турели")]
    [SerializeField] private Transform turretTower; // Объект, который будет вращаться
    [SerializeField] private float searchRadius = 10f; // Радиус поиска врагов
    [SerializeField] private float rotationSpeed = 5f; // Скорость поворота турели
    [SerializeField] private float fireRate = 1f; // Скорость стрельбы (выстрелов в секунду)

    // --- Параметры стрельбы ---
    [Header("Настройка стрельбы")]
    [SerializeField] private GameObject bulletPrefab; // Префаб снаряда
    [SerializeField] private Transform firePoint; // Точка, откуда вылетают снаряды

    // --- Внутренние переменные ---
    private Transform _currentTarget;
    private float _nextFireTime;

    void Update()
    {
        // Если у нас нет цели, ищем её
        if (_currentTarget == null)
        {
            FindClosestZombie();
        }
        else
        {
            // Если цель есть, проверяем, находится ли она в радиусе и жива ли
            float distanceToTarget = Vector3.Distance(transform.position, _currentTarget.position);
            if (distanceToTarget > searchRadius)
            {
                // Если цель вышла из радиуса или умерла, сбрасываем её
                _currentTarget = null;
                return;
            }

            // Поворачиваемся к цели
            RotateTowardsTarget();

            // Если турель повернулась к цели и готова стрелять, стреляем
            if (CanShoot())
            {
                Shoot();
            }
        }
    }

    private void FindClosestZombie()
    {
        // Находим все коллайдеры в радиусе поиска.
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        float minDistance = float.MaxValue;
        Transform closestTarget = null;

        foreach (Collider collider in colliders)
        {
            // Пытаемся получить компонент ZombieCollisionScript.
            // GetComponent вернет null, если компонента нет.
            ZombieCollisionScript zombieScript = collider.GetComponent<ZombieCollisionScript>();

            // Проверяем, что компонент существует и isHead установлен в true.
            if (zombieScript != null && zombieScript.isHead)
            {
                // Теперь проверяем расстояние до этой "головы".
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
        // Горизонтальный поворот (по оси Y)
        Vector3 direction = _currentTarget.position - turretTower.position;
        // Игнорируем вертикальную составляющую для горизонтального поворота
        direction.y = 0;
        Quaternion horizontalRotation = Quaternion.LookRotation(direction);
        turretTower.rotation = Quaternion.Slerp(turretTower.rotation, horizontalRotation, Time.deltaTime * rotationSpeed);

        // Вертикальный поворот (по оси X)
        // Направление к цели, но без учета горизонтального вращения
        Vector3 directionForVertical = _currentTarget.position - turretTower.position;
        Quaternion verticalRotation = Quaternion.LookRotation(directionForVertical);

        // Ограничиваем вертикальный поворот, чтобы не смотреть в землю или небо
        // Quaternion.Euler создает поворот из углов Эйлера
        // Clamp - ограничивает значение
        Quaternion clampedVerticalRotation = verticalRotation;
        Vector3 eulerAngles = clampedVerticalRotation.eulerAngles;
        eulerAngles.x = ClampAngle(eulerAngles.x, -15f, 45f); // Примерные углы
        clampedVerticalRotation = Quaternion.Euler(eulerAngles);

        turretTower.rotation = Quaternion.Slerp(turretTower.rotation, clampedVerticalRotation, Time.deltaTime * rotationSpeed);
    }

    private bool CanShoot()
    {
        // Проверяем, наступило ли время следующего выстрела
        bool isTimeToShoot = Time.time >= _nextFireTime;
        
        // Проверяем, достаточно ли турель повернулась к цели
        Vector3 directionToTarget = (_currentTarget.position - turretTower.position).normalized;
        bool isFacingTarget = Vector3.Dot(turretTower.forward, directionToTarget) > 0.95f; // Почти повернулась

        return isTimeToShoot && isFacingTarget;
    }

    private void Shoot()
    {
        _nextFireTime = Time.time + 1f / fireRate; // Обновляем время следующего выстрела
        
        // Создаем снаряд
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        
        // Здесь можно добавить эффекты стрельбы (звук, вспышка, анимация)
        // Например: audioSource.PlayOneShot(shootSound);
        //            animator.SetTrigger("Shoot");
    }

    // Метод для визуализации радиуса поиска в редакторе Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}