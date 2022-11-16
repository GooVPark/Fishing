using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    private RaycastHit rayHit;
    public Transform target;

    public ParticleSystem currentEffect;
    public ParticleSystem castEffect;
    public ParticleSystem biteEffect;
    public ParticleSystem fightEffect;
    public ParticleSystem struggleEffect;
    public ParticleSystem catchEffect;
    public ParticleSystem missEffect;

    public LayerMask layer;

    public AnimationState state;

    private Vector3 effectPosition = Vector3.zero;

    public Transform effectPivot;
    private float effectY;

    private void Awake()
    {
        GameManager.fishingStateEvent += ActivateEffect;
    }

    private void Start()
    {
        effectY = effectPivot.position.y;
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, target.position - transform.position, out rayHit, Vector3.Distance(transform.position, target.position), layer))
        {
            effectPosition = rayHit.point;
            switch (state)
            {
                case AnimationState.Ready:
                    break;
                case AnimationState.Cast:
                    if (castEffect.isStopped)
                    {
                        castEffect.transform.position = new Vector3(rayHit.point.x, effectY, rayHit.point.z);
                        Debug.Log(castEffect.transform.position);
                        castEffect.Play();
                    }
                    break;
                case AnimationState.Wait:
                    break;
                case AnimationState.Nibble:
                    break;
                case AnimationState.Bite:

                    break;
                case AnimationState.Fight:
                    currentEffect.transform.position = new Vector3(rayHit.point.x, effectY, rayHit.point.z);
                    break;
                case AnimationState.Struggle:
                    currentEffect.transform.position = new Vector3(rayHit.point.x, effectY, rayHit.point.z);
                    break;
                case AnimationState.Catch:
                    break;
                case AnimationState.Miss:
                    break;
                case AnimationState.Reload:
                    break;
            }
        }
    }

    public void ActivateEffect(AnimationState state)
    {
        this.state = state;
        if(currentEffect != null)
        {
            currentEffect.Stop();
            currentEffect = null;
        }
        switch (state)
        {
            case AnimationState.Ready:
                break;
            case AnimationState.Cast:
                currentEffect = castEffect;
                break;
            case AnimationState.Wait:
                break;
            case AnimationState.Nibble:
                break;
            case AnimationState.Bite:
                currentEffect = biteEffect;
                break;
            case AnimationState.Fight:
                currentEffect = fightEffect;
                break;
            case AnimationState.Struggle:
                currentEffect = struggleEffect;
                break;
            case AnimationState.Catch:
                currentEffect = catchEffect;
                break;
            case AnimationState.Miss:
                currentEffect = missEffect;
                break;
            case AnimationState.Reload:
                break;
        }
        if (currentEffect != null)
        {
            currentEffect.transform.position = new Vector3(effectPosition.x, effectY, effectPosition.z);
            currentEffect.Play();
        }
    }
}
