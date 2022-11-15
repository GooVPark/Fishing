using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateWait : GameState
{
    [Header("Ohter States")]
    public GameStateNibble gameStateNibble;
    public GameStateReload gameStateReload;

    private IEnumerator Wait()
    {
        bool isNibble = false;

        while (true)
        {
            isNibble = Random.value > 0.5f;
            if(isNibble)
            {
                gameManager.GameState = gameStateNibble;
                break;
            }

            string accState = InputManager.AccState;
            if(InputManager.AccValue.magnitude > 100)
            {
                gameManager.GameState = gameStateReload;
                break;
            }

            Debug.Log("=============== Waiting ==============");
            float waitingTime = Random.Range(1.0f, 5.0f);
            yield return new WaitForSeconds(waitingTime);
        }
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        StartCoroutine(Wait());
        SetAnimation(AnimationState.Wait);
    }

    public override void OnStateExit()
    {
        StopCoroutine(Wait());
        base.OnStateExit();
    }
}
