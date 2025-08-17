using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private Animator inventoryAnimator;
    public VerticalLayoutGroup layoutGroup;
    private List<UIInventoryItemSelectorElement> inventoryItems;
    [SerializeField] private SerializableDictionary<ScriptTypeRef, Sprite> itemTextures;
    private Dictionary<string, Sprite> _itemTexturesHash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryItems = new List<UIInventoryItemSelectorElement>();
        _itemTexturesHash = itemTextures.ToDictionary(k => k.Key.Type.ToString(), v => v.Value);
    }
    public void SetItems(Inventory inventory)
    {
        while (inventoryItems.Count > 0)
        {
            Destroy(inventoryItems[0]);
        }
        foreach (var item in inventory.Items)
        {
            inventoryItems.Add(Instantiate(new UIInventoryItemSelectorElement(),
                layoutGroup.transform));
            inventoryItems.Last().SetImage(_itemTexturesHash[item.GetType().ToString()]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenInventory()
    {
        inventoryAnimator.Play("InventoryAppearing");
    }

    public void CloseInventory()
    {
        inventoryAnimator.Play("InventoryDisappearing");
    }
}

