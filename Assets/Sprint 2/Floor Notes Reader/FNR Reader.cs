using UnityEngine;
using TMPro;

// huge props to https://www.youtube.com/watch?v=B6nCNhsJNjU
public class FNRManager : MonoBehaviour
{
    public GameObject FNRWindow;
    public GameObject Darken_Background;
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
    }

    // hide the Floor Note Reader
    public void hideFNR()
    {
        FNRWindow.SetActive(false);
        Darken_Background.SetActive(false);
    }
}