using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LoadingTextAnimation : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    private string baseText = "Loading";
    private string dots = "";
    private int dotCount = 0;

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        while (true) // This creates an infinite loop
        {
            dots = new string('.', dotCount % 4); // Create a string of dots, from 0 to 3
            loadingText.text = baseText + dots; // Update the TMP text
            dotCount++;

            yield return new WaitForSeconds(0.5f); // Wait for half a second
        }
    }
}
