using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Plays VFX and SFX based on scary events near the player, such as seeing a monster.
/// </summary>
public class FearTracker : MonoBehaviour
{
    [Tooltip("How long the fear effect can last with no immediate threat, measured in seconds.")]
    public float lingerDuration = 1f;

    [Tooltip("The approximate time it takes for fear effects to change, measured in seconds. Higher values result in smoother transitions.")]
    public float smoothness = 1f;

    [Tooltip("Whether or not to spike fear effects when first encountering a monster.")]
    public bool canBeJumpscared = true;

    [Tooltip("Time the target must be completely safe before they can be jumpscared again, measured in seconds.")]
    public float resetDuration = 5f;

    /// <summary>
    /// The current intensity of fear effects, from 0 to 1.
    /// </summary>
    public float FearIntensity => _smoothedFear;
    public ChromAbManager _ChromAbManager;
    private float _lastFearTime = float.NegativeInfinity;
    private float _unsmoothedFear;
    private float _smoothedFear;
    private float _vel;

    /// <summary>
    /// Trigger fear effects.
    /// </summary>
    /// <param name="amount">A number from 0 to 1 that represents how threatening the fear source is, possibly attenuated based on distance.</param>
    /// <param name="jumpscare"><c>true</c> if the fear effect should spike before tapering off, <c>false</c> otherwise.</param>
    public void AddFear(float amount, bool jumpscare = true)
    {
        if (amount <= 0f)
            return;

        // "Jumpscare" effect, immediately setting the visual effect to maximum when first startled
        if (Time.time > _lastFearTime + resetDuration && jumpscare && canBeJumpscared)
        {
            Jumpscare();
        }

        _lastFearTime = Time.time;

        _unsmoothedFear = Mathf.Max(_unsmoothedFear, amount);
    }

    private void Jumpscare()
    {
        // If there's a sound effect that plays when you first get spotted by a monster, it goes here!
        // The "resetDuration" field determines the minimum amount of time that must pass before this is called again
        // Sound here

        _smoothedFear = 1f;
    }

    private void Update()
    {
        _unsmoothedFear = Mathf.MoveTowards(_unsmoothedFear, 0f, Time.deltaTime / lingerDuration);
        _smoothedFear = Mathf.SmoothDamp(_smoothedFear, _unsmoothedFear, ref _vel, smoothness);

//#warning Change this to apply the chromatic abberation effect!
        // transform.Find("Sprite").localScale = new Vector3(0.5f, 0.5f - 0.5f * FearIntensity, 1f);
        _ChromAbManager.PissingPants(true, FearIntensity);
    }
}
