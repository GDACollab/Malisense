using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=HXFoUGw7eKk
public class FloorNotesReader : MonoBehaviour
{

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public LayoutElement layoutElement;
    public int characterWrapLimit;

    private void Update(){
        int itemNameLength = itemName.text.Length;
        int itemDescriptionLength = itemDescription.text.Length;

        layoutElement.enabled = (itemNameLength > characterWrapLimit || itemDescriptionLength > characterWrapLimit) ? true : false;
    }
}

