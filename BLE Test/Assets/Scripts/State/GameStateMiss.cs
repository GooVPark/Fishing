using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateMiss : GameState
{
    [Header("Other States")]
    public GameState gameStateReload;
    [Space(5)]

    [Header("Restart UI")]
    public GameObject restartUI;
    public Image restartGauge;
    [Space(5)]

    [Header("Parameters")]
    public float grabTimeThrashold;

    private IEnumerator Miss()
    {
        float grabTime = 0f;
        restartGauge.fillAmount = 0f;
        
        while (grabTime < grabTimeThrashold)
        {
            if (InputManager.GrabPower < -4.5f)
            {
                grabTime += Time.deltaTime;
            }
            else
            {
                grabTime = 0f;
            }

            restartGauge.fillAmount = 1 - grabTime / grabTimeThrashold;

            yield return null;
        }

        gameManager.GameState = gameStateReload;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        StartCoroutine(Miss());
        SetAnimation(AnimationState.Miss);
        restartUI.SetActive(true);
    }

    public override void OnStateExit()
    {
        StopCoroutine(Miss());
        restartUI.SetActive(false);
        gameManager.Restart();
        base.OnStateExit();
    }
}
