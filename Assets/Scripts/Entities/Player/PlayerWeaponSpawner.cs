using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerWeaponSpawner : MonoBehaviour
{

    [SerializeField] private List<Weapon> weaponPrefabsList;
    [SerializeField] private GameObject gunDisplayerHolder;
    [SerializeField] private GameObject gunLogicDisplayer;
    [SerializeField] private PlayerController Player;
    [SerializeField] private LayerMask hiddenGunLayer;
    [SerializeField] private LayerMask shownGunLayer;
    private List<GameObject> _weaponDisplayedInstancesList = new List<GameObject>();
    private List<GameObject> _weaponLogicDisplayedInstancesList = new List<GameObject>();

    private GameObject gunDisplayed;
    private GameObject gunLogicDisplayed;
    void Start()
    {
        //foreach (var item in weaponPrefabsList)
        //{
        //    print(item);
        //    print(item.gameObject);
        //    _weaponDisplayedInstancesList.Add(Instantiate(item.gameObject, gunDisplayerHolder.transform));
        //    _weaponLogicDisplayedInstancesList.Add(Instantiate(item.gameObject, gunLogicDisplayer.transform));
        //}
        foreach (Transform item in gunDisplayerHolder.transform)
            _weaponDisplayedInstancesList.Add(item.gameObject);

        foreach (Transform item in gunLogicDisplayer.transform)
            _weaponLogicDisplayedInstancesList.Add(item.gameObject);


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
            Player.Inventory.AddItem(item.GetComponent<Weapon>(), 1);

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
        
        if (Player == PlayerController.LocalPlayer)
        {
            print("Local ammo change");
            RecursivelySetLayer(gunLogicDisplayed, hiddenGunLayer);
            gunLogicDisplayed = _weaponLogicDisplayedInstancesList[_index];
            RecursivelySetLayer(gunLogicDisplayed, shownGunLayer); 
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
#if UNITY_EDITOR
    // Editor helpers (works in play mode). Uses server path if available.
    [ContextMenu("Randomize Weapon")]
    void ContextRandomizeWeapon()
    {
        SelectGunRandomly();
    }

    internal void Attack()
    {
        
        (gunLogicDisplayed.GetComponent<Weapon>()).Attack();
    }

#endif

}
