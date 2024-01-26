using System;
using System.Collections.Generic;
using UnityEngine;

// Base class for Characters, both the player, NPCS, and enemies
public abstract class Character : MonoBehaviour, IEffectable
{
    private float _health;
    
    [SerializeField]
    public float maxHealth;

    private Effect data;
    private float currentEffectTime = 0f;
    private float nextTickTime = 0f;
    private List<Effect> effects;
    
    // Start is called before the first frame update
    void Start()
    {
        _health = maxHealth;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (data != null)
        {
            HandleEffect();
        }
    }
    int test = 0;
    public void HandleEffect()
    {
        currentEffectTime += Time.deltaTime;

        if (data.EffectType == EffectType.Persistent && Math.Round(currentEffectTime, 2, MidpointRounding.AwayFromZero) > nextTickTime + data.period)
        {
            nextTickTime += data.period;
            test += 1;
            print("tick" + test);

            // apply specific buffs
            data.ApplyEffect(this);
        }
        else if (data.EffectType == EffectType.Instant)
        {
            data.ApplyEffect(this);
            RemoveEffect();
        }
        
        if (currentEffectTime >= data.lifetime) RemoveEffect();
    }

    private ParticleSystem effectParticles;
    
    public void AddEffect(Effect data)
    {
        print(data);
        this.data = data;
        effectParticles = Instantiate(data.particle, transform);
        HandleEffect();
    }

    public void RemoveEffect()
    {
        data = null;
        currentEffectTime = 0;
        nextTickTime = 0;
        if (effectParticles != null) Destroy(effectParticles);
    }
}
