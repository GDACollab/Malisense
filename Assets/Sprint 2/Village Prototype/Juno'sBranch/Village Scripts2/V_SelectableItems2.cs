using System.Collections.Generic;
using UnityEngine;

public class V_SelectableItems2 : MonoBehaviour
{
    [SerializeField] private List<GameObject> SELECTABLES = new List<GameObject>();
    [SerializeField] private int listIndex;
    [SerializeField] private bool currentlySelected = false;
    [SerializeField] private bool hasEntered = false;
    GameObject selectedGameObject;
    [SerializeField] private GameObject theFunnyArrow;

    //Camera Zoom Variables
    public Camera mainCamera;
    public float buildingZoomScale;
    public float zoomTimeSeconds;
    private float defaultCameraScale;
    private bool zooming;
    private float zoomScaleStart, zoomScaleGoal;
    private float secondsPassed; // seconds passed since starting the lerp
    private Vector3 cameraStartPosition;
    private Vector3 cameraTargetPosition;
    private bool isCenteringCamera;

    private GameObject thisObject;

    private void Start()
    {
        thisObject = gameObject;
        defaultCameraScale = mainCamera.orthographicSize;
        cameraStartPosition = mainCamera.transform.position;
    }

    private void Update()
    {
        ifSelected();
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
        itemSelected();
    }

    public void selectObject()
    {
        if (selectedGameObject == null)
        {
            Debug.LogError("No selected GameObject.");
            return;
        }

        if (hasEntered) return;
        hasEntered = true;

        // Set target position for camera zoom
        cameraTargetPosition = new Vector3(selectedGameObject.transform.position.x, selectedGameObject.transform.position.y, mainCamera.transform.position.z);
        cameraStartPosition = mainCamera.transform.position;
        isCenteringCamera = true;
        Zoom(buildingZoomScale);

        // UI Fading - Coroutine
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
    }

    private void ifSelected()
    {
        if (V_KeyboardInteractiontion2.Instance.currentlySelectedObject() == thisObject)
        {
            currentlySelected = true;
        }
        else
        {
            currentlySelected = false;
        }
    }
}
