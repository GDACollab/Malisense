using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Audio;


public class OptionMenu : MonoBehaviour
{

    //

    public AudioMixer audioMixer;

    // Start is called before the first frame update
    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("volume", volume);
    }

    public void SetBrightness(float bright)
    {
        Debug.Log(bright);
    }

}
