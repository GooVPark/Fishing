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

        //android.StartVibration();

        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);

        gameManager.GameState = gameStateReload;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        //android.SetVibrationPower(100);
        //android.SetVibrationType("VA");
        SetAnimation(AnimationState.Bite);
        GameManager.fishingStateEvent?.Invoke(AnimationState.Bite);
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
