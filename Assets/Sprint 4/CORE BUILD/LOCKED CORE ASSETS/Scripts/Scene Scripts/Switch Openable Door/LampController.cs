using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampController : MonoBehaviour
{
    private GameObject lightObject;
    private GameObject volumetricObject;

    private void Start()
    {
        // Find the "Light" and "Volumetric" children objects
        lightObject = transform.Find("Light").gameObject;
        volumetricObject = transform.Find("Volumetric").gameObject;

        // Ensure that the lamp is initially turned off
        TurnOff();
    }

    public void TurnOn()
    {
        // Activate the "Light" and "Volumetric" objects
        lightObject.SetActive(true);
        volumetricObject.SetActive(true);
    }

    public void TurnOff()
    {
        // Deactivate the "Light" and "Volumetric" objects
        lightObject.SetActive(false);
        volumetricObject.SetActive(false);
    }
}