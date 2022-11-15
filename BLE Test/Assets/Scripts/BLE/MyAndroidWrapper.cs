using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class MyAndroidWrapper : MonoBehaviour
{
    public class IPlatformAdapter : MonoBehaviour
    {
        public virtual void Initialize(MyAndroidWrapper myAndroidWrapper)
        {
            
        }

        public virtual bool IsBluetoothEnabled()
        {
            return false;
        }

        public virtual bool IsConnected()
        {
            return false;
        }

        public virtual void StartScan()
        {

        }

        public virtual void Disconnect()
        {

        }

        public virtual void SendData(string data)
        {

        }

        public virtual void OnConnected()
        {

        }

        public virtual void OnConnectFailed()
        {

        }

        public virtual void OnDisconnected()
        {

        }
    }

    public delegate void OnGrabReadedEv(float power);
    public static event OnGrabReadedEv OnGrabReaded;

    public delegate void OnMotionReadedEv(int dx, int dy);
    public static event OnMotionReadedEv OnMotionReaded;

    public delegate void OnWalkReadedEv(int count);
    public static event OnWalkReadedEv OnWalkReaded;


    // text 
    public Text Message;

    private List<string> logs;
    public static int asdf;

    public UnityEvent onConnected;
    public UnityEvent onDisconnected;
    public UnityEvent onConnectFailed;

    public IPlatformAdapter myPlatformAdapter;
    public MyWoawoaCallback myWoawoaCallback;
    public MyBluetoothAndroid myBluetoothIAndroid;
    public MyBluetoothNull myBluetoothNull;

    public bool IsConnected()
    {
        return myPlatformAdapter.IsConnected();
    }


    public void AddLog(string s)
    {
        return;
        asdf++;
        logs.Add("[" + asdf + "] " + s);
        if (logs.Count > 10)
            logs.RemoveAt(0);
        SetLogText();
    }

    public void SetLogText()
    {
        return;
        string s = "";
        for (int i = logs.Count - 1; i >= 0; i--)
        {
            s += logs[i] + "\n";
        }
        Message.text = s;
    }


    // Use this for initialization
    void Start()
    {
        Init();
        Debug.Log("====================Start MyAndoridWrapper====================");
    }


    public void Init()
    {
        Debug.Log("Started!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //OnGrabReaded = null;
        OnMotionReaded = null;
        //OnWalkReaded = null;

        asdf = 0;
        logs = new List<string>();

        // connectingEnded = false;

        Setting();
    }

    public void Setting()
    {
        Debug.Log("SETTINGGGGGGGGGGGGGGG!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //myWoawoaCallback.Init(this);
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("==========Android============");
            /*myPlatformAdapter = new MyBluetoothAndroid();
            Destroy(GetComponent<MyBluetoothIPhone>());*/
            myPlatformAdapter = myBluetoothIAndroid;
        }
        else
        {
            Debug.Log("==========Null============");
            myPlatformAdapter = myBluetoothNull;
        }
        myPlatformAdapter.Initialize(this);
    }



    /*    private void StartWaitForConnecting()
        {
            if (waitForConnectingCo != null)
                StopCoroutine(waitForConnectingCo);
            waitForConnectingCo = StartCoroutine(WaitForConnecting());
        }

        private bool connectingEnded = false;

        private Coroutine waitForConnectingCo;
        IEnumerator WaitForConnecting()
        {
            connectingEnded = false;
            while (!connectingEnded)
                yield return null;

            Debug.Log("WaitForConnecting Ended!");

            if (IsConnected())
            {
                Debug.Log("It's Connected!");
                onConnected.Invoke();

                // StopAllCoroutines();
                StartCoroutine(WaitAndSetting());
                Debug.Log("Coroutine Started!!");
            }
        }*/

    /*
        private void StartWaitForInit()
        {
            if (waitForInitCo != null)
                StopCoroutine(waitForInitCo);
            waitForInitCo = StartCoroutine(WaitForInit());
        }

        private bool initEnded = false;
        private bool initSuccessed = false;

        private Coroutine waitForInitCo;
        IEnumerator WaitForInit()
        {
            initEnded = false;
            initSuccessed = false;
            while (!initEnded)
                yield return null;

            Debug.Log("WaitForInit Ended!");

            if (initSuccessed)
            {
                Debug.Log("It's Inited!");

                yield return null;
                yield return null;

                Debug.Log("A");
                myPlatformAdapter = myBluetoothIPhone;
                Debug.Log("B");
                myPlatformAdapter.StartScan();
                Debug.Log("ACZXCASD");
                StartWaitForConnecting();
            } else
            {
                Debug.Log("Not Inited!!");
                OnConnectFailed();
            }
        }*/

    public void StartScan()
    {
        // StartWaitForInit();
        Debug.Log("Start Scan!!!");

        //initSuccessed = true;
        // initEnded = true;

        /*        BluetoothLEHardwareInterface.Initialize(true, false, () =>
                {
                    Debug.Log("Initialized !!!!!!!!!!!!!!!!!! ");
                    initSuccessed = true;
                    initEnded = true;
                }, (error) =>
                {
                    Debug.Log("Initializing FAILED !!!!!!!!!!!!!!!!!! ");
                    initSuccessed = false;
                    initEnded = true;
                });*/


        myPlatformAdapter.StartScan();
    }

    public void Disconnect()
    {
        myPlatformAdapter.Disconnect();
    }

    public void SendData(string data)
    {
        myPlatformAdapter.SendData(data);
    }

    // SN: WalkMode On
    // SF: WalkMode Off
    // WalkCount Callback은 N%d 형태로 옴 (N 다음 숫자가 카운트)
    // SR 하면 GetWalkCount고, SS하면 ResetWalkCount임

    public void StartWalk()
    {
        SendData("SN");
    }

    public void StopWalk()
    {
        SendData("SF");
    }

    public void ClearWalkData()
    {
        SendData("SS");
    }


    public void SetVibrationPower(int value)
    {
        SendData($"VS{value}");
    }

    public void SetVibrationStartTime(float time)
    {
        SendData($"VO{time}");
    }

    public void SetVibrationEndTime(float time)
    {
        SendData("VI" + time);
    }

    public void StartVibration()
    {
        SendData("VZ");
    }

    public void StartGyro()
    {
        myWoawoaCallback.SetGyroscopeType("M2");
        SendData("KS");
    }

    public void StopGyro()
    {
        SendData("KT");
    }

    public void StartGyro3D()
    {
        myWoawoaCallback.SetGyroscopeType("M3");
        SendData("KS");
    }


    public void InitGrab()
    {
        myWoawoaCallback.SetGrabPower(0, 100);
        myWoawoaCallback.GetLoadCells();
    }

    public void GetConfigs()
    {
        myWoawoaCallback.GetConfigList();
    }

    /*
    private void Update()
    {
#if UNITY_EDITOR

#else
        string s = GetJavaObject().Call<string>("getCallbackValues");
        if (s != null)
        {
            AddLog(s);
            string[] datas = s.Split('\n');
            for (int i = 0; i < datas.Length; i++)
            {
                HandleData(datas[i]);
            }
        }
#endif
    }

    private void HandleData(string s)
    {
        
    }
    */


    public void OnConnected()
    {
        Debug.Log("================ OnConnected ================");
        // connectingEnded = true;

        Debug.Log("It's Connected!");

        MyWoawoaAdapter.ins.StartGyro3DMode();
        MyWoawoaAdapter.ins.StartWalk();
        MyWoawoaAdapter.ins.ClearWalkData();
        // StopAllCoroutines();
        //StartCoroutine(WaitAndSetting());
        //Debug.Log("Coroutine Started!!");
        StartVibration();
    }

    IEnumerator WaitAndSetting()
    {
        yield return null;
        onConnected.Invoke();

        yield return new WaitForSecondsRealtime(1f);
        if (myPlatformAdapter.IsConnected())
        {
            Debug.Log("WOW, Init Grab!!");
            InitGrab();
            
            
        }
        yield return new WaitForSecondsRealtime(1f);
        if (myPlatformAdapter.IsConnected())
        {
            Debug.Log("WOW, Init Grab!! 222");
            InitGrab();
        }
    }

    public virtual void OnConnectFailed()
    {
        Debug.Log("================ OnConnectFailed ================");
        // isConnected = false;

        StopAllCoroutines();
        StartCoroutine(WaitAndFailed());
    }

    IEnumerator WaitAndFailed()
    {
        yield return null;
        OnConnectFailed2();
    }

    protected virtual void OnConnectFailed2()
    {
        onConnectFailed.Invoke();
    }

    public void OnDisconnected()
    {
        Debug.Log("================ OnDisconnected ================");

        StopAllCoroutines();
        StartCoroutine(WaitAndDisconnected());

        // isConnected = false;
    }

    IEnumerator WaitAndDisconnected()
    {
        yield return null;
        onDisconnected.Invoke();
    }

    public virtual void OnGrab(float power)
    {
        if (OnGrabReaded != null)
        {
            OnGrabReaded?.Invoke(power);
        }
    }

    public virtual void OnMotion(int dx, int dy)
    {
        if (OnMotionReaded != null)
        {
            OnMotionReaded?.Invoke(dx, dy);
        }
    }

    public virtual void OnWalk(int count)
    {
        if (OnWalkReaded != null)
        {
            OnWalkReaded?.Invoke(count);
        }
    }

    public virtual void OnAcc(double x, double y, double z, string state)
    {

    }

    public virtual void OnGyro3D(double x, double y, double z)
    {

    }
}
