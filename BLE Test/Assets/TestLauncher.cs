using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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
    [SerializeField] private Transform player;

    private bool isCatch = false;

    public StickState stickState;
    private double gyroX;
    private double gyroY;
    private double gyroZ;

    private float accX;
    private float accY;
    private float accZ;

    private string state;

    private float dx;
    private float dy;

    private int shakeCount;

    public float[] gyroZs;
    private float lastAngle;

    private float throwAngle;

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

    public GameObject fishingFloat;
    public Transform floatPivot;

    public Animator playerAnimator;
    public Animator fishingPoleAnimator;

    public FishingMinigame fishingMiniGame;

    public float finalLimitTime;
    public float grabTimeLimit;
    public float grabPower;
    private float grabTime = 0f;
    private bool isEndCast;
    private bool restartTrigger = false;

    public GameObject struggling;
    public Image finalStrugglingUI;
    public Image finalStruggleTimerUI;

    public TMP_Text floatDistanceUI;

    public bool skipTutorial;
    public GameObject tutorialReady;
    public GameObject tutorialWait;
    public GameObject tutorialNibble;
    public GameObject tutorialBite;
    public GameObject tutorialFighting;
    public GameObject tutorialStruggle;

    private GameObject currentTutorial;
    private GameObject CurrentTutorial
    {
        get { return currentTutorial; }
        set
        {
            currentTutorial?.SetActive(false);
            currentTutorial = value;
            if(!skipTutorial) currentTutorial.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnRestart += OnRestart;

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

        fishingFloat.GetComponent<FishingFloat>().CastEnd += StartWaitring;

        currentTutorial = tutorialReady;
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
        //    isStartFishing = true;
        //    stickState = StickState.Cast;
        //    SetAnimation(stickState);

        //    if (startThrow == null)
        //    {
        //        startThrow = StartCoroutine(StartThrow());
        //    }

        //    totalPower = 100f;
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

        if (stickState == StickState.Nibble)
        {
            if (angularVelocity.magnitude > 100f)
            {
                CurrentTutorial = tutorialReady;
                stickState = StickState.Reload;
                SetAnimation(stickState);
            }
        }

        if (stickState == StickState.Bite)
        {
            if (gyroY < -100)
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
        totalPower = Mathf.Sqrt(totalPower /= (elapsedTime * elapsedTime));

        if(totalPower == float.NaN)
        {
            OnReloadEnd();
            StopCoroutine(StartThrow());
        }

        throwAngle = Mathf.Atan(accY / Mathf.Sqrt((accX * accX) + (accZ * accZ))) * Mathf.Rad2Deg;
        Vector3 velocity = player.rotation * new Vector3(0, -10, totalPower * 0.8f);
        fishingFloat.GetComponent<FishingFloat>().Flight(Quaternion.Euler(0, throwAngle * (1f / 6f), 0) * velocity);
        Debug.Log("===========================================" + throwAngle * (1f / 6f)+ "==========================================");
        sumOfPower.text = totalPower.ToString();
        throwTime.text = elapsedTime.ToString();

        startThrow = null;
    }

    Coroutine waiting;
    public void StartWaitring()
    {
        Debug.Log("===========================================StartWaiting==========================================");
        StartCoroutine(StartWaiting());
    }
    private IEnumerator StartWaiting()
    {
        stickState = StickState.Wait;
        SetAnimation(stickState);
        fishingFloat.gameObject.SetActive(true);

        CurrentTutorial = tutorialWait;

        //isCatch = true;
        while(stickState == StickState.Wait)
        {
            float randomValue = Random.Range(0f, 100f);

            if (state == "1")
            {
                stickState = StickState.Reload;
                SetAnimation(stickState);
                break;
            }
            else if (randomValue < 10f)
            {
                stickState = StickState.Nibble;
                CurrentTutorial = tutorialNibble;
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

            yield return null;
            yield return new WaitForSeconds(randomDelay + playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        stickState = StickState.Bite;
        CurrentTutorial = tutorialBite;

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
        CurrentTutorial = tutorialFighting;
        MyWoawoaAdapter.ins.StopGyro();
        yield return null;
        MyWoawoaAdapter.ins.StartWalk();
        yield return null;
        fishingMiniGame.MiniGameStart();
        yield return null;
    }

    public void Final()
    {
        grabTime = 0;
        struggling.SetActive(true);
        CurrentTutorial = tutorialStruggle;

        finalStruggleTimerUI.fillAmount = 0f;
        finalStrugglingUI.fillAmount = 0f;

        if(finalCoroutine != null)
        {
            StopCoroutine(finalCoroutine);
        }
        finalCoroutine = StartCoroutine(FinalCoroutine());
    }

    Coroutine finalCoroutine;
    private IEnumerator FinalCoroutine()
    {
        stickState = StickState.Final;
        SetAnimation(stickState);

        float elapsedTime = 0f;

        while(elapsedTime < finalLimitTime)
        {
            if(grabPower < -4.5)
            {
                grabTime += Time.deltaTime;
            }

            Debug.Log("============================" + grabTime + "=============================");
            Debug.Log("============================" + grabTimeLimit + "=============================");
            Debug.Log("============================" + finalStrugglingUI.fillAmount + "=============================");

            finalStrugglingUI.fillAmount = grabTime / grabTimeLimit;

            if (grabTime > grabTimeLimit)
            {
                Catch();
                struggling.SetActive(false);
                break;
            }
            elapsedTime += Time.deltaTime;
            finalStruggleTimerUI.fillAmount = (finalLimitTime - elapsedTime) / finalLimitTime;
            yield return null;
        }

        while(restartTrigger == false)
        {
            yield return null;
        }
        restartTrigger = !restartTrigger;

        OnReloadEnd();
    }

    public void GetAccValue(double x, double y, double z)
    {
        accX = (float)x;
        accY = (float)y;
        accZ = (float)z;
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
    }

    public void GetGrab(float power)
    {
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
        isEndCast = true;
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
        fishingFloat.GetComponent<FishingFloat>().isFloating = false;
        CurrentTutorial = tutorialReady;
        stickState = StickState.Ready;
        SetAnimation(stickState);
    }

    public void Catch()
    {
        MyWoawoaAdapter.ins.StartGyro3DMode();
        GameManager.Catch?.Invoke();
        //finalStrugglingUI.gameObject.SetActive(false);
        stickState = StickState.Catch;
        SetAnimation(stickState);
    }

    public void Miss()
    {
        MyWoawoaAdapter.ins.StartGyro3DMode();
        GameManager.Miss?.Invoke();
        //finalStrugglingUI.gameObject.SetActive(false);
        stickState = StickState.Reload;
        SetAnimation(stickState);       
    }

    public void OnRestart()
    {
        restartTrigger = !restartTrigger;
    }
}
