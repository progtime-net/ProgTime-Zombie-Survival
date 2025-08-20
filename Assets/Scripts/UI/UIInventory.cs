using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIInventory : MonoBehaviour
{
    public VerticalLayoutGroup layoutGroup;
    [SerializeField] private List<string> inventoryItemPanelNames;
    [SerializeField] private List<UIInventoryItemSelectorElement> inventoryItemPanelElements;
    Dictionary<string, UIInventoryItemSelectorElement> _map;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _map = new Dictionary<string, UIInventoryItemSelectorElement>();
        for (int i = 0; i < inventoryItemPanelNames.Count; i++)
        {
            _map.Add(inventoryItemPanelNames[i], inventoryItemPanelElements[i]);
        }
    } 
      

    internal void UpdateState()
    {

        //PlayerController.LocalPlayer.Inventory.Items;
        print("UpdateState");

        //disable elements which are not presented
        foreach (var item in PlayerController.LocalPlayer.Inventory.Items)
        {
            string type = item.GetType().ToString();
            var el = _map[type];
            print(item.Quantity);
            print($"Updating State {PlayerController.LocalPlayer.Inventory.CurrentWeapon.name}");
            if (item.Quantity == 0)
            {
                el.HideElement();
            }
            else
            {
                el.ShowElement();
                if (type == PlayerController.LocalPlayer.Inventory.CurrentWeapon.GetType().ToString())
                    el.Select();                
                else
                    el.Deselect();
            }
        };

    }
}

