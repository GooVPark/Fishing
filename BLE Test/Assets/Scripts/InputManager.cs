using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Input Properties

    private static Vector3 accValue;
    public static Vector3 AccValue => accValue;

    private static string accState;
    public static string AccState => accState;

    private static Vector3 gyroValue;
    public static Vector3 GyroValue => gyroValue;

    private static float grabPower;
    public static float GrabPower => grabPower;

    private static int walkCount;
    public static int WalkCount => walkCount;

    #endregion

    private void Start()
    {
        MyWoawoaAdapter.OnAccReaded += GetAcc;
        MyWoawoaAdapter.OnAccStateReaded += GetAccState;
        MyWoawoaAdapter.OnGyroReaded += GetGyro;
        MyWoawoaAdapter.OnGrabReaded += GetGrab;
        MyWoawoaAdapter.OnWalkReaded += GetWalkCount;
    }

    #region Input Methods

    public void GetAcc(double x, double y, double z)
    {
        accValue.x = (float)x;
        accValue.y = (float)y;
        accValue.z = (float)z;
    }
    public void GetAccState(string state)
    {
        accState = state;
    }
    public void GetGyro(double x, double y, double z)
    {
        gyroValue.x = (float)x;
        gyroValue.y = (float)y;
        gyroValue.z = (float)z;
    }
    public void GetGrab(float power)
    {
        grabPower = power;
    }
    public void GetWalkCount(int count)
    {
        walkCount = count;
    }

    #endregion
}
