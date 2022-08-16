using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType { Grab, Shake }

public class GaugeController : MonoBehaviour
{

    private ActionType actionType;

    public Slider targetSlider;
    public Slider gaugeSlider;

    private float power;
    private float gaugeAmount;

    private float shakeCount;
    private float shakeReduce;
    public float shakeLimit;

    private float grabPower;
    private float grabTime;
    private float grabReduce;
    public float grabLimit;
    private float grabValue;
    private float hitTime;
    public float hitTimeLimit;
    private float missTime;
    public float missTimeLimit;


    public Text shakeDebugger;
    public Text actionTypeText;
    public Text timer;

    public void StartGaugeControl(ActionType actionType)
    {
        StartCoroutine(GaugeControl(actionType));
    }

    private IEnumerator GaugeControl(ActionType actionType)
    {
        actionTypeText.text = actionType.ToString();
        MyWoawoaAdapter.ins.StopGyro();
        MyWoawoaAdapter.ins.ClearWalkData();
        MyWoawoaAdapter.ins.StartWalk();

        missTime = 0f;
        hitTime = 0f;
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        float hitZone = 2f;
        float hitZoneHalf = hitZone / 2;
        float targetPosition = Random.Range(0f, 1f);

        

        targetSlider.value = targetPosition;

        while(true)
        {

            switch (actionType)
            {
                case ActionType.Grab:

                    if (grabPower < -4.5f)
                    {
                        grabTime += Time.fixedDeltaTime;
                    }
                    else
                    {
                        if (grabTime > 0)
                        {
                            grabTime -= Time.fixedDeltaTime * 1.5f;
                        }
                    }

                    grabValue = grabTime / grabLimit;
                    gaugeSlider.value = grabValue;

                    shakeDebugger.text = $"{grabTime}";

                    break;
                case ActionType.Shake:

                    if (shakeCount > shakeReduce)
                    {
                        shakeReduce += Time.fixedDeltaTime;
                    }

                    power = (shakeCount - shakeReduce) / shakeLimit;
                    gaugeSlider.value = power;


                    shakeDebugger.text = $"{shakeCount}, {shakeReduce}";

                    break;
            }

            if (gaugeSlider.value < targetPosition - 0.1f || gaugeSlider.value > targetPosition + 0.1f)
            {
                missTime += Time.fixedDeltaTime;
                if (missTimeLimit < missTime)
                {
                    //Miss Event
                    break;
                }
            }
            else
            {
                hitTime += Time.fixedDeltaTime;
                if (hitTimeLimit < hitTime)
                {
                    //Hit Event
                    int at = ((int)actionType + 1) % 2;
                    StartCoroutine(GaugeControl((ActionType)at));
                    break;
                }
            }

            timer.text = $"m: {missTime}, h: {hitTime}";

            yield return waitForFixedUpdate;
        }
    }

    public void GetGrabValue(float power)
    {
        grabPower = power;
    }

    public void GetShakeValue(int count)
    {
        shakeCount = (float)count;
    }
}
