using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ARTIFACT CLASS
[CreateAssetMenu(fileName = "New Artifact", menuName = "Inventory/Items/Artifact")]
public class Artifact : ItemBase
{
    [Header("Artifact")]
    [Tooltip("Artifact cooldown in seconds")]
    public float cooldown;
    [Tooltip("Artifact duration in seconds")]
    public float duration;

    private void OnValidate()
    {
        cooldown = 0;
        duration = 0;
    }
}
