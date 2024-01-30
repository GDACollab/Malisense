using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{

    public Slider brightnessSlider;
    public VolumeProfile postProfile;

    private LiftGammaGain liftGammaGainEffect;

    // Start is called before the first frame update
    void Start()
    {
        if (!postProfile)
            return;

        if (postProfile.TryGet(out liftGammaGainEffect))
        {
            brightnessSlider.value = liftGammaGainEffect.gamma.value.w;
        }
        else
        {
            Debug.LogWarning("Missing Lift Gamma Gain post processing effect!");
        }

    }

    public void SetBrightness(float value)
    {
        if (!liftGammaGainEffect)
            return;

        Vector4 gamma = liftGammaGainEffect.gamma.value;

        gamma.w = value;

        liftGammaGainEffect.gamma.value = gamma;
    }
}