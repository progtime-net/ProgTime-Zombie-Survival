using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject pauseMenuPanel;
    
    public UIIndicator BloodLevel;
    public UIIndicator EnergyLevel;
    public UIDamageOverlay UIDamageOverlay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
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
