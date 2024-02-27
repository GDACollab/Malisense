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
            liftGammaGainEffect.gamma.value = new Vector4(1,1,1,0);
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
            Debug.LogWarning("Missing Lift Gamma Gain Effect");
        //return;

        Debug.Log(liftGammaGainEffect.gamma.value);

        Vector4 gamma = liftGammaGainEffect.gamma.value;

        gamma.w = value;

        liftGammaGainEffect.gamma.value = gamma;
        liftGammaGainEffect.lift.value = gamma;
    }
}