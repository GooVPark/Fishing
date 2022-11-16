using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public ParticleSystem nibble;
    public ParticleSystem bite;
    public ParticleSystem cast;
    public ParticleSystem fight;
    public ParticleSystem struggle;
    public ParticleSystem end;

    private AnimationState state;

    private void OnTriggerStay(Collider other)
    {
        
    }
}
