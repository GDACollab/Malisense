using UnityEngine;
using TMPro;

// huge props to https://www.youtube.com/watch?v=B6nCNhsJNjU
public class FNRManager : MonoBehaviour
{
    [Header("Canvas and Window Slots")]
    public GameObject FNRWindow;
    public GameObject Darken_Background;
    public GameObject Floor_Note_Popup_Image;
    [Header("Text")]
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Body;

    public static FNRManager instance; // singleton

    // creates instance 
    public void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(gameObject); }
    }

    // shows the Floor Note Reader
    public void showFNR(string title, string body)
    {
        this.Title.text = title;
        this.Body.text = body;

        FNRWindow.SetActive(true);
        Darken_Background.SetActive(true);
        Floor_Note_Popup_Image.SetActive(true);
    }

    // hide the Floor Note Reader
    public void hideFNR()
    {
        FNRWindow.SetActive(false);
        Darken_Background.SetActive(false);
        Floor_Note_Popup_Image.SetActive(false);
    }
}