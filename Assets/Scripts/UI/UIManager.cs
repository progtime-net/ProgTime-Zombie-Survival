using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private UIScoreIndicator ScoreIndicator;
    [SerializeField] private UIBulletIndicator BulletIndicator;
    [SerializeField] private UIIndicator BloodLevel;
    [SerializeField] private UIIndicator EnergyLevel;
    [SerializeField] private UIDamageOverlay UIDamageOverlay;

    void Start()
    {
    }
     
    void Update()
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="t">[0..1]</param>
    public void SetHealth(float t)
    {
        BloodLevel.SetValue(t);
        if (t < BloodLevel.IndicatorCurrent)
        {
            UIDamageOverlay.AddDamage(BloodLevel.IndicatorCurrent - t);
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="t">[0..1]</param>
    public void SetEnergy(float t)
    {
        EnergyLevel.SetValue(t);
    }

    public void AddScore(float score)
    {
        print(score);
        ScoreIndicator.AddScore(score);
    }


    /// <summary>
    /// Set total Bullets
    /// </summary>
    /// <param name="totalBullets"></param>
    public void SetTotalBullets(int totalBullets) => BulletIndicator.SetTotalBullets(totalBullets);
    /// <summary>
    /// force set - when changing weapons
    /// </summary>
    public void SetCurrentBullets(int currentBullets) => BulletIndicator.SetCurrentBullets(currentBullets);

    /// <summary>
    /// Use this method during shooting bullets
    /// </summary>
    /// <param name="bulletsLeft"></param>
    public void UpdateBulletsLeft(int bulletsLeft) => BulletIndicator.UpdateBulletsLeft(bulletsLeft);


    #region debug
    public void ResetHealth()
    {
        print("Health reset");
        BloodLevel.SetValue(1);

    }
    private float _debHeath = 1;
    public void MinusHealth()
    {
        _debHeath -= 0.1f;
        SetHealth(_debHeath);
    }
    private int _bulLeft = 120;
    public void DebugSetBullets()
    {
        SetTotalBullets(_bulLeft);

    }
    public void Shoot()
    {
        _bulLeft -= 1;
        UpdateBulletsLeft(_bulLeft);
    }

    #endregion

    public void Remuse()
    {
        pauseMenuPanel.SetActive(false);
    }

    public void OpenSettig()
    {
        
    }

    public void Exit()
    {
        
    }

}
