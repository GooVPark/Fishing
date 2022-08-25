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

        //������ ����� ���� totalPower�� Nan�� ������ ��찡 �߻��� ó�� �ʿ�

        Vector3 accValue = InputManager.AccValue;
        Vector3 velocity = playerTransform.rotation * new Vector3(0, -10, totalPower * 0.8f);

        float throwAngle = Mathf.Atan(accValue.y / Mathf.Sqrt((accValue.x * accValue.x) + (accValue.z * accValue.z))) * Mathf.Rad2Deg;
        fishingFloat.Flight(Quaternion.Euler(0, throwAngle * (1f / 6f), 0f) * velocity);

        //����� �÷��̾� �Ӹ� ��¦ ������ �÷��̾��� ���鿡�� totalPower * 0.8 ��ŭ �� -10��ŭ �Ʒ��� ���ؼ� ����� �߻���.
        //����� ���鿡 ��� �������� waiting���� ��
        //������ ���ư��°Ϳ� ���� ����ȿ�� ���� �ʿ�

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
