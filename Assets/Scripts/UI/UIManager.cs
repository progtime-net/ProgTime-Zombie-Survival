using System;
using Mirror;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private UIScoreIndicator scoreIndicator;
    [SerializeField] private UIBulletIndicator bulletIndicator;
    [SerializeField] private UITimeIndicator timeIndicator;
    [SerializeField] private UIIndicator bloodLevel;
    [SerializeField] private UIIndicator staminaLevel;
    [SerializeField] private UIDamageOverlay UIDamageOverlay;
    [SerializeField] private UIAnnouncer announcer; 
    [SerializeField] private UIInventory UIInventory;
    [SerializeField] private DebugInventoryHolder inventoryHolder;
      
    [SerializeField] private Animator inventoryAnimator;
    
    private InputSystem _controls;
    private bool _isInventoryOpen = false;

    void Awake()
    {
        PlayerController.OnPlayerSpawned += _ => RegisterEvents();
    }

    private void Start()
    {
        Debug.Log("Starting UI Manager");
        _controls = new InputSystem();
        _controls.UI.InventoryOpen.performed += _ => ChangeInventoryState();
        
        _controls.Enable();
        
    }

    private void RegisterEvents()
    {
        if (PlayerController.LocalPlayer == null || WaveManager.Instance == null)
        {
            return;
        }
        PlayerController.LocalPlayer.OnUpdateHealth += SetHealth;
        PlayerController.LocalPlayer.OnUpdateStamina += SetStamina;
        PlayerController.LocalPlayer.weaponSpawner.OnWeaponSelected += UpdateWeaponIndicator;
        UpdateWeaponIndicator(null, PlayerController.LocalPlayer.weaponSpawner.gunLogicDisplayed.GetComponent<Weapon>());
        //PlayerController.LocalPlayer.Inventory.OnWeaponChanged += UIInventory.UpdateState(); 
        WaveManager.Instance.OnWaveStateChanged += WaveStateChanged;
    }
    
    private void UpdateWeaponIndicator(Weapon oldWeapon, Weapon newWeapon)
    {
        if (oldWeapon is Gun oldGun)
        {
            oldGun.OnAmmoChanged -= UpdateBullets;
            bulletIndicator.enabled = false;
        }
        if (newWeapon is Gun newGun)
        {
            bulletIndicator.enabled = true;
            newGun.OnAmmoChanged += UpdateBullets;
            bulletIndicator.SetTotalBullets(newGun.TotalAmmo);
            bulletIndicator.SetCurrentBullets(newGun.CurrentAmmo);   
        }
    }
    
    private void WaveStateChanged(int wave, bool state)
    {
        if (state)
        {
            Debug.Log("WaveStateChanged");
            Announce($"Волна {wave} началась!");
        }
        else
        {
            Announce($"Волна {wave} успешно зачищена!");
        } 
    }

    private void ChangeInventoryState()
    {
        Debug.Log("Changing inventory state");
        if (_isInventoryOpen) CloseInventory();
        else OpenInventory();
    }
     
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


    private void UpdateBullets(int currentBullets, int totalBullets)
    {
        bulletIndicator.SetTotalBullets(totalBullets);
        UpdateBulletsLeft(currentBullets);
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
    /// <summary>
    /// Seconds of the day left
    /// </summary>
    /// <param name="dayLength"></param> 

    public void OpenInventory()
    {
        //UIInventory.OpenInventory();
        UIInventory.UpdateState();
        _isInventoryOpen = true;
        inventoryAnimator.SetBool("isInventoryOpen", _isInventoryOpen);
    }
    public void CloseInventory()
    {
        //UIInventory.CloseInventory();
        _isInventoryOpen = false;
        inventoryAnimator.SetBool("isInventoryOpen", _isInventoryOpen); 
    }

    public void Announce(string text)
    {
        announcer.Announce(text);
    }

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
}
