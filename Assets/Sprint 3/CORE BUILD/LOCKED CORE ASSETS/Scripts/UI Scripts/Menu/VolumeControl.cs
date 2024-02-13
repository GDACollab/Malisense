using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for working with UI elements like Slider

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the UI slider

    private void Start()
    {
        // Set the slider's value to the current volume at the start
        FMOD.Studio.Bus masterBus = RuntimeManager.GetBus("bus:/Master");
        masterBus.getVolume(out float volume);
        volumeSlider.value = volume;
    }

    public void SetVolume()
    {
        FMOD.Studio.Bus masterBus = RuntimeManager.GetBus("bus:/Master");

        // Check if getting the bus was successful
        FMOD.RESULT result = masterBus.setVolume(volumeSlider.value);

        // Handle potential error
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("FMOD setVolume error: " + result);
        }
    }
}
