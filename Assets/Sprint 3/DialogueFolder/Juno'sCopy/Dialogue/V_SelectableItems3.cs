using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // If needed
using Ink.Runtime; // Make sure you have this using directive for Ink script

public class V_SelectableItems3 : MonoBehaviour
{   
    [Header("Temp Playtest Vars")]
    public GameObject dungeonMessage;
    
    //Each "Buildings/NPC"
    [SerializeField] private List<GameObject> SELECTABLES = new List<GameObject>();
    [SerializeField] private List<GameObject> UI_ELEMENTS = new List<GameObject>(); // List for UI elements
    [SerializeField] private List<TextAsset> InkScripts = new List<TextAsset>(); // List for Inkle Stuff :3
    [SerializeField] private int listIndex;
    [SerializeField] public bool currentlySelected = false;
    [SerializeField] private bool hasEntered = false;
    [SerializeField] private bool hasSelected = false;
    GameObject selectedGameObject;
    [SerializeField] private GameObject theFunnyArrow;

    //Camera Zoom Variables
    public Camera mainCamera;
    public float buildingZoomScale;
    public float zoomTimeSeconds;
    private float defaultCameraScale;
    public bool zooming;
    private float zoomScaleStart, zoomScaleGoal;
    private float secondsPassed; // seconds passed since starting the lerp
    private Vector3 cameraStartPosition;
    private Vector3 cameraTargetPosition;
    private bool isCenteringCamera;
    private GameObject thisObject;

    //UI Stuff
    public Image fadeOutUIImage; // Reference to the UI Image
    public float fadeSpeed = 0.5f;

    //Inkle
    public TextAsset CurrentInkTextAsset;

    private void Start()
    {
        thisObject = gameObject;
        defaultCameraScale = mainCamera.orthographicSize;
        cameraStartPosition = mainCamera.transform.position;
        selectedGameObject = SELECTABLES[0];
        CurrentInkTextAsset = InkScripts[0];
    }

    private void Update()
    {
      
        if (!currentlySelected) return;

        if (zooming)
        {
            float t = Mathf.Clamp01(secondsPassed / zoomTimeSeconds); // Ensure t is between 0 and 1
            mainCamera.orthographicSize = Mathf.Lerp(zoomScaleStart, zoomScaleGoal, t);

            if (isCenteringCamera)
            {
                mainCamera.transform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition, t);
            }

            secondsPassed += Time.deltaTime;

            if (secondsPassed >= zoomTimeSeconds)
            {
                zooming = false;
                isCenteringCamera = false;
                StartCoroutine(FadeToClear());
            }
        }



    }

    public void moveInList(int move)
    {
        if (!currentlySelected) return;

        listIndex += move;
        if (listIndex < 0)
        {
            listIndex = SELECTABLES.Count - 1;
        }
        else if (listIndex >= SELECTABLES.Count)
        {
            listIndex = 0;
        }

        selectedGameObject = SELECTABLES[listIndex];
        CurrentInkTextAsset = InkScripts[listIndex];

        itemSelected();
    }

    public void selectObject()
    {
        if(hasSelected){
            return;
        }
        else{
            hasSelected = true;
        }
        if (selectedGameObject == null)
        {
            Debug.LogError("No selected GameObject.");
            return;
        }

        if (hasEntered){
            hasEntered = false;
            Vector3 temp = cameraStartPosition;
            cameraStartPosition = cameraTargetPosition;
            cameraTargetPosition = temp;
            isCenteringCamera = true;
            DefaultZoom();
            
            StartCoroutine(FadeToBlack());
            return;
        };
        hasEntered = true;

        // Set target position for camera zoom
        cameraTargetPosition = new Vector3(selectedGameObject.transform.position.x, selectedGameObject.transform.position.y, mainCamera.transform.position.z);
        cameraStartPosition = mainCamera.transform.position;
        isCenteringCamera = true;
        Zoom(buildingZoomScale);

        // UI Fading - Coroutine
        StartCoroutine(FadeToBlack());
        // Villager UI Appears
        // Reset Camera
    }

    private void Zoom(float newScale)
    {
        zoomScaleStart = mainCamera.orthographicSize;
        zoomScaleGoal = newScale;
        secondsPassed = 0;
        zooming = true;
    }

    private void DefaultZoom()
    {
        Zoom(defaultCameraScale);
    }

    private void itemSelected()
    {
        float targetXPosition = selectedGameObject.transform.position.x;

        Vector3 currentPosition = theFunnyArrow.transform.position;
        Vector3 newPosition = new Vector3(targetXPosition, currentPosition.y, currentPosition.z);
        theFunnyArrow.transform.position = newPosition;
        dungeonMessage.SetActive(listIndex == 2);
    }

    private void ifSelected()
    {
        if (V_KeyboardInteractiontion3.Instance.currentlySelectedObject() == thisObject)
        {
            currentlySelected = true;
        }
        else
        {
            currentlySelected = false;
        }
    }

    private IEnumerator FadeToBlack()
    {
        Color objectColor = fadeOutUIImage.color;
        float fadeAmount;

        while (fadeOutUIImage.color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            fadeOutUIImage.color = objectColor;
            yield return null;
        }

        if (listIndex == 2)
        {
            Loader.Load(Loader.Scene.Dungeon);
        }

    }


    private IEnumerator FadeToClear()
    {
        Color objectColor = fadeOutUIImage.color;
        float fadeAmount;
        if(hasEntered){
            ActivateUI(listIndex);
        }
        else{
            DeActivateUI(listIndex);
        }

        yield return new WaitForSeconds(2.0f);

        while (fadeOutUIImage.color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            fadeOutUIImage.color = objectColor;
            yield return null;
        }
        hasSelected = false;
    }

    private void ActivateUI(int index)
    {
        // Deactivate all UI elements
        foreach (var uiElement in UI_ELEMENTS)
        {
            uiElement.SetActive(false);
        }

        // Activate the UI element that corresponds to the selected object
        if (index >= 0 && index < UI_ELEMENTS.Count)
        {
            //UI Activate
            UI_ELEMENTS[index].SetActive(true);

            //Activate Ink
        }
    }
    
    private void DeActivateUI(int index)
    {
        // Deactivate all UI elements
        foreach (var uiElement in UI_ELEMENTS)
        {
            uiElement.SetActive(false);
        }
    }


 }
