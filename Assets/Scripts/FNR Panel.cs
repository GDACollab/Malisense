using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FNRPanel : MonoBehaviour
{
    [Header("ExitButton")]
    [SerializeField]
    private Transform _exitButtonArea;
    [SerializeField]
    private Button _exitButton;

    [Header("Item Name")]
    [SerializeField]
    private Transform _itemNameArea;
    [SerializeField]
    private TextMeshProUGUI _itemName;

    [Header("Item Description")]
    [SerializeField]
    private Transform _descriptionArea;
    [SerializeField]
    private TextMeshProUGUI _itemText;

    private Action onExitAction;

    public void exitPanel() {
        onExitAction?.Invoke();
        _exitButtonArea.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(false);
        _itemNameArea.gameObject.SetActive(false);
        _itemName.gameObject.SetActive(false);
        _descriptionArea.gameObject.SetActive(false);
        _itemText.gameObject.SetActive(false);
    }

    public void showPanel() {
        _exitButtonArea.gameObject.SetActive(true);
        _exitButton.gameObject.SetActive(true);
        _itemNameArea.gameObject.SetActive(true);
        _itemName.gameObject.SetActive(false);
        _descriptionArea.gameObject.SetActive(true);
        _itemText.gameObject.SetActive(true);

        //onExitAction = exitButton;
    }
}
