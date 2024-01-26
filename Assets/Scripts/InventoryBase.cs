using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    // TEMP DATABASE MAYBE???
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
    public class Inventory : ScriptableObject
    {
        [SerializeField] private int size;
        public List<InventorySlot> inventory;

        private void OnValidate()
        {
            // Set inventory size
            inventory.Clear();
            
            for (int i = 0; i < size; i++)
            {
                inventory.Add(new InventorySlot(null, 0));
            }
        }

        public bool AddItem(ItemBase item, int amount)
        {
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
