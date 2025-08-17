using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private UIScoreIndicator scoreIndicator;
    [SerializeField] private UIBulletIndicator bulletIndicator;
    [SerializeField] private UIIndicator bloodLevel;
    [SerializeField] private UIIndicator staminaLevel;
    [SerializeField] private UIDamageOverlay UIDamageOverlay;
     
    /// <summary>
    /// </summary>
    /// <param name="t">[0..1]</param>
    public void SetHealth(float t)
    {
        bloodLevel.SetValue(t);
        if (t < bloodLevel.IndicatorCurrent)
        {
            UIDamageOverlay.AddDamage(bloodLevel.IndicatorCurrent - t);
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="t">[0..1]</param>
    public void SetStamina(float t)
    {
        staminaLevel.SetValue(t);
    } 

    public void AddScore(float score)
    {
        print(score);
        scoreIndicator.AddScore(score);
    }


    /// <summary>
    /// Set total Bullets
    /// </summary>
    /// <param name="totalBullets"></param>
    public void SetTotalBullets(int totalBullets) => bulletIndicator.SetTotalBullets(totalBullets);
    /// <summary>
    /// force set - when changing weapons
    /// </summary>
    public void SetCurrentBullets(int currentBullets) => bulletIndicator.SetCurrentBullets(currentBullets);

    /// <summary>
    /// Use this method during shooting bullets
    /// </summary>
    /// <param name="bulletsLeft"></param>
    public void UpdateBulletsLeft(int bulletsLeft) => bulletIndicator.UpdateBulletsLeft(bulletsLeft);


    #region debug
    public void ResetHealth()
    {
        print("Health reset");
        bloodLevel.SetValue(1);

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
    public void DebugShoot()
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
