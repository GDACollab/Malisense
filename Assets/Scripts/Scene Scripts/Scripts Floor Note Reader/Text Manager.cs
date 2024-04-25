using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI contentBox;

    public void setText(string text) 
    {
        contentBox.text = text;
    }
}
