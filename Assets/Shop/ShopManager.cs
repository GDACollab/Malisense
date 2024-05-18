using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] public List<ItemBase> availableItems;
    [SerializeField] public PlayerInventory playerInventory;
    [SerializeField] public GlobalTeapot teapot;
    // ref to canvas
    private GameObject _canvas;

    private void Start()
    {
        _canvas = gameObject.transform.GetChild(0).gameObject;

        // adding event handling for each button
        for (int i = 0; i < 3; i++)
        {
            Button button = _canvas.transform.GetChild(i).Find("Button").GetComponent<Button>();
            
            var i1 = i;
            print(i1);
            button.onClick.AddListener(() => TryBuyItem(i1));
        }
    }

    // move to ui
    void TryBuyItem(int item)
    {
        int index = playerInventory.FindEmptySlot();

        if (index == -1)
        {
            return;
        }

        // if player has enough money, add item
        if (teapot.numStoreCredits >= (int) availableItems[item].cost)
        {
            teapot.numStoreCredits -= (int)availableItems[item].cost;
            playerInventory.AddItem(availableItems[item], 1);
        }
    }
}