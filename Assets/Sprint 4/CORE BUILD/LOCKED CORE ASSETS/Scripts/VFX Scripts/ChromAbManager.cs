using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class ChromAbManager : MonoBehaviour
{
    public FearTracker fearTracker;
    private Volume _volume;

    private void Update()
    {
        if (!_volume && !TryGetComponent(out _volume))
            return;

        _volume.weight = fearTracker.FearIntensity;
    }
}
