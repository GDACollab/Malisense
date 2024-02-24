using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CanvasGroup))]
public class Hotbar : MonoBehaviour
{
    public InventoryBase inventory;
    public RectTransform selector;

    [Tooltip("The time it takes for the selector to move to another slot, measured in seconds.")]
    public float selectorMoveTime = 0.25f;

    [Tooltip("The currently selected inventory slot, starting from 0.")]
    public int selectedSlot;

    [Tooltip("Opacity of the hotbar when a heavy item is carried.")]
    public float lockedAlpha = 0.15f;

    private float selectorVel;
    private HotbarSlot artifactSlot;
    private List<HotbarSlot> inventorySlots = new();
    private CanvasGroup group;

    private void Start()
    {
        var allSlots = GetComponentsInChildren<HotbarSlot>();

        foreach(var slot in allSlots)
        {
            if(slot.slotNumber == -1)
            {
                artifactSlot = slot;
            }
            else if(slot.slotNumber > -1)
            {
                while (inventorySlots.Count <= slot.slotNumber)
                    inventorySlots.Add(null);

                inventorySlots[slot.slotNumber] = slot;
            }
        }
    }

    private void Update()
    {
        // Update alpha
        if (!group) group = GetComponent<CanvasGroup>();
        group.alpha = inventory.carriedObject ? lockedAlpha : 1f;

        // Move selector
        var slot = inventorySlots[selectedSlot];
        if (slot)
        {
            var rect = (RectTransform)slot.transform;
            var pos = selector.anchoredPosition;
            pos.x = Mathf.SmoothDamp(pos.x, rect.anchoredPosition.x, ref selectorVel, selectorMoveTime);
            selector.anchoredPosition = pos;
        }
    }

    private void SwitchSlot(int index)
    {
        selectedSlot = index;
    }

    public void OnSlot1() => SwitchSlot(0);
    public void OnSlot2() => SwitchSlot(1);
    public void OnSlot3() => SwitchSlot(2);
    
    public void OnScroll(InputValue value)
    {
        int scroll = (int)Mathf.Sign(value.Get<float>());
        SwitchSlot((selectedSlot + scroll + inventorySlots.Count) % inventorySlots.Count);
    }

    public void OnUseItem()
    {
        if (inventory.carriedObject) return;

        var slot = inventory.inventory[selectedSlot];
        if (slot.item && slot.amount > 0)
        {
            // Activate item
            var item = slot.item;
            Debug.Log($"Item '{item.ItemID}' activated");

            Use(item);

            // Decrease item count, removing if necessary
            if(--slot.amount == 0)
            {
                slot.item = null;
            }
        }
    }

    public void OnUseArtifact()
    {
        if (inventory.carriedObject) return;
        Debug.Log("Artifact activated");
        Use(inventory.currentArtifact);
    }

    private void Use(ItemBase item)
    {
        if (item && item.prefab)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var effect = Instantiate(item.prefab);

            effect.transform.SetParent(player.transform.parent, false);
            effect.transform.position = player.transform.position;
        }
    }
}
