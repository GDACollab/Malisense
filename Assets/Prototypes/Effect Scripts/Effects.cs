using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// List of all custom effects
public class Effects : MonoBehaviour
{
    public class CloudBootsEffect : Effect
    {
        public CloudBootsEffect(EffectData data) : base(data) {}
        public override void ApplyEffect(Character target)
        {
            print("hiiiiiii this works");
        }
    }
}
