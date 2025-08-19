using UnityEngine;

public class GunMovement : MonoBehaviour
{
    [SerializeField] private float lerpSpeed = 2f;

    private Transform _targetT;

    private Transform _t;

    void Awake()
    {
        _t = transform;
    }

    void Update()
    {
        if (_targetT == null)
        {
            if (PlayerController.LocalPlayer != null)
                _targetT = PlayerController.LocalPlayer.transform;
        }
    }

    void LateUpdate()
    {
        if (_targetT == null) return;

        _t.position = _targetT.position; // Задаем позицию объекта t
        // плавно (с интерполяцией) вращает объект t к целевому вращению targetT.rotation с заданной скоростью lerpSpeed
        _t.rotation = Quaternion.Lerp(_t.rotation, _targetT.rotation, lerpSpeed * Time.deltaTime);
    }
}
