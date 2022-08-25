using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationState
{
    Ready,
    Cast,
    Wait,
    Nibble,
    Bite,
    Fight,
    Struggle,
    Catch,
    Miss,
    Reload
}
public abstract class GameState : MonoBehaviour
{
    public GameManager gameManager;

    public Transform playerTransform;
    public Animator playerAnimator;
    public Animator fishingRodAnimator;

    public GameObject tutorial;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetAnimation(AnimationState state)
    {
        string animationState = state.ToString();

        playerAnimator.Play(animationState);
        fishingRodAnimator.Play(animationState);
    }

    public virtual void OnStateEnter()
    {
        gameObject.SetActive(true);
        tutorial?.SetActive(true);
        Debug.Log("============================= Start: " + gameObject.name + " ===================================");
    }
    public virtual void OnStateExit()
    {
        Debug.Log("============================= End: " + gameObject.name + " ===================================");
        tutorial?.SetActive(false);
        gameObject.SetActive(false);
        gameManager.locomotion.SetActive(false);
    }
    public virtual void OnStateUpdate() { }
}
