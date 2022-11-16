using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateFight : GameState
{
    [Header("Other States")]
    public GameStateStruggle gameStateStruggle;
    public GameState gameStateReload;

    [Header("Objects")]
    public FishingMinigame fishingMinigame;

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        FishingMinigame.FishingMinigameWin += Final;
        FishingMinigame.FishingMinigameLose += Miss;

        MyWoawoaAdapter.ins.StartWalk();
        GameManager.fishingStateEvent(AnimationState.Fight);
        SetAnimation(AnimationState.Fight);
        fishingMinigame.MiniGameStart();
    }

    public void Final()
    {
        gameManager.GameState = gameStateStruggle;
    }

    public void Miss()
    {
        gameManager.GameState = gameStateReload;
    }

    public override void OnStateExit()
    {
        FishingMinigame.FishingMinigameWin -= Final;
        FishingMinigame.FishingMinigameLose -= Miss;
        base.OnStateExit();
    }
}
