using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
    public class Inventory : ScriptableObject
    {
        [SerializeField] private int size;
        public List<InventorySlot> inventory;
        
        public HeavyItem carriedObject;

        private void OnValidate()
        {
            // Set inventory size
            inventory.Clear();
            
            for (int i = 0; i < size; i++)
            {
                inventory.Add(new InventorySlot(null, 0));
            }
        }

        // return success
        public bool AddItem(ItemBase item, int amount)
        {
            // Prevents players from picking up items
            if (carriedObject)
            {
                return false;
            }
            
            // if item IS in inv, add it to the slot.
            int targetSlot = HasItem(item);
            
            if (targetSlot != -1)
            {
                // Add to current item
                inventory[targetSlot].amount += amount;
                return true;
            }
            
            // if item is not in current inv, check for empty slot
            targetSlot = FindEmptySlot();
            print(targetSlot);
                
            // if Empty slot found
            if (targetSlot != -1)
            {
                // Add item
                inventory[targetSlot].item = item;
                inventory[targetSlot].amount = amount;
                return true;
            }
            else
            {
                // No space in inv, return unsuccessful
                return false;
            }
        }

        public bool RemoveItem(ItemBase targetItem, int amount)
        {
            if (amount <= 0)
            {
                print("Amount Less than 1");
                return false;
            }

            if (targetItem == null)
            {
                print("Item is null");
                return false;
            }
            
            foreach (InventorySlot slot in inventory)
            {
                if (slot.item.Equals(targetItem))
                {
                    // Item found
                    slot.amount -= Math.Max(slot.amount - amount, 0);

                    if (slot.amount <= 0)
                    {
                        // Clear slot
                        slot.item = null;
                    }
                    
                    return true;
                }
            }

            // Item not found
            print("Item " + targetItem.name + " Not Found");
            return false;
        }

        // we love overloading
        public bool RemoveItem(int targetSlot, int amount)
        {
            if (amount <= 0)
            {
                print("Amount Less than 1");
                return false;
            }

            if (targetSlot <= 0 || targetSlot > size - 1)
            {
                print("Target slot out of array bounds");
                return false;
            }

            InventorySlot slot = inventory[targetSlot];

            if (slot.item == null)
            {
                print("Item at slot: " + targetSlot + ", is null");
                return false;
            }
            
            // Item found
            slot.amount -= Math.Max(slot.amount - amount, 0);

            if (slot.amount <= 0)
            {
                // Clear slot
                slot.item = null;
            }
                    
            return true;
        }
        
        // attempts to return the inv slot that has the specific item
        public int HasItem(ItemBase targetItem)
        {
            var i = 0;
            foreach (InventorySlot slot in inventory)
            {
                if (slot.item == targetItem)
                {
                    return i;
                }

                i++;
            }
            // if no item found, return -1.
            return -1;
        }

        // Returns index if an empty slot is found
        public int FindEmptySlot()
        {
            int i = 0;
            foreach (InventorySlot slot in inventory)
            {
                if (slot.item == null)
                {
                    return i;
                }

                i++;
            }

            return -1;
        }
    }

    // could also be a struct prolly
    [System.Serializable]
    public class InventorySlot
    {
        public ItemBase item = null;
        public int amount = 0;

        public InventorySlot(ItemBase item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
    
}
