using UnityEngine;
public class Item : MonoBehaviour
{
    [SerializeField] protected string itemName = "Default Item";  
    [SerializeField] protected int maxStackSize = 1;
    
    public string ItemName => itemName;
    public int MaxStackSize => maxStackSize;
}