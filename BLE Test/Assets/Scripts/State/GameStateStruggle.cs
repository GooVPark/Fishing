using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateStruggle : GameState
{
    [Header("Other States")]
    public GameStateCatch gameStateCatch;
    public GameStateMiss gameStateMiss;
    [Space(5)]

    [Header("UI")]
    public GameObject struggleGaugeUI;
    public Image struggleGauge;
    public Image struggleTimeLimitGauge;
    [Space(5)]

    [Header("Parameters")]
    public float limitTime;
    public float grabTimeThrashold;

    private IEnumerator Struggle()
    {
        float elapsedTime = 0f;
        float grabTime = 0f;

        while (elapsedTime < limitTime)
        {
            if(InputManager.GrabPower < -4.5f)
            {
                grabTime += Time.deltaTime;
            }

            struggleGauge.fillAmount = grabTime / grabTimeThrashold;

            if(grabTime > grabTimeThrashold)
            {
                gameManager.GameState = gameStateCatch;
                StopCoroutine(Struggle());
            }

            elapsedTime += Time.deltaTime;
            struggleTimeLimitGauge.fillAmount = 1 - elapsedTime / limitTime;
            yield return null;
        }

        gameManager.GameState = gameStateMiss;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        struggleGaugeUI.SetActive(true);
        SetAnimation(AnimationState.Struggle);
        StartCoroutine(Struggle());
    }

    public override void OnStateExit()
    {
        struggleGaugeUI.SetActive(false);
        StopCoroutine(Struggle());
        base.OnStateExit();
    }
}
