using System;
using System.Collections.Generic;

public class Inventory
{
    public event Action OnInventoryChanged;
    public List<Item> Items { get; private set; } = new();
    public Weapon CurrentWeapon { get; private set; } = null;
    
    
    private Weapon GetFirstWeapon() => GetWeapons().Count > 0 ? GetWeapons()[0] : null;
    private List<Weapon> GetWeapons() => Items.FindAll(i => i is Weapon).ConvertAll(i => (Weapon)i);
    
    public void AddItem(Item item)
    {
        Items.Add(item);
        if (item is Weapon weapon && CurrentWeapon == null)
            CurrentWeapon = weapon;
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(Item item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            if (item is Weapon weapon && CurrentWeapon == weapon)
                CurrentWeapon = GetFirstWeapon();
            OnInventoryChanged?.Invoke();
        }
    }

    public void SetCurrentWeapon(Weapon weapon)
    {
        if (Items.Contains(weapon))
        {
            CurrentWeapon = weapon;
            OnInventoryChanged?.Invoke();
        }
    }

    public void SetCurrentWeapon(int index)
    {
        var weapons = GetWeapons();
        if (index >= 0 && index < weapons.Count)
        {
            CurrentWeapon = weapons[index];
            OnInventoryChanged?.Invoke();
        }
    }

    public void SwitchWeapon(int move)
    {
        var weapons = GetWeapons();
        if (weapons.Count == 0) return;
        int newIndex = (weapons.IndexOf(CurrentWeapon) + move) % weapons.Count;
        if (newIndex < 0) newIndex += weapons.Count;
        CurrentWeapon = weapons[newIndex];
        OnInventoryChanged?.Invoke();
    }
}