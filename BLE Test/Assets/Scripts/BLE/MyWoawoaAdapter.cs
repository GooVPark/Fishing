using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.Events;

public class MyWoawoaAdapter : MyAndroidWrapper
{
    public static MyWoawoaAdapter ins = null;

    public delegate void OnAccReadedEv(double x, double y, double z);
    public static event OnAccReadedEv OnAccReaded;

    public delegate void OnAccStateEv(string state);
    public static event OnAccStateEv OnAccStateReaded;

    public delegate void OnGyroStateEv(double x, double y, double z);
    public static event OnGyroStateEv OnGyroReaded;

    public delegate void OnMotionAccumulatedReadedEv(float x, float y);
    public static event OnMotionAccumulatedReadedEv OnMotionAccumulatedReaded;

    public UnityEvent onConnectFailedWhenNotInConnectingView;

    public Button orgCancelButton;

    public GameObject debugTextCanvas;
    public Text[] texts;

    public bool isGyro3DMode;

    private float accMotionX;
    private float accMotionY;

    public Transform testObject;


    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        Init();
        //if (debugTextCanvas.activeInHierarchy)
        //    StartCoroutine(DebugTextCo());
        Debug.Log("====================Start MyAndoridAdapterr====================");
        StartScan();
    }

    public bool waitForRelease;
    public float releaseThreshold;

    protected override void OnConnectFailed2()
    {
        
    }

    public void StartGyro3DMode()
    {
        Debug.Log("================StartGyro3DMode================");
        isGyro3DMode = true;
        StartGyro3D();
    }

    public void StartMotionMode()
    {
        Debug.Log("================StartMotionMode================");
        isGyro3DMode = false;
        ResetMotionAccumulated();
        StartGyro();
    }

    public void SetVibrationPower(int value)
    {
        SendData($"VS{value}");
    }

    public void StartVibration()
    {
        SendData("VZ");
    }

    public void ResetMotionAccumulated()
    {
        accMotionX = 0;
        accMotionY = 0;
    }


    public void WaitReleasing(float th)
    {
        waitForRelease = true;
        releaseThreshold = th;
    }

    public override void OnGrab(float power)
    {
        grab = power;

        if (waitForRelease)
        {
            if (power < releaseThreshold)
                waitForRelease = false;
        }
        else
        {
            base.OnGrab(power);
        }
    }

    public override void OnMotion(int dx, int dy)
    {
    //    motx = dx;
    //    moty = dy;
        base.OnMotion(dx, dy);

        ////accMotionX += dx;
        ////accMotionY += dy;
        //texts[10].text = accMotionX.ToString();
        //texts[11].text = accMotionY.ToString();

        //if (OnMotionAccumulatedReaded != null)
        //    OnMotionAccumulatedReaded(dx, dy);
    }

    public override void OnGyro3D(double x, double y, double z)
    {
        gyx = x;
        gyy = y;
        gyz = z;

        OnGyroReaded?.Invoke(x, y, z);
    }

    public override void OnAcc(double x, double y, double z, string state)
    {
        accx = x;
        accy = y;
        accz = z;
        accs = state;

        if (OnAccReaded == null)
            return;

        OnAccReaded?.Invoke(x, y, z);
        OnAccStateReaded?.Invoke(state);
    }


    private double gyx;
    private double gyy;
    private double gyz;
    private double accx;
    private double accy;
    private double accz;
    private string accs;

    private float motx;
    private float moty;

    private float grab;

    IEnumerator DebugTextCo()
    {
        while (true)
        {
            //texts[0].text = gyx.ToString();
            //texts[1].text = gyy.ToString();
            //texts[2].text = gyz.ToString();

            ////Vector3 gyro = new Vector3((float)gyx, (float)gyz, (float)gyy);
            ////if (gyro.magnitude < 0.001f) gyro = Vector3.zero;
            ////testObject.Rotate(gyro);

            //double tempAccX = accx * 100;
            //double tempAccY = accy * 100;
            //double tempAccZ = accz * 100;
            //texts[3].text = tempAccX.ToString("0:0000");
            //texts[4].text = tempAccY.ToString("0:0000");
            //texts[5].text = tempAccZ.ToString("0:0000");
            //texts[6].text = accs;

            //texts[7].text = grab.ToString("0:0000");

            //texts[8].text = motx.ToString("0:0000");
            //texts[9].text = moty.ToString("0:0000");

            yield return null;
        }
    }
}


