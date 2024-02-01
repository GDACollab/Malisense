using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Rendering.PostProcessing;

public class ChromAbManager : MonoBehaviour
{
    private PostProcessVolume _postProcessVolume;
    private ChromaticAberration _chromaticAberration;
    [SerializeField] public TextureParameter _spectralLut;


    private void Start()
    {
        _postProcessVolume = GetComponent<PostProcessVolume>();
        _postProcessVolume.profile.TryGetSettings(out _chromaticAberration);
    }

    public bool PissingPants(bool now, float wetness)
    {
        // spectralLut is the shift in hue of the effect
        _chromaticAberration.spectralLut = _spectralLut;
        // the intensity of the aberration effect
        _chromaticAberration.intensity.value = wetness;
        // fastMode boosts performance when true
        _chromaticAberration.fastMode.value = false;

        _chromaticAberration.active = false;
        if (now)
        {
            _chromaticAberration.active = true;
        }

        return now;
    }
}
