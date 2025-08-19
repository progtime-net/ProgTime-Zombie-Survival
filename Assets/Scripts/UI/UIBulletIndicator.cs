using TMPro;
using UnityEngine;

public class UIBulletIndicator : MonoBehaviour
{
    private float _currentBullets = 0;
    private int _targetBullets;
    private int _totalBullets;
    [SerializeField] private float changeBulletSmoothness = 0.2f;
    [SerializeField] private TextMeshProUGUI bulletText;
    private string _baseBulletText;

    void Start()
    {
        _baseBulletText = bulletText.text;
        bulletText.text = bulletText.text.Replace("XXX", _currentBullets.ToString()).Replace("YYY", _totalBullets.ToString());
    }
    public void SetTotalBullets(int totalBullets)
    {
        _totalBullets = totalBullets;
        bulletText.text = _baseBulletText.Replace("XXX", _currentBullets.ToString()).Replace("YYY", _totalBullets.ToString());
    }
    /// <summary>
    /// force set - when changing weapons
    /// </summary>
    public void SetCurrentBullets(int currentBullets)
    {
        _currentBullets = currentBullets;
        _targetBullets = currentBullets;
        bulletText.text = _baseBulletText.Replace("XXX", _currentBullets.ToString()).Replace("YYY", _totalBullets.ToString());
    }

    public void UpdateBulletsLeft(int bulletsLeft)
    {
        _targetBullets = bulletsLeft;
    }

     
    void Update()
    { 
        if (_targetBullets == _currentBullets)
        {
            bulletText.text = _baseBulletText.Replace("XXX", _targetBullets.ToString()).Replace("YYY", _totalBullets.ToString());
            return;
        }

        _currentBullets = (int)Mathf.Lerp(_currentBullets, _targetBullets, changeBulletSmoothness);
        if (Mathf.Abs(_currentBullets - _targetBullets) < 5)
        {
            _currentBullets = (int)Mathf.Lerp(_currentBullets, _targetBullets, 0.95f);

        }
        if (Mathf.Abs(_currentBullets - _targetBullets) < 0.9)
        {
            _currentBullets = _targetBullets;
        }
        bulletText.text = _baseBulletText.Replace("XXX", _currentBullets.ToString()).Replace("YYY", _totalBullets.ToString());

    }

}
