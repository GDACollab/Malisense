using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    [Tooltip("The slot index, starting at 0, or -1 for the artifact.")]
    public int slotNumber;

    [Tooltip("The image to replace with the current item.")]
    public Image itemIcon;

    [Tooltip("The label to update with the current number of items held.")]
    public TextMeshProUGUI count;

    private Hotbar hotbar;

    private ItemBase Item
    {
        get
        {
            if (!hotbar) hotbar = GetComponentInParent<Hotbar>();
            if (!hotbar) return null;

            if(slotNumber == -1)
            {
                return hotbar.inventory.currentArtifact;
            }
            else
            {
                if (slotNumber < 0 || slotNumber >= hotbar.inventory.inventory.Count) return null;

                return hotbar.inventory.inventory[slotNumber].item;
            }
        }
    }

    private int Amount
    {
        get
        {
            if (!hotbar) hotbar = GetComponentInParent<Hotbar>();
            if (!hotbar) return 0;

            if (slotNumber == -1)
            {
                return hotbar.inventory.currentArtifact != null ? 1 : 0;
            }
            else
            {
                if (slotNumber < 0 || slotNumber >= hotbar.inventory.inventory.Count) return 0;

                return hotbar.inventory.inventory[slotNumber].amount;
            }
        }
    }

    public void Update()
    {
        // Update image
        if (Item == null)
        {
            itemIcon.gameObject.SetActive(false);
        }
        else
        {
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = Item.thumbnail;
        }

        // Update count
        if (Amount <= 1)
        {
            count.gameObject.SetActive(false);
        }
        else
        {
            count.gameObject.SetActive(true);
            count.text = Amount.ToString();
        }
    }
}
