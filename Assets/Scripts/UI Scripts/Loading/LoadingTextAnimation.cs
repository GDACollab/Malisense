using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LoadingTextAnimation : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public GameObject monsterParent;
    public GameObject hintParent;
    public TextMeshProUGUI hintText;
    [TextArea(10, 30)]
    public string hintTexts = "";
    RectTransform[] monsters;
    RectTransform[] hints;
    private string baseText = "Loading";
    private string dots = "";
    private int dotCount = 0;

    void Start()
    {
        StartCoroutine(AnimateText());
        
        monsters = monsterParent.GetComponentsInChildren<RectTransform>(true);
        // hints = hintParent.GetComponentsInChildren<RectTransform>(true);
        string[] hintChoices = hintTexts.Split("\n");
        hintText.text = hintChoices[Random.Range(0, hintChoices.Length)];
        
        foreach(RectTransform monster in monsters){
            monster.gameObject.SetActive(false);
        }
        // foreach(RectTransform hint in hints){
        //     hint.gameObject.SetActive(false);
        // }
        
        monsters[Random.Range(0, monsters.Length)].gameObject.SetActive(true);
        // hints[Random.Range(0, hints.Length)].gameObject.SetActive(true);
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
