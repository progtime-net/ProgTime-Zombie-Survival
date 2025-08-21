using TMPro;
using UnityEngine;

public class UIBulletIndicator : MonoBehaviour
{
    private const string InfinitySymbol = "\u221E";

    private float _currentBullets = 0;
    private int _targetBullets;
    private int _totalBullets;
    [SerializeField] private float changeBulletSmoothness = 0.2f;
    [SerializeField] private TextMeshProUGUI bulletText;
    private string _baseBulletText;

    void Start()
    {
        _baseBulletText = bulletText.text;
        UpdateText();
    }

    public void SetTotalBullets(int totalBullets)
    {
        _totalBullets = totalBullets;
        UpdateText();
    }

    /// <summary>
    /// force set - when changing weapons
    /// </summary>
    public void SetCurrentBullets(int currentBullets)
    {
        _currentBullets = currentBullets;
        _targetBullets = currentBullets;
        UpdateText();
    }

    public void UpdateBulletsLeft(int bulletsLeft)
    {
        _targetBullets = bulletsLeft;
        if (_targetBullets < 0)
        {
            // lock display to infinity immediately
            _currentBullets = -1f;
            UpdateText();
        }
    }

    void Update()
    {
        // Infinite current bullets: show ∞ and skip smoothing
        if (_targetBullets < 0 || _currentBullets < 0)
        {
            _currentBullets = -1f;
            UpdateText();
            return;
        }

        if (_targetBullets == (int)_currentBullets)
        {
            UpdateText();
            return;
        }

        _currentBullets = (int)Mathf.Lerp(_currentBullets, _targetBullets, changeBulletSmoothness);

        if (Mathf.Abs(_currentBullets - _targetBullets) < 5)
        {
            _currentBullets = (int)Mathf.Lerp(_currentBullets, _targetBullets, 0.95f);
        }

        if (Mathf.Abs(_currentBullets - _targetBullets) < 2)
        {
            _currentBullets = _targetBullets;
        }

        UpdateText();
    }

    private void UpdateText()
    {
        try
        {
            string currentDisplay = (_targetBullets < 0 || _currentBullets < 0)
                ? "inf"
                : ((int)_currentBullets).ToString();

            string totalDisplay = (_totalBullets < 0)
                ? "inf"
                : _totalBullets.ToString();

            bulletText.text = _baseBulletText
                .Replace("XXX", currentDisplay)
                .Replace("YYY", totalDisplay);
        }
        catch (System.Exception)
        {}
    }
}