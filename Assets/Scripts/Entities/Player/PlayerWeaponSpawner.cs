using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class PlayerWeaponSpawner : NetworkBehaviour
{
    public event Action<Weapon?, Weapon?> OnWeaponSelected;
    
    [SerializeField] private List<Weapon> weaponPrefabsList;
    [SerializeField] private GameObject gunDisplayerHolder;
    [SerializeField] private GameObject gunLogicDisplayer;
    [SerializeField] private LayerMask hiddenGunLayer;
    [SerializeField] private LayerMask shownGunLayer;
    private List<GameObject> _weaponDisplayedInstancesList = new List<GameObject>();
    private List<GameObject> _weaponLogicDisplayedInstancesList = new List<GameObject>();

    private GameObject gunDisplayed;
    public GameObject gunLogicDisplayed;
    private bool _isStart = false;
    public void Start()
    {
        if (_isStart) return;
        _isStart = true;
        //foreach (var item in weaponPrefabsList)
        //{
        //    print(item);
        //    print(item.gameObject);
        //    _weaponDisplayedInstancesList.Add(Instantiate(item.gameObject, gunDisplayerHolder.transform));
        //    _weaponLogicDisplayedInstancesList.Add(Instantiate(item.gameObject, gunLogicDisplayer.transform));
        //}
        Debug.Log("Weapon spawner started");
        foreach (Transform item in gunDisplayerHolder.transform)
            _weaponDisplayedInstancesList.Add(item.gameObject);

        foreach (Transform item in gunLogicDisplayer.transform)
        {
            _weaponLogicDisplayedInstancesList.Add(item.gameObject);
            Debug.Log(item.name);
        }


        foreach (var item in _weaponLogicDisplayedInstancesList)
            RecursivelySetLayer(item, hiddenGunLayer);
        foreach (var item in _weaponDisplayedInstancesList)
            RecursivelySetLayer(item, hiddenGunLayer);

        gunLogicDisplayed = _weaponDisplayedInstancesList[0];
        gunDisplayed = _weaponDisplayedInstancesList[0];
    }
    public void SetupBindings()
    {
        foreach (var item in _weaponLogicDisplayedInstancesList)
            PlayerController.LocalPlayer.Inventory.AddItem(item.GetComponent<Weapon>(), 1);

        SelectGun(0);
    }
    int MaskToLayer(LayerMask m)
    {
        int v = m.value;
        if (v == 0 || (v & (v - 1)) != 0) return -1; // not exactly one layer
        int i = 0; while (v > 1) { v >>= 1; i++; }
        return i; // 0..31
    }
    private void RecursivelySetLayer(GameObject gameObject, LayerMask layerMask)
    {
        foreach (var item in gameObject.GetComponentsInChildren<Transform>())
        {
            item.gameObject.layer = MaskToLayer(layerMask);
        }
    }
    public void SelectGunRandomly()
    {
        int _newWeaponId = UnityEngine.Random.Range(0, _weaponDisplayedInstancesList.Count);
        SelectGun(_newWeaponId);
    }
    public void SelectGun(int _index)
    {
        if (_index < 0 || _index >= _weaponDisplayedInstancesList.Count)
        {
            Debug.LogError("Invalid weapon index: " + _index + " some: " + isLocalPlayer);
            return;
        }

        if (isLocalPlayer)
        {
            print("Local ammo change");
            RecursivelySetLayer(gunLogicDisplayed, hiddenGunLayer);
            var oldGun = gunLogicDisplayed.GetComponent<Weapon>();
            gunLogicDisplayed = _weaponLogicDisplayedInstancesList[_index];
            RecursivelySetLayer(gunLogicDisplayed, shownGunLayer); 
            var newGun = gunLogicDisplayed.GetComponent<Weapon>();
            OnWeaponSelected?.Invoke(oldGun, newGun);
        }
        else
        { 
            print("Not Local ammo change");
            RecursivelySetLayer(gunDisplayed, hiddenGunLayer);
            gunDisplayed = _weaponDisplayedInstancesList[_index];
            RecursivelySetLayer(gunDisplayed, shownGunLayer);
        }

    }
    public void SelectGun(Weapon weapon)
    {
        var curGun = weaponPrefabsList.Where(x=>x.GetComponent<Weapon>().name == weapon.name).First();
         
    }

    internal void Attack()
        {

            (gunLogicDisplayed.GetComponent<Weapon>()).Attack();
        }

#if UNITY_EDITOR
    // Editor helpers (works in play mode). Uses server path if available.
    [ContextMenu("Randomize Weapon")]
    void ContextRandomizeWeapon()
    {
        SelectGunRandomly();
    }



#endif

}
