using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private float hookSpeed = 0.1f;
    [SerializeField] private float hookGravity = 0.05f;
    [SerializeField] private float hookPower;

    [SerializeField] private float shakeMax = 100f;

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

    public GameObject miniGameUI;
    private bool isStart = false;

    private void Awake()
    {
        MyWoawoaAdapter.OnGrabReaded += GetGrabValue;
        MyWoawoaAdapter.OnWalkReaded += GetShakeCount;
        
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
        miniGameUI.SetActive(true);
        isStart = true;
    }

    public void MiniGameEnd()
    {
        miniGameUI.SetActive(false);
        isStart = false;
    }

    private void CheckProgress()
    {
        if (fishSlider.value > hookSlider.value - hookSize / 2f && fishSlider.value < hookSlider.value + hookSize / 2f)
        {
            catchProgress += hookPower * Time.deltaTime;
            if(catchProgress >= 1)
            {
                FishingMinigameWin?.Invoke();
                MiniGameEnd();
            }
        }
        else
        {
            catchProgress -= progressBarDecay * Time.deltaTime;
            if(catchProgress <= 0)
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

    float prevCount = 0;

    private void MoveHook()
    {
        switch (actionType)
        {
            case ActionType.Grab:
                if (grabValue < -4.5f)
                {
                    hookPullVelocity += hookSpeed * Time.deltaTime;
                }

                hookPullVelocity -= hookGravity * Time.deltaTime;
                hookPosition += hookPullVelocity;
                break;
            case ActionType.Shake:

                hookPullVelocity -= hookGravity * Time.deltaTime;
                hookPosition = (hookPullVelocity + shakeCount) / shakeMax;
                break;
        }

        if (hookSlider.value <= 0 && hookPullVelocity < 0)
        {
            hookPullVelocity = 0;
        }
        if (hookSlider.value >= 1 && hookPullVelocity > 0)
        {
            hookPullVelocity = 0;
        }
        hookSlider.value = hookPosition;
    }

    public void GetGrabValue(float power)
    {
        grabValue = power;
    }

    public void GetShakeCount(int count)
    {
        shakeCount = (float)count;
    }
}
