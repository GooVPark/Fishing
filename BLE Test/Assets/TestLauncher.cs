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
        Reload,
        Nibble,
        Bite,
        Fighting,
        Final
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

    public FishingMinigame fishingMiniGame;

    public float finalLimitTime;
    public float grabTimeLimit;
    public float grabPower;

    public Image finalStrugglingUI;
    public Image finalStruggleTimerUI;

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

        FishingMinigame.FishingMinigameWin += Final;
        FishingMinigame.FishingMinigameLose += Miss;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angularVelocity = new Vector3((float)gyroX, (float)gyroY, (float)gyroZ);
        power = (float)gyroZ;


        //Debug.Log("==========================================                  GYRO Z : " + gyroZ + "             ======================================================");
        //if (Input.GetMouseButtonDown(0)) SetAnimation(StickState.Ready);
        //if (Input.GetMouseButtonDown(1))
        //{
        //    SetAnimation(StickState.Cast);
        //}


        if (!isStartFishing && Mathf.Abs((float)gyroZ) < 60 && (state == "1" || state == "2")) //Ready
        {
            stickState = StickState.Ready;

            SetAnimation(stickState);

            max = 0;
            totalPower = 0;

            fishingFloat.transform.position = floatPivot.position;
        }
        else if (stickState == StickState.Ready && gyroZ < -60) //BeginThrow
        {
            isStartFishing = true;
            stickState = StickState.Cast;
            SetAnimation(stickState);

            if (startThrow == null)
            {
                startThrow = StartCoroutine(StartThrow());
            }
        }

        if(stickState == StickState.Nibble)
        {
            if(angularVelocity.magnitude > 100f)
            {
                stickState = StickState.Reload;
                SetAnimation(stickState);
            }
        }

        if(stickState == StickState.Bite)
        {
            if(gyroY < -100)
            {
                stickState = StickState.Fighting;
                SetAnimation(stickState);
                StartCoroutine(Fighting());
            }
        }

        max = max < (power) ? power : max;
        maxValue.text = state;
        throwTime.text = angularVelocity.magnitude.ToString();
        motionValue.text = $"{gyroX}\n{gyroY}\n{gyroZ}";
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

        totalPower = Mathf.Abs(totalPower);
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
        while(stickState == StickState.Wait)
        {
            float randomValue = Random.value * 100f;
            if(randomValue > 30f)
            {
                stickState = StickState.Nibble;
            }

            if(state == "1")
            {
                stickState = StickState.Reload;
                SetAnimation(stickState);
                break;
            }
            
            if(stickState == StickState.Nibble)
            {
                StartCoroutine(NibbleTimer());
            }

            yield return null;
        }
    }


    private IEnumerator NibbleTimer()
    {
        int nibbleCount = Random.Range(0, 4);

        for(int i = 0; i < nibbleCount; i++)
        {
            SetAnimation(stickState);
            float randomDelay = Random.Range(0.5f, 2f);

            MyWoawoaAdapter.ins.SetVibrationPower(10);
            MyWoawoaAdapter.ins.StartVibration();

            yield return null;
            yield return new WaitForSeconds(randomDelay + playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        stickState = StickState.Bite;
        SetAnimation(stickState);
        StartCoroutine(Bite());
        yield return null;
    }

    private IEnumerator Bite()
    {
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);

        if (stickState == StickState.Bite)
        {
            stickState = StickState.Reload;
            SetAnimation(stickState);
        }
    }
    private IEnumerator Fighting()
    {
        MyWoawoaAdapter.ins.StopGyro();
        yield return null;
        MyWoawoaAdapter.ins.StartWalk();
        yield return null;
        fishingMiniGame.MiniGameStart();
        yield return null;
    }

    public void Final()
    {
        finalStrugglingUI.gameObject.SetActive(true);
        StartCoroutine(FinalCoroutine());
    }

    private IEnumerator FinalCoroutine()
    {
        stickState = StickState.Final;
        SetAnimation(stickState);

        float elapsedTime = 0f;
        float grabTime = 0f;

        while(elapsedTime < finalLimitTime)
        {
            if(grabPower < -4.5)
            {
                grabTime += Time.deltaTime;

                finalStrugglingUI.fillAmount = grabTime / grabTimeLimit;
            }

            if(grabTime > grabTimeLimit)
            {
                Catch();
                finalStrugglingUI.gameObject.SetActive(false);
                break;
            }
            elapsedTime += Time.deltaTime;
            finalStruggleTimerUI.fillAmount = (finalLimitTime - elapsedTime) / finalLimitTime;
            yield return null;
        }
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
        grabPower = power;
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

    public void OnNibbleBegin()
    {
        stickState = StickState.Nibble;   
    }

    public void OnReloadEnd()
    {
        isStartFishing = false;
        fishingFloat.transform.position = floatPivot.position;
        waiting = null;

        stickState = StickState.Ready;
        SetAnimation(stickState);
    }

    public void Catch()
    {
        MyWoawoaAdapter.ins.StartGyro3DMode();
        finalStrugglingUI.gameObject.SetActive(false);
        stickState = StickState.Catch;
        SetAnimation(stickState);
    }

    public void Miss()
    {
        MyWoawoaAdapter.ins.StartGyro3DMode();
        finalStrugglingUI.gameObject.SetActive(false);
        stickState = StickState.Reload;
        SetAnimation(stickState);       
    }
}
