using System;
using System.Collections.Generic;
using Entities.Player;
using UnityEngine.Events;

[Serializable]
public class Inventory
{
    public UnityEvent OnInventoryChanged { get; private set; } = new UnityEvent();
    public event Action<int> OnWeaponChanged;
    public List<InventoryEntry> Items { get; private set; } = new();
    public Weapon CurrentWeapon { get; private set; } = null;
    
    
    private Weapon GetFirstWeapon() => GetWeapons().Count > 0 ? GetWeapons()[0] : null;
    private List<Weapon> GetWeapons() => Items.FindAll(i => i.Item is Weapon).ConvertAll(i => (Weapon)i.Item);

    
    public void AddItem(Item item, int quantity = 1) 
    {
        var existingEntry = Items.Find(i => i.Item == item);
        if (existingEntry != null)
        {
            quantity = existingEntry.Add(quantity);
        }
        while (quantity > 0)
        {
            InventoryEntry newEntry = new(item);
            quantity = newEntry.Add(quantity);
            Items.Add(newEntry);
        }
        OnInventoryChanged?.Invoke();
    }

    public bool RemoveItem(Item item)
    {
        var existingEntry = Items.FindLast(i => i.Item == item);
        if (existingEntry == null) return false;
        existingEntry.Quantity--;
        if (existingEntry.Quantity <= 0)
        {
            Items.Remove(existingEntry);
        }
        OnInventoryChanged?.Invoke();
        return true;
    }
    
    /// <summary>
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    public int GetCurrentWeaponId(Weapon weapon)
    {
        var weapons = GetWeapons();
        return weapons.IndexOf(weapon);
    }
    
    public bool SetCurrentWeapon(int index)
    {
        var weapons = GetWeapons();
        if (index >= 0 && index < weapons.Count)
        {
            CurrentWeapon = weapons[index];
            OnWeaponChanged?.Invoke(index);
            return true;
        }
        return false;
    }

    public void SwitchWeapon(int move)
    {
        var weapons = GetWeapons();
        if (weapons.Count == 0) return;
        int newIndex = (weapons.IndexOf(CurrentWeapon) + move) % weapons.Count;
        if (newIndex < 0) newIndex += weapons.Count;
        CurrentWeapon = weapons[newIndex];
        OnWeaponChanged?.Invoke(newIndex);
    }
}