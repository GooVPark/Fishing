using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateCatch : GameState
{
    [Header("Other States")]
    public GameState gameStateReload;
    [Space(5)]

    [Header("Restart UI")]
    public GameObject restartUI;
    public Image restartGauge;
    [Space(5)]

    [Header("Objects")]
    public HookedFish fish;
    [Space(5)]

    [Header("Parameters")]
    public float grabTimeThrashold;

    private IEnumerator Catch()
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
        fish.gameObject.SetActive(true);
        restartUI.SetActive(true);
        StartCoroutine(Catch());
        SetAnimation(AnimationState.Catch);
    }

    public override void OnStateExit()
    {
        fish.gameObject.SetActive(false);
        restartUI.SetActive(false);
        gameManager.Restart();
        StopCoroutine(Catch());
        base.OnStateExit();
    }
}
