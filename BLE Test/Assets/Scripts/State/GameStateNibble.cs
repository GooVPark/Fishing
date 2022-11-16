using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateNibble : GameState
{
    [Header("Other States")]
    public GameStateBite gameStateBite;
    public GameStateReload gameStateReload;

    private IEnumerator Nibble()
    {
        int nibbleCount = Random.Range(0, 4);

        Debug.Log("========================= Nibble Count: " + nibbleCount + "=========================");

        for(int i = 0; i < nibbleCount; i++)
        {
            Debug.Log("========================= Current Nibble Count: " + i + "=========================");
            SetAnimation(AnimationState.Nibble);

            android.StartVibration();

            float randomDelay = Random.Range(1f, 4f);

            yield return null;
            yield return new WaitForSeconds(randomDelay + playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        gameManager.GameState = gameStateBite;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        android.SetVibrationPower(30);
        android.SetVibrationType("VA");


        StartCoroutine(Nibble());
    }

    public override void OnStateExit()
    {
        StopCoroutine(Nibble());
        base.OnStateExit();
    }

    public override void OnStateUpdate()
    {
        if(InputManager.GyroValue.magnitude > 100f)
        {
            gameManager.GameState = gameStateReload;
        }
    }
}
