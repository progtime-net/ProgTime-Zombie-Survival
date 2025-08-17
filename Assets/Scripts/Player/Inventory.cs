using System.Collections.Generic;

public class Inventory
{
    private List<Weapon> Weapons { get; set; } = new();
    private Weapon CurrentWeapon { get; set; } = null;


    public void AddWeapon(Weapon weapon)
    {
        Weapons.Add(weapon);
    }
    
    public void RemoveWeapon(Weapon weapon)
    {
        if (Weapons.Contains(weapon))
        {
            Weapons.Remove(weapon);
            if (CurrentWeapon == weapon)
            {
                CurrentWeapon = Weapons.Count > 0 ? Weapons[0] : null;
            }
        }
    }
    
    public void SetCurrentWeapon(Weapon weapon)
    {
        if (Weapons.Contains(weapon))
        {
            CurrentWeapon = weapon;
        }
    }
    
    public void SetCurrentWeapon(int index)
    {
        if (index >= 0 && index < Weapons.Count)
        {
            CurrentWeapon = Weapons[index];
        }
    }

    
    public void SwitchWeapon(int move)
    {
        if (Weapons.Count == 0) return;
        int newIndex = ( Weapons.IndexOf(CurrentWeapon) + move) % Weapons.Count;
        if (newIndex < 0) newIndex += Weapons.Count;
        CurrentWeapon = Weapons[newIndex];
    }

}