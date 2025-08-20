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


        if (Player == PlayerController.LocalPlayer)
        {
            //hide gunDisplayed
            foreach (var item in _weaponLogicDisplayedInstancesList) 
                item.layer = MaskToLayer(hiddenGunLayer); 
            foreach (var item in _weaponDisplayedInstancesList) 
                item.layer = MaskToLayer(hiddenGunLayer); 

            // Убрать слой из отрисовки
            //int layer = MaskToLayer(hiddenGunLayer);
            //Player.cam.GetComponent<Camera>().cullingMask &= ~(1 << layer); //надо 1 раз при создании главного игрока делать
        }
        else
        {

            foreach (var item in _weaponLogicDisplayedInstancesList)
                item.layer = MaskToLayer(hiddenGunLayer);
            foreach (var item in _weaponDisplayedInstancesList)
                item.layer = MaskToLayer(hiddenGunLayer);

        }

        gunLogicDisplayed = _weaponDisplayedInstancesList[0];
        gunDisplayed = _weaponDisplayedInstancesList[0];
        SelectGun();

    }
    int MaskToLayer(LayerMask m)
    {
        int v = m.value;
        if (v == 0 || (v & (v - 1)) != 0) return -1; // not exactly one layer
        int i = 0; while (v > 1) { v >>= 1; i++; }
        return i; // 0..31
    }
    public void SelectGun()
    {
        int _newWeaponId = UnityEngine.Random.Range(0, _weaponDisplayedInstancesList.Count);

        print("Gun selected: " + _newWeaponId);
        print("1: " + hiddenGunLayer);
        print("1-1: " + MaskToLayer(hiddenGunLayer));
        print("2: " + shownGunLayer);
        print("2-2: " + MaskToLayer(shownGunLayer));
        if (Player == PlayerController.LocalPlayer)
        {
            gunLogicDisplayed.layer = MaskToLayer(hiddenGunLayer);
            gunLogicDisplayed = _weaponDisplayedInstancesList[_newWeaponId];
            gunLogicDisplayed.layer = MaskToLayer(shownGunLayer);
        }
        else
        {

            gunDisplayed.layer = MaskToLayer(hiddenGunLayer);
            gunDisplayed = _weaponDisplayedInstancesList[_newWeaponId];
            gunDisplayed.layer = MaskToLayer(shownGunLayer);
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
        SelectGun();
    }

    internal void Attack()
    {

    }

#endif

}
