using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;			// If needed
using Ink.Runtime;      // Make sure you have this using directive for Ink script

[System.Serializable]		// this lets me fill in the following two fields in the inspector
public class Building
{
	public GameObject light;
	public Transform zoomPoint;
}

public class V_SelectableItems3New : MonoBehaviour
{
	
	[Header("Building Selection")]      // Implemented by Justin Lam (Rxlling_Pxly)

	[Tooltip("0: Scholar, 1: Custodian, 3: Crypt Keeper, 4: Church, 5: Mayor")]
	[SerializeField] Building[] buildings = new Building[5];        // we know that there's only going to be 5 buildings, so we can use an array

	enum Buildings { SCHOLAR, CUSTODIAN, CRYPT_KEEPER, CHURCH, MAYOR }
	[Tooltip("Determines which building is selected at the start of the scene.")]
	[SerializeField] Buildings initialSelectedBuilding;

	int selectedBuildingIndex;
	Building selectedBuilding;


	[Header("Temp Playtest Variables")]
	public GameObject dungeonMessage;

	//Each "Buildings/NPC"
	[SerializeField] private List<GameObject> UI_ELEMENTS = new List<GameObject>(); // List for UI elements
	[SerializeField] private List<TextAsset> InkScripts = new List<TextAsset>(); // List for Inkle Stuff :3
	[SerializeField] public bool currentlySelected = false;
	[SerializeField] public bool hasEntered = false;
	[SerializeField] private bool hasSelected = false;

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
	public bool activateInk;

	private void Start()
	{
		// Building Selection:
		// Turn off every building's light
		foreach (Building building in buildings)
		{
			building.light.SetActive(false);
		}

		// Set selectedBuildingIndex to the index of the building that's initially selected at the start of the scene
		selectedBuildingIndex = (int)initialSelectedBuilding;

		// Set selectedBuilding to the building associated with selectedBuildingIndex
		selectedBuilding = buildings[selectedBuildingIndex];

		// Turn on the light of the selected building 
		buildings[selectedBuildingIndex].light.SetActive(true);


		thisObject = gameObject;
		defaultCameraScale = mainCamera.orthographicSize;
		cameraStartPosition = mainCamera.transform.position;
		CurrentInkTextAsset = InkScripts[0];
		activateInk = false;
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

		selectedBuildingIndex += move;
		if (selectedBuildingIndex < 0)
		{
			selectedBuildingIndex = buildings.Length - 1;
		}
		else if (selectedBuildingIndex >= buildings.Length)
		{
			selectedBuildingIndex = 0;
		}

		selectedBuilding = buildings[selectedBuildingIndex];
		CurrentInkTextAsset = InkScripts[selectedBuildingIndex];

		itemSelected();
	}

	public void selectObject()
	{
		if (hasSelected)
		{
			return;
		}
		else
		{
			hasSelected = true;
		}
		if (selectedBuilding == null)
		{
			Debug.LogError("No selected GameObject.");
			return;
		}

		if (hasEntered)
		{
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
		cameraTargetPosition = selectedBuilding.zoomPoint.position;
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
		// Turn on the light of the building that's selected
		selectedBuilding.light.SetActive(true);

		// Turn off the lights of all the other buildings
		foreach (Building building in buildings)
		{
			if (building != selectedBuilding)
			{
				building.light.SetActive(false);
			}
		}

		dungeonMessage.SetActive(selectedBuildingIndex == 2);
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

		if (selectedBuildingIndex == 2)
		{
			Loader.Load(Loader.Scene.Dungeon);
		}

	}


	private IEnumerator FadeToClear()
	{
		Color objectColor = fadeOutUIImage.color;
		float fadeAmount;
		if (hasEntered)
		{
			ActivateUI(selectedBuildingIndex);
		}
		else
		{
			DeActivateUI(selectedBuildingIndex);
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
			//Ink Activate
			activateInk = true;
		}
	}

	private void DeActivateUI(int index)
	{
		// Deactivate all UI elements
		foreach (var uiElement in UI_ELEMENTS)
		{
			uiElement.SetActive(false);
			activateInk = false;
		}
	}


}
