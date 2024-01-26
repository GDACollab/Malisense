using System;
using System.Collections.Generic;
using UnityEngine;

// Base class for Characters, both the player, NPCS, and enemies
public abstract class Character : MonoBehaviour, IEffectable
{
    private float _health;
    
    [SerializeField]
    public float maxHealth;
    private float currentEffectTime = 0f;
    private float nextTickTime = 0f;
    public List<Effect> effects = new List<Effect>();
    
    // Start is called before the first frame update
    void Start()
    {
        _health = maxHealth;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (effects != null)
        {
            HandleEffect();
        }
    }
    public void HandleEffect()
    {
        currentEffectTime += Time.deltaTime;
        foreach (Effect data in effects)
        {
            if (data.EffectType == EffectType.Persistent && Math.Round(currentEffectTime, 2, MidpointRounding.AwayFromZero) > nextTickTime + data.period)
            {
                nextTickTime += data.period;

                // apply specific buffs
                data.ApplyEffect(this);
            }
            else if (data.EffectType == EffectType.Instant)
            {
                data.ApplyEffect(this);
                RemoveEffect(data);
            }
        
            if (currentEffectTime >= data.lifetime) RemoveEffect(data);   
        }
    }

    private ParticleSystem effectParticles;
    
    public void AddEffect(Effect data)
    {
        effects.Add(data);
        print(effects);
        effectParticles = Instantiate(data.particle, transform);
        HandleEffect();
    }

    public void RemoveEffect(Effect data)
    {
        effects.Remove(data);
        currentEffectTime = 0;
        nextTickTime = 0;
        if (effectParticles != null) Destroy(effectParticles);
    }
}
