using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageChange : MonoBehaviour
{

    public GameObject[] pages; 
    public Button[] bookmarks;

    private void Start()
    {
        // show the first page
        ShowPage(0);

        for (int i = 0; i < bookmarks.Length; i++)
        {
            int pageIndex = i; 
            bookmarks[i].onClick.AddListener(() => ShowPage(pageIndex));
        }
    }

    void ShowPage(int pageIndex) //only enables the selected page
    {
        //disabling all the pages
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        //enabling the selected page
        pages[pageIndex].SetActive(true);
    }

}
