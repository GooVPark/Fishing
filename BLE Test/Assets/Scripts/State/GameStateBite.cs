using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBite : GameState
{
    [Header("Other States")]
    public GameStateFight gameStateFight;
    public GameStateReload gameStateReload;

    [Header("Prameters")]
    public float biteToFightTrashold;

    private IEnumerator Bite()
    {
        yield return null;
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);

        gameManager.GameState = gameStateReload;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        SetAnimation(AnimationState.Bite);
        StartCoroutine(Bite());
    }

    public override void OnStateExit()
    {
        StopCoroutine(Bite());
        base.OnStateExit();
    }

    public override void OnStateUpdate()
    {
        if(InputManager.GyroValue.magnitude > biteToFightTrashold)
        {
            Debug.Log(biteToFightTrashold);
            SetAnimation(AnimationState.Fight);
            gameManager.GameState = gameStateFight;
        }
    }
}
