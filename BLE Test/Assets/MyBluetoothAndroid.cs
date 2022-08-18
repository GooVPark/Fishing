using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBluetoothAndroid : MyAndroidWrapper.IPlatformAdapter
{
    public static MyBluetoothAndroid ins = null;

    public const string K_LOG = "LOG";
    public const string K_RAW = "RAW";

    public const string K_SCAN_STARTED = "SCN";
    public const string K_SCANNING = "SIN";
    public const string K_SCANNED = "SED";

    public const string K_CONNECT_FAILED = "COF";
    public const string K_CONNECT_SUCCEEDED = "COS";
    public const string K_DISCONNECTED = "DIC";

    public const string K_GRAB = "GRB";
    public const string K_MOTION = "MOT";
    public const string K_GYRO3D = "G3D";
    public const string K_ACC = "ACC";
    public const string K_WALK = "WLK";

    public static bool StringToBool(string s)
    {
        return s == "P";
    }

    public class AndroidUnityCallback : AndroidJavaProxy
    {
        public AndroidUnityCallback() : base("com.example.myble.UnityCallback") { }

        public void onLog(string str)
        {
            //Debug.Log("~~~!!ON LOG!!~~~ : " + str);
            ins.OnLog(str);
        }

        public void onConnected()
        {
            ins.OnConnected();
        }
        public void onConnectFailed()
        {
            ins.OnConnectFailed();
        }

        public void onDisconnected()
        {
            ins.OnDisconnected();
        }

        public void dataProcess(string str)
        {
            //Debug.Log("DATA PROCESS!!!!!!!!!!! " + str);
            MyWoawoaCallback.ins.DataProcess(str);
        }

        /*
public void HandleCallbackValue(string val)
{
string code = val.Substring(0, 3);
Debug.Log("CODE: " + code + ", FULL: " + val);


if (code.Equals(K_MOTION))
{
    string[] els = val.Split('|');

    bool b1 = int.TryParse(els[1], out int dx);
    bool b2 = int.TryParse(els[2], out int dy);

    if (b1 && b2)
        ins.OnMotion(dx, dy);
    else
        Debug.LogError("Motion ÆÄ½Ì ½ÇÆÐ! : " + val);
}
else if (code.Equals(K_GYRO3D))
{
    string[] els = val.Split('|');
    ins.OnGyro3D(els);
}
else if (code.Equals(K_ACC))
{
    string[] els = val.Split('|');
    ins.OnAcc(els);
}
else if (code.Equals(K_GRAB))
{
    bool b = float.TryParse(val.Substring(4), out float result);
    if (b)
        ins.OnGrab(result);
    else
        Debug.LogError("Grab ÆÄ½Ì ½ÇÆÐ! : " + val);
}
else if (code.Equals(K_WALK))
{
    bool b = int.TryParse(val.Substring(4), out int result);
    if (b)
        ins.OnWalk(result);
    else
        Debug.LogError("Walk ÆÄ½Ì ½ÇÆÐ! : " + val);
}

if (code.Equals(K_SCAN_STARTED))
{
    if (!StringToBool(val[4].ToString()))
        ins.OnConnectFailed();
}
else if (code.Equals(K_CONNECT_FAILED))
{
    ins.OnConnectFailed();
}
else if (code.Equals(K_CONNECT_SUCCEEDED))
{
    ins.OnConnected();
}
else if (code.Equals(K_DISCONNECTED))
{
    ins.OnDisconnected();
}

    }*/

    }


    // android object
    private AndroidJavaObject AndroidObject = null;
    private AndroidUnityCallback callbackObject = null;

    private bool isConnected;

    public MyAndroidWrapper myAndroidWrapper;

    private AndroidJavaObject GetJavaObject()
    {
        if (AndroidObject == null)
        {
            AndroidObject = new AndroidJavaObject("com.example.myble.ui.MainActivity");
        }
        return AndroidObject;
    }

    private void Start()
    {
        // Retrieve current Android Activity from the Unity Player
        AndroidJavaClass jclass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = jclass.GetStatic<AndroidJavaObject>("currentActivity");

        // Pass reference to the current Activity into the native plugin,
        // using the 'setActivity' method that we defined in the ImageTargetLogger Java class
        GetJavaObject().Call("setActivity", activity);
        GetJavaObject().Call("initialize");

        isConnected = false;
        ins = this;

        callbackObject = new AndroidUnityCallback();
        GetJavaObject().Call("setCallBackListener", callbackObject);
    }

    public override void Initialize(MyAndroidWrapper myAndroidWrapper)
    {
        this.myAndroidWrapper = myAndroidWrapper;
    }

    public override bool IsConnected()
    {
        return isConnected;
    }

    public override void StartScan()
    {
        Debug.Log("Scan Start?");
        GetJavaObject().Call("mooziScan");
    }

    public override void Disconnect()
    {
        myAndroidWrapper.StopGyro();
        GetJavaObject().Call("disconnect");
    }

    public override void SendData(string data)
    {
        if (isConnected)
            GetJavaObject().Call("sendData", data);
    }

    public override void OnConnected()
    {
        isConnected = true;
        myAndroidWrapper.OnConnected();
    }

    public override void OnConnectFailed()
    {
        isConnected = false;
        myAndroidWrapper.OnConnectFailed();
    }

    public override void OnDisconnected()
    {
        isConnected = false;
        myAndroidWrapper.OnDisconnected();
    }

    public override bool IsBluetoothEnabled()
    {
        return GetJavaObject().Call<bool>("isBlueEnable");
    }

    public void OnLog(string str)
    {
        myAndroidWrapper.AddLog(str);
    }
}
