using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LoadingTextAnimation : MonoBehaviour
{
    public LoaderCallback loaderCallback;
    public TextMeshProUGUI loadingText;
    public GameObject monsterParent;
    public TextMeshProUGUI hintText;
    [TextArea(10, 30)]
    public string hintTexts = "";
    [SerializeField] private GameObject party;
    RectTransform[] monsters;
    private string baseText = "Loading";
    private string dots = "";
    private int dotCount = 0;

    void Start()
    {
        StartCoroutine(AnimateText());
        
        monsters = monsterParent.GetComponentsInChildren<RectTransform>(true);
        string[] hintChoices = hintTexts.Split("\n");
        hintText.text = hintChoices[Random.Range(0, hintChoices.Length)];
        
        foreach(RectTransform monster in monsters){
            monster.gameObject.SetActive(false);
        }
        
        monsters[Random.Range(0, monsters.Length)].gameObject.SetActive(true);
        if(Random.Range(0,1000)==3){
            StartCoroutine(Party());
        }
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
    
    
    IEnumerator Party(){
        hintText.text = "Did you know there's a 1 in 1000 chance to...";
        loaderCallback.keepLoading = true;
        yield return new WaitForSeconds(3f);
        party.SetActive(true);
        yield return new WaitForSeconds(2.75f);
        loadingText.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(5f);
        loaderCallback.keepLoading = false;
    }
}
