using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishingMinigame : MonoBehaviour
{
    public enum ActionType { Grab, Shake }
    public ActionType actionType = ActionType.Shake;

    public delegate void FishingMiniGameWinEvent();
    public static FishingMiniGameWinEvent FishingMinigameWin;
    public delegate void FishingMiniGameLoseEvent();
    public static FishingMiniGameLoseEvent FishingMinigameLose;


    [Header("Fishing Area")]
    [SerializeField] private Transform topBounds;
    [SerializeField] private Transform bottomBounds;


    [Header("Fish Setting")]
    [SerializeField] private Transform fish;
    [SerializeField] private float smoothMotion = 3f;
    [SerializeField] private float fishTimeRandomizer = 3f;

    private float fishPosition;
    private float fishSpeed;
    private float fishTimer;
    private float fishTargetPosition;

    [SerializeField] private Slider fishSlider;

    [Header("Hook Setting")]
    [SerializeField] private Transform hook;
    [SerializeField] private float hookSize = 0.18f;
    [SerializeField] private float hookSpeed = 0.001f;
    [SerializeField] private float hookGravity = 0.05f;
    [SerializeField] private float hookPower;

    [SerializeField] private float shakeMax = 100f;
    [SerializeField] private float shakeDecay = 3f;

    private float hookPosition;
    private float hookPullVelocity;
    private float shakeReduce = 0;

    [SerializeField] private Slider hookSlider;
    [SerializeField] private Image progressBar;

    public float progressBarDecay;
    private bool actionEnabled;

    private float catchProgress;

    private float grabValue;
    private float shakeCount;
    private int prevCount = 0;
    private int countFactor = 0;

    [SerializeField] private float grabTime;
    [SerializeField] private float grabLimit;

    [SerializeField] private Text minigameElement;
    public GameObject miniGameUI;
    private bool isStart = false;

    private float shakeCountEq;

    float distance;
    public TMP_Text distanceText;

    public FishingFloat floating;
    private Vector3 floatingStartPosition;

    public float shakeFactor = 0;

    private void Awake()
    {
        
    }
    private void FixedUpdate()
    {
        if (isStart)
        {
            MoveFish();
            MoveHook();
            CheckProgress();
        }
    }

    public void MiniGameStart()
    {
        catchProgress = 0.3f;

        fishTimer = 0;
        grabTime = 0;

        shakeReduce = shakeCount;
        distance = 100f;

        floatingStartPosition = floating.transform.position;

        MyWoawoaAdapter.OnGrabReaded += GetGrabValue;
        MyWoawoaAdapter.OnWalkReaded += GetShakeCount;

        miniGameUI.SetActive(true);
        isStart = true;
    }

    public void MiniGameEnd()
    {
        shakeReduce = 0;
        miniGameUI.SetActive(false);
        isStart = false;
    }
    //x = distance / 0.7

    private void CheckProgress()
    {
        if (fishSlider.value > hookSlider.value - hookSize / 2f && fishSlider.value < hookSlider.value + hookSize / 2f)
        {
            catchProgress += hookPower * Time.deltaTime;
            float closingDistance = ((distance - (distance * catchProgress)) / 0.7f);
            distanceText.text = closingDistance.ToString("0.0");
            //floating.transform.position = Vector3.Lerp(transform.position, floatingStartPosition, closingDistance / floating.distance);
            if(catchProgress >= 1)
            {
                FishingMinigameWin?.Invoke();
                MiniGameEnd();
            }
        }
        else
        {
            catchProgress -= progressBarDecay * Time.deltaTime;
            if (catchProgress <= 0)
            {
                FishingMinigameLose?.Invoke();
                MiniGameEnd();
            }
        }

        catchProgress = Mathf.Clamp(catchProgress, 0, 1);
        progressBar.fillAmount = catchProgress;
    }

    private void MoveFish()
    {
        fishTimer -= Time.deltaTime;

        if(fishTimer < 0)
        {
            fishTimer = Random.value * fishTimeRandomizer;
            fishTargetPosition = Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishTargetPosition, ref fishSpeed, smoothMotion);
        fishSlider.value = fishPosition;
    }


    private void MoveHook()
    {
        switch (actionType)
        {
            case ActionType.Grab:
                if (grabValue < -4.5f)
                {
                    grabTime += Time.deltaTime;
                }
                else
                {
                    if(grabTime > 0)
                    {
                        grabTime -= Time.fixedTime * 1.5f;
                    }
                }

                hookPosition = grabTime / grabLimit;
                break;
            case ActionType.Shake:

                float grabFactor = 0;

                if (grabValue < -4.5f)
                {
                    grabFactor = -1f;
                }

                if(shakeCount > shakeReduce)
                {
                    shakeReduce += Time.deltaTime * shakeDecay;
                }

                hookPosition = (shakeCount - shakeReduce + grabFactor) / shakeMax;
                break;
        }

        minigameElement.text = $"{shakeCount} - {shakeReduce}";
        hookSlider.value = hookPosition;
    }

    public void GetGrabValue(float power)
    {
        grabValue = power;
    }

    public void GetShakeCount(int count)
    {
        //int currentCount = count;

        //if(currentCount - prevCount == 4)
        //{
        //    countFactor -= 4;
        //}

        //Debug.Log($"CurrentCount: {count}\nPrevCount: {prevCount}\nCountFActor: {countFactor}");
        shakeCount = (float)(count + countFactor);// * 2 * (1 + shakeFactor/100f);
        Debug.Log("=======================================================================================================" + shakeCount + "============================================================================");
        //prevCount = currentCount;
    }
}
