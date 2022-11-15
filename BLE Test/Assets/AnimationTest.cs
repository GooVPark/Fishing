using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator fishingrodAnimator;

    [SerializeField] private AnimationState currentState;

    public void SetAnimation(int state)
    {
        AnimationState animationState = (AnimationState)state;

        currentState = animationState;

        playerAnimator.Play(animationState.ToString());
        fishingrodAnimator.Play(animationState.ToString());
    }
}
