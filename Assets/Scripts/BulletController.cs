using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Vector3 _targetPoint;
    private float _moveSpeed;
    private bool _initialized = false;

    public void Init(Vector3 target, float speed)
    {
        _initialized = true;
        _targetPoint = target;
        _moveSpeed = speed;

        transform.LookAt(_targetPoint);
    }

    void Update()
    {
        if (!_initialized) throw new CustomException("Пуля не инициализирована");

        float _step = _moveSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _targetPoint, _step);

        if (Vector3.Distance(transform.position, _targetPoint) < 0.05f)
        {
            Destroy(gameObject);
        }
    }
}