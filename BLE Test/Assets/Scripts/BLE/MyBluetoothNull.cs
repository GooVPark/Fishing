using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBluetoothNull : MyAndroidWrapper.IPlatformAdapter
{
    private MyAndroidWrapper myAndroidWrapper;

    public override void Disconnect()
    {

    }

    public override void Initialize(MyAndroidWrapper myAndroidWrapper)
    {
        this.myAndroidWrapper = myAndroidWrapper;
    }

    public override bool IsConnected()
    {
        return false;
    }

    public override void SendData(string data)
    {

    }

    public override void StartScan()
    {
        OnConnectFailed();
    }

    public override void OnConnected()
    {
        myAndroidWrapper.OnConnected();
    }

    public override void OnConnectFailed()
    {
        myAndroidWrapper.OnConnectFailed();
    }

    public override void OnDisconnected()
    {
        myAndroidWrapper.OnDisconnected();
    }

    public override bool IsBluetoothEnabled()
    {
        return false;
    }
}
