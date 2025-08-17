using UnityEngine;

namespace Entities.Player
{
    public class InventoryEntry
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }

        public InventoryEntry(Item item, int quantity = 0)
        {
            Item = item;
            Quantity = quantity;
        }

        public int Add(int quantity = 1)
        {
            int toAdd = Mathf.Min(quantity, Item.maxStackSize - Quantity);
            Quantity += toAdd;
            return quantity - toAdd;
        }
    }
}