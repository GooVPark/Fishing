using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateCast : GameState
{
    [Header("Other States")]
    public GameStateWait gameStateWait;
    [Space(5)]

    [Header("Using Obects")]
    public FishingFloat fishingFloat;
    

    IEnumerator Cast()
    {
        WaitForFixedUpdate waitTime = new WaitForFixedUpdate();

        float totalPower = 0f;
        float elapsedTime = 0f;

        while(InputManager.AccState != "0")
        {
            float power = InputManager.GyroValue.z;
            elapsedTime += Time.fixedDeltaTime;
            totalPower += (int)power;

            yield return waitTime;
        }

        totalPower = Mathf.Abs(totalPower);
        totalPower = Mathf.Sqrt(totalPower /= (elapsedTime * elapsedTime));

        //던지는 방법에 따라 totalPower가 Nan이 나오는 경우가 발생함 처리 필요

        Vector3 accValue = InputManager.AccValue;
        Vector3 velocity = playerTransform.rotation * new Vector3(0, -10, totalPower * 0.8f);

        float throwAngle = Mathf.Atan(accValue.y / Mathf.Sqrt((accValue.x * accValue.x) + (accValue.z * accValue.z))) * Mathf.Rad2Deg;
        fishingFloat.Flight(Quaternion.Euler(0, throwAngle * (1f / 6f), 0f) * velocity);

        //낚시찌를 플레이어 머리 살짝 위에서 플레이어의 정면에서 totalPower * 0.8 만큼 앞 -10만큼 아래를 향해서 직사로 발사함.
        //낚시찌가 수면에 닿는 순간부터 waiting으로 들어감
        //낚시찌 날아가는것에 대한 물리효과 구현 필요

        //GameManager.CastEvent
    }

    public void StartWait()
    {
        gameManager.GameState = gameStateWait;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        StartCoroutine(Cast());
        SetAnimation(AnimationState.Cast);
        fishingFloat.CastEnd += StartWait;
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {
        fishingFloat.CastEnd -= StartWait;
        StopCoroutine(Cast());
        base.OnStateExit();
    }
}
