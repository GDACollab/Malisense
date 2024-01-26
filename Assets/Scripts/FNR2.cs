using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FNRPanel2 : MonoBehaviour
{
    [Header("ExitButton")]
    [SerializeField]
    private Transform _exitButtonArea;
    [SerializeField]
    private Button _exitButton;

    [Header("Lore Panel")]
    [SerializeField]
    private Transform _LorePanelArea;
    [SerializeField]
    private TextMeshProUGUI _LoreText;


    private Action onExitAction;

    public void exitPanel()
    {
        onExitAction?.Invoke();
        _exitButtonArea.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(false);
        _LorePanelArea.gameObject.SetActive(false); 
        _LoreText.gameObject.SetActive(false);  

    }

    public void showPanel()
    {
        _exitButtonArea.gameObject.SetActive(true);
        _exitButton.gameObject.SetActive(true);
        _LorePanelArea.gameObject.SetActive(true);
        _LoreText.gameObject.SetActive(true);
        
        //onExitAction = exitButton;
    }
}
