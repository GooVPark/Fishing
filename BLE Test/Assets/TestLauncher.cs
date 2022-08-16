using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLauncher : MonoBehaviour
{
    public enum StickState
    {
        Ready,
        BeginThrow,
        OnThrow,
        EndThrow,
        Wait,
        Catch
    }

    private bool isCatch = false;

    public StickState stickState;
    private double gyroX;
    private double gyroY;
    private double gyroZ;

    private double accX;
    private double accY;
    private double accZ;

    private string state;

    private float dx;
    private float dy;

    private int shakeCount;

    public float[] gyroZs;
    private float lastAngle;

    public Text currentGameStateText;
    public Text gyroSqrMagnitude;
    public Text maxValue;
    public Text motionValue;
    public Text sumOfPower;
    public Text throwTime;
    public Text shakeCountValue;

    public Text roll;
    public Text pitch;

    private float max = -1;
    float power = 0f;

    private float totalPower = 0f;

    public GaugeController gaugeController;
    public FishingFloat fishingFloat;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        MyWoawoaAdapter.OnAccReaded += GetAccValue;
        MyWoawoaAdapter.OnAccStateReaded += GetAccState;
        MyWoawoaAdapter.OnGyroReaded += GetGyroValue;
        MyWoawoaAdapter.OnMotionReaded += GetMotionValue;
        MyWoawoaAdapter.OnGrabReaded += GetGrab;
        MyWoawoaAdapter.OnWalkReaded += GetWalkCount; 

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angularVelocity = new Vector3((float)gyroX, (float)gyroX, (float)gyroZ);
        power = (float)gyroZ;

        if (gyroZ < 60 && state == "1") //Ready
        {
            stickState = StickState.Ready;

            animator.Play("Fishing Idle");

            max = 0;
            totalPower = 0;

            StopCoroutine(waiting);
        }
        else if (stickState == StickState.Ready && power > 90f) //BeginThrow
        {
            stickState = StickState.BeginThrow;

            if (startThrow == null)
            {
                startThrow = StartCoroutine(StartThrow());
            }

        }

        max = max < (power) ? power : max;
        maxValue.text = ((int)max).ToString();

        currentGameStateText.text = stickState.ToString();
    }

    Coroutine startThrow;
    private IEnumerator StartThrow()
    {
        stickState = StickState.OnThrow;
        WaitForFixedUpdate updateTime = new WaitForFixedUpdate();
        float elpasedTime = 0f;

        animator.Play("Fishing Cast");

        while(state != "0")
        {
            elpasedTime += Time.fixedDeltaTime;
            totalPower += (int)power;

            yield return updateTime;
        }

        sumOfPower.text = totalPower.ToString();
        totalPower /= elpasedTime;
        motionValue.text = totalPower.ToString();
        throwTime.text = elpasedTime.ToString();
        stickState = StickState.EndThrow;
        startThrow = null;

        waiting = StartCoroutine(StartWaiting());
    }

    Coroutine waiting;
    private IEnumerator StartWaiting()
    {
        stickState = StickState.Wait;
        fishingFloat.gameObject.SetActive(true);

        //isCatch = true;
        while(true)
        {
            if(state == "1" && isCatch == false)
            {
                stickState = StickState.Ready;
                waiting = null;
                break;
            }
            if(isCatch)
            {
                draw = StartCoroutine(Draw());
                break;
            }
            yield return null;
        }
    }

    Coroutine draw;
    private IEnumerator Draw()
    {
        stickState = StickState.Catch;
        currentGameStateText.text = stickState.ToString();

        ActionType actionType = (ActionType)Random.Range(0, 1);

        gaugeController.StartGaugeControl(actionType);

        yield return null;
    }

    public void OnHit()
    {
        isCatch = true;
    }


    public void GetAccValue(double x, double y, double z)
    {
        accX = x;
        accY = y;
        accZ = z;
    }

    public void GetAccState(string state)
    {
        this.state = state;
    }

    public void GetGyroValue(double x, double y, double z)
    {
        gyroX = x;
        gyroY = y;
        gyroZ = z;
    }

    public void GetMotionValue(int dx, int dy)
    {
    }

    public void GetWalkCount(int count)
    {
        shakeCount = count;
        gaugeController.GetShakeValue(count);
    }

    public void GetGrab(float power)
    {
        gaugeController.GetGrabValue(power);
    }
}
