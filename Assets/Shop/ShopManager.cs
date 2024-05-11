using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] public List<ItemBase> availableItems;
    [SerializeField] public PlayerInventory playerInventory;
    // ref to canvas
    private GameObject _canvas;

    private void Start()
    {
        _canvas = gameObject.transform.GetChild(0).gameObject;

        // adding event handling for each button
        for (int i = 0; i < 3; i++)
        {
            Button button = _canvas.transform.GetChild(i).Find("Button").GetComponent<Button>();
            
            // change inv to SO so it saves across scenes
            //button.onClick.AddListener(TryBuyItem());
        }
    }

    // move to ui
    bool TryBuyItem(int item)
    {
        int index = playerInventory.FindEmptySlot();
        
        if (index == -1)
        {
            return false;
        }
        
        // if player has enough money, add item
        playerInventory.AddItem(availableItems[item], 1);

        return true;
    }
}