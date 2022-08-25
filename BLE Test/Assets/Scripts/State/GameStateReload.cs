using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateReload : GameState
{
    [Header("Other States")]
    public GameStateReady gameStateReady;
    [Space(5)]

    [Header("Objects")]
    public FishingFloat fishingFloat;
    public Transform floatPivot;

    private IEnumerator Reload()
    {
        yield return null;
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);

        gameManager.GameState = gameStateReady;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        SetAnimation(AnimationState.Reload);
        StartCoroutine(Reload());
    }

    public override void OnStateExit()
    {
        StopCoroutine(Reload());

        fishingFloat.transform.position = floatPivot.position;
        fishingFloat.isFloating = false;

        base.OnStateExit();
    }
}
