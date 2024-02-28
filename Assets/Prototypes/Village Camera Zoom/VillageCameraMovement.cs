using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageCameraMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float buildingZoomX, buildingZoomY;
    public float buildingZoomScale;
    public float zoomTimeSeconds = 0;
    private float defaultCameraScale;
    private bool zooming;
    private float zoomScaleStart, zoomScaleGoal;
    private float secondsPassed; // seconds passed since starting the lerp
    private List<GameObject> villageBuildings;

    void Start()
    {
        // TODO: get the list of villageBuildings
        zoomScaleStart = mainCamera.orthographicSize;
        zoomScaleGoal = mainCamera.orthographicSize;

        defaultCameraScale = mainCamera.orthographicSize;
        secondsPassed = 0f;

        BuildingZoom(new GameObject("empty"));
    }

    void Update()
    {
        // when zooming becomes true, lerp from zoomScaleStart to zoomScaleGoal over zoomTimeSeconds
        // TODO: zoom to specific x, y coordinates, maybe not linear zooming, maybe condense into one function
        if (zooming == true)
        {
            // changes camera size based on secondsPassed / zoomTimeSeconds
            mainCamera.orthographicSize = Mathf.Lerp(zoomScaleStart, zoomScaleGoal, secondsPassed / zoomTimeSeconds);
            secondsPassed += Time.deltaTime;

            // handles reaching the end of zoomTimeSeconds and ends the loop
            if (secondsPassed >= zoomTimeSeconds)
            {
                mainCamera.orthographicSize = zoomScaleGoal;
                zooming = false;
                secondsPassed = 0f;
            }
        }
    }

    void Zoom(float scale, float x, float y) // zooms to scale x and y
    {
        zoomScaleStart = mainCamera.orthographicSize;
        zoomScaleGoal = scale;
        zooming = true;
    }

    void BuildingZoom(GameObject building) // call this function to zoom to building
    {
        Debug.Log("Zooming to " + building.name);
        Zoom(1f, 1f, 1f);
    }

    void DefaultZoom() // call this function to zoom back out
    {
        Debug.Log("returning to default zoom");
        Zoom(defaultCameraScale, 0f, 0f);
    }
}