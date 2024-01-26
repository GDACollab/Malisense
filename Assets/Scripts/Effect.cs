using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Effect
{
    public string effectName;
    public string description;
    public float lifetime;
    public EffectType EffectType;
    
    // The interval which the effect will repeat (in seconds)
    public float period;
    public int stackSize;
    public ParticleSystem particle;
    
    public Effect(EffectData data)
    {
        effectName = data.effectName;
        description = data.description;
        lifetime = data.lifetime;
        EffectType = data.EffectType;
        period = data.period;
        stackSize = data.stackSize;
        particle = data.particle;
    }

    // THIS is where the unique logic of an effect goes, please override :)
    public virtual void ApplyEffect(Character target) {}
}

[CreateAssetMenu(fileName = "New Effect", menuName = "Inventory/Effect")]
public class EffectData : ScriptableObject
{
    [SerializeField, Category("Effect")]
    public string effectName;
    [SerializeField, Category("Effect")]
    public string description;
    [SerializeField, Category("Effect")]
    public float lifetime;
    [SerializeField, Category("Persistance")]
    public EffectType EffectType;
    
    [SerializeField, Category("Persistance"), Tooltip("The interval which the effect will repeat (in seconds)")]
    public float period;
    [SerializeField, Category("Effect")]
    public int stackSize;
    [SerializeField, Category("Effect")]
    public ParticleSystem particle;
}

public enum EffectType {Instant, Persistent}