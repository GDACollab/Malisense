using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class CreditScroll : MonoBehaviour
{
    [Serializable]
    private class CreditSong
    {
        public float duration;
        public UnityEvent<AudioManager> song;
    }

    [SerializeField] TextAsset creditsText;
    [Header("Scroll Control")]
    [SerializeField] List<Sprite> imageObjects = new List<Sprite>();
    [SerializeField] List<CreditSong> songs = new List<CreditSong>();
    [SerializeField] float scrollSpeed = 1f;
    [SerializeField] int maxLines = 35;
    bool startImage = true;
    List<float> textScrollLanes = new List<float> { -270, 0, 270 };
    List<float> objectScrollLanes = new List<float> { -720, -480, 480, 720 };

    [Header("Fade Control")]
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float fadeOutTime = 0.5f;
    [SerializeField] float longFadeOutTime = 2f;
    [SerializeField] float hintTime = 2f;

    public GameObject creditsTextTemplate;
    public GameObject creditsImageTemplate;
    public GameObject creditsLargeImageTemplate;
    public Animator hintAnimator;
    public Sprite GDALogo;

    List<RectTransform> creditsDisplay = new List<RectTransform>();
    RectTransform imageDisplay;
    RectTransform lastText;
    RectTransform lastRightText;

    List<string> parsedCredits;
    int currLine = 0;
    int currImage = 0;
    bool creditsEnd = false;
    bool creditsTrueEnd = false;

    int currSong = 0;

    float hintTimer;
    bool skipHint => hintTimer > 0;

    GlobalTeapot globalTeapot;
    AudioManager audioManager;
    PlayerInput input;
    Action destroyObjects;

    // Start is called before the first frame update
    void Start()
    {
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();
        audioManager = globalTeapot.audioManager;
        input = GetComponent<PlayerInput>();
        input.currentActionMap.actionTriggered += ShowSkipHint;
        hintTimer = 0;

        parsedCredits = creditsText.text.Split("\n").ToList();

        RectTransform temp = Instantiate(creditsTextTemplate, transform).GetComponent<RectTransform>();
        temp.GetComponent<TMP_Text>().text = parsedCredits[currLine];
        creditsDisplay.Add(temp);
        lastText = temp;
        currLine++;
        Canvas.ForceUpdateCanvases();

        for (int i = 1; i < maxLines; i++)
        {
            SetText();
            Canvas.ForceUpdateCanvases();
        }

        // lastText = creditsDisplay.Last();

        imageDisplay = Instantiate(creditsImageTemplate, transform).GetComponent<RectTransform>();
        imageDisplay.GetComponent<Image>().sprite = imageObjects[currImage];
        imageDisplay.GetComponent<AspectRatioFitter>().aspectRatio = imageObjects[currImage].rect.width / imageObjects[currImage].rect.height;
        imageDisplay.anchoredPosition = new Vector2((currImage % 2 == 0) ? objectScrollLanes[0] : objectScrollLanes[3], imageDisplay.anchoredPosition.y);
        currImage++;

        StartCoroutine(globalTeapot.fader.FadeFromBlack(fadeInTime));
        StartCoroutine(WaitToCall(songs[currSong]));
    }

    // Update is called once per frame
    void Update()
    {
        // Early Exit Hint
        if (hintTimer > 0)
        {
            hintTimer -= Time.deltaTime;
        }
        hintAnimator.SetBool("SkipHint", skipHint);

        foreach (RectTransform disp in creditsDisplay)
        {
            if (disp && disp.anchoredPosition.y > disp.rect.height)
            {
                destroyObjects += () =>
                {
                    creditsDisplay.Remove(disp);
                    Destroy(disp.gameObject);
                };
            }
            disp.transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed);
        }

        if (destroyObjects != null)
        {
            destroyObjects();
            destroyObjects = null;
        }

        if (currLine < parsedCredits.Count)
        {
            if (creditsDisplay.Count < maxLines)
            {
                if (maxLines > creditsDisplay.Count + 1)
                {
                    maxLines--;
                }
                SetText();
            }
            if (imageDisplay == null)
            {
                SetImage();
            }
            if (startImage && Mathf.Abs(creditsDisplay.First().anchoredPosition.y) <= 540)
            {
                startImage = false;
                creditsDisplay.Add(imageDisplay);
            }
        }
        else if (!creditsEnd && currLine >= parsedCredits.Count && Mathf.Abs(lastText.anchoredPosition.y) + lastText.rect.height - 150 <= 540)
        {
            creditsEnd = true;
            var temper = Instantiate(creditsLargeImageTemplate, transform).GetComponent<RectTransform>();

            creditsDisplay.Add(temper);
            temper.GetComponent<Image>().sprite = GDALogo;
        }
        else if (creditsEnd && creditsDisplay.Count <= 1 && !creditsTrueEnd)
        {
            creditsTrueEnd = true;
            StartCoroutine(globalTeapot.fader.FadeToBlack(() => Loader.Load(Loader.Scene.MainMenu, true), longFadeOutTime));
        }
    }

    void SetText()
    {
        string[] tempText = parsedCredits[currLine].Split("|||");
        RectTransform temper;
        if (tempText.Length > 1)
        {
            if (tempText[0].Contains("size"))
            {
                lastText = (lastRightText && lastRightText.anchoredPosition.y < lastText.anchoredPosition.y) ? lastRightText : lastText;
                lastRightText = lastText;
            }
            else
            {
                lastRightText ??= lastText;
            }

            temper = Instantiate(creditsTextTemplate, transform).GetComponent<RectTransform>();
            creditsDisplay.Add(temper);
            temper.sizeDelta /= 1.8f;
            temper.GetComponent<TMP_Text>().text = tempText[0];
            temper.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.CaplineRight;
            temper.anchoredPosition = new Vector2(textScrollLanes[0], lastText.anchoredPosition.y - lastText.rect.height);
            lastText = temper;
            temper = Instantiate(creditsTextTemplate, transform).GetComponent<RectTransform>();
            temper.sizeDelta /= 1.8f;
            creditsDisplay.Add(temper);
            temper.GetComponent<TMP_Text>().text = tempText[1];
            temper.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.CaplineLeft;
            Canvas.ForceUpdateCanvases();
            if (lastText.rect.height != temper.rect.height)
            {
                lastText.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.MidlineRight;
                temper.anchoredPosition = new Vector2(textScrollLanes[2], lastText.anchoredPosition.y - lastText.rect.height / 2 + temper.rect.height / 2);
            }
            else
            {
                temper.anchoredPosition = new Vector2(textScrollLanes[2], lastText.anchoredPosition.y);
            }
            lastRightText = temper;
            maxLines++;
        }
        else
        {
            lastText = (lastRightText && lastRightText.anchoredPosition.y < lastText.anchoredPosition.y) ? lastRightText : lastText;
            lastRightText = null;
            temper = Instantiate(creditsTextTemplate, transform).GetComponent<RectTransform>();
            creditsDisplay.Add(temper);
            temper.GetComponent<TMP_Text>().text = tempText[0];
            temper.anchoredPosition = new Vector2(textScrollLanes[1], lastText.anchoredPosition.y - lastText.rect.height);
            lastText = temper;
        }
        currLine++;
    }

    void SetImage()
    {
        imageDisplay = Instantiate(creditsImageTemplate, transform).GetComponent<RectTransform>();
        imageDisplay.GetComponent<Image>().sprite = imageObjects[currImage];
        imageDisplay.GetComponent<AspectRatioFitter>().aspectRatio = imageObjects[currImage].rect.width / imageObjects[currImage].rect.height;
        imageDisplay.anchoredPosition = new Vector2((currImage % 2 == 0) ? objectScrollLanes[0] : objectScrollLanes[3], imageDisplay.anchoredPosition.y);
        creditsDisplay.Add(imageDisplay);
        currImage = Mathf.Clamp(currImage + 1, 0, imageObjects.Count - 1);
    }

    IEnumerator WaitToCall(Action act, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        act();
    }

    IEnumerator WaitToCall(CreditSong song)
    {
        song.song.Invoke(audioManager);
        yield return new WaitForSeconds(song.duration);
        if (currSong < songs.Count)
        {
            audioManager.StopCurrentSong();
            yield return new WaitForSeconds(1f);
            StartCoroutine(WaitToCall(songs[currSong]));
            currSong++;
        }
    }

    void OnPause()
    {
        if (skipHint)
        {
            StartCoroutine(globalTeapot.fader.FadeToBlack(() => Loader.Load(Loader.Scene.MainMenu, true), fadeOutTime));
        }
    }

    void ShowSkipHint(InputAction.CallbackContext ctx)
    {
        if (ctx.control.device is Keyboard || ctx.control.device is Gamepad)
        {
            hintTimer = hintTime;
        }
    }

    void ShowSkipHint(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is Keyboard || device is Gamepad)
        {
            hintTimer = hintTime;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        audioManager.StopCurrentSong();
    }
}
