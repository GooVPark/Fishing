using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateReady : GameState
{
    [Header("Other States")]
    public GameStateCast gameStateCast;
    

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        SetAnimation(AnimationState.Ready);   
    }
    
    public override void OnStateUpdate()
    {
        Vector3 gyroValue = InputManager.GyroValue;

        if(gyroValue.z < -60)
        {
            gameManager.GameState = gameStateCast;
        }

    }
    
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
