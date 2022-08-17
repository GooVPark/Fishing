using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestLauncher : MonoBehaviour
{
    public enum StickState
    {
        Cast,
        Ready,
        BeginThrow,
        OnThrow,
        EndThrow,
        Wait,
        Catch,
        Reload
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

    private bool isStartFishing = false;

    private bool isThrow = false;
    private float totalPower = 0f;

    public GaugeController gaugeController;
    public GameObject fishingFloat;
    public Transform floatPivot;

    public Animator playerAnimator;
    public Animator fishingPoleAnimator;


    // Start is called before the first frame update
    void Start()
    {
        MyWoawoaAdapter.OnAccReaded += GetAccValue;
        MyWoawoaAdapter.OnAccStateReaded += GetAccState;
        MyWoawoaAdapter.OnGyroReaded += GetGyroValue;
        MyWoawoaAdapter.OnMotionReaded += GetMotionValue;
        MyWoawoaAdapter.OnGrabReaded += GetGrab;
        MyWoawoaAdapter.OnWalkReaded += GetWalkCount;

        PlayerAnimationEvent.CastEndEvent += OnCastEnd;
        PlayerAnimationEvent.ThrowEvent += OnThrow;
        PlayerAnimationEvent.ReloadEndEvent += OnReloadEnd;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angularVelocity = new Vector3((float)gyroX, (float)gyroX, (float)gyroZ);
        power = (float)gyroZ;


        //Debug.Log("==========================================                  GYRO Z : " + gyroZ + "             ======================================================");
        //if (Input.GetMouseButtonDown(0)) SetAnimation(StickState.Ready);
        //if (Input.GetMouseButtonDown(1))
        //{
        //    SetAnimation(StickState.Cast);
        //}


        if (!isStartFishing && gyroZ < 60 && state == "1") //Ready
        {
            stickState = StickState.Ready;

            SetAnimation(stickState);

            max = 0;
            totalPower = 0;

            fishingFloat.transform.position = floatPivot.position;

            StopCoroutine(waiting);
        }
        else if (stickState == StickState.Ready && power > 90f) //BeginThrow
        {
            isStartFishing = true;
            stickState = StickState.Cast;
            SetAnimation(stickState);

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
        isThrow = false;
        WaitForFixedUpdate updateTime = new WaitForFixedUpdate();

        totalPower = 0f;
        float elapsedTime = 0f;
        while(state != "0" && !isThrow)
        {
            elapsedTime += Time.fixedDeltaTime;
            totalPower += (int)power;

            yield return updateTime;
        }

        totalPower = Mathf.Sqrt(totalPower /= elapsedTime);

        fishingFloat.GetComponent<Rigidbody>().useGravity = true;
        fishingFloat.GetComponent<Rigidbody>().isKinematic = false;

        fishingFloat.GetComponent<Rigidbody>().AddForce(totalPower * Vector3.forward * 50f);

        sumOfPower.text = totalPower.ToString();
        throwTime.text = elapsedTime.ToString();
        startThrow = null;
    }

    Coroutine waiting;
    private IEnumerator StartWaiting()
    {
        stickState = StickState.Wait;
        SetAnimation(stickState);
        fishingFloat.gameObject.SetActive(true);

        //isCatch = true;
        while(true)
        {
            if(state == "1" && isCatch == false)
            {
                stickState = StickState.Reload;
                SetAnimation(stickState);
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

    public void SetAnimation(StickState state)
    {
        string animationState = state.ToString();

        playerAnimator.Play(animationState);
        fishingPoleAnimator.Play(animationState);
    }

    public void OnCastEnd()
    {

        waiting = StartCoroutine(StartWaiting());
    }

    public void OnThrow()
    {
        isThrow = true;
    }

    public void OnReloadEnd()
    {
        isStartFishing = false;
        fishingFloat.transform.position = floatPivot.position;
        waiting = null;

        stickState = StickState.Ready;
        SetAnimation(stickState);
    }
}
