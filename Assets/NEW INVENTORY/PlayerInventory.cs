using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerInventory", menuName = "Inventory/PlayerInventory")]
public class PlayerInventory : ScriptableObject
{
    [SerializeField] private int size;
    public List<InventorySlot> inventory;
    
    public HeavyItem carriedObject;
    public Artifact artifact1;
    public Artifact artifact2;

    // happens when editor is updated
    private void OnValidate()
    {
        if (size <= 0)
        {
            return;
        }
        
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
            return false;
        }

        if (targetItem == null)
        {
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
        return false;
    }
    
    // getter
    public int GetSize()
    {
        return size;
    }
    
    // setter
    public void SetSize(int newSize)
    {
        size = newSize;
    }

    // we love overloading
    public bool RemoveItem(int targetSlot, int amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (targetSlot <= 0 || targetSlot > size - 1)
        {
            return false;
        }

        InventorySlot slot = inventory[targetSlot];

        if (slot.item == null)
        {
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
            if (!slot.item)
            {
                return i;
            }

            i++;
        }

        return -1;
    }
    
    public void ClearInventory(){
        inventory.Clear();
        
        for (int i = 0; i < size; i++)
        {
            inventory.Add(new InventorySlot(null, 0));
        }
    }
    
    public void ResetInventory(){
        ClearInventory();
        artifact2 = null;
    }
}
