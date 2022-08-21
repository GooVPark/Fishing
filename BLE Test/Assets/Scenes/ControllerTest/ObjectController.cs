using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Filters;

public class ObjectController : MonoBehaviour
{
    private float accX;
    private float accY;
    private float accZ;
    public TMP_Text accText;
    
    private float pitch;
    private float roll;
    private float yaw;
    public TMP_Text pryText;

    private float gyroX = 0.0f;
    private float gyroY = 0.0f;
    private float gyroZ = 0.0f;
    public TMP_Text gyroText;

    float angleX = 0.0f;
    float angleY = 0.0f;
    float angleZ = 0.0f;

    private float prevGyroX = 0.0f;
    private float prevGyroY = 0.0f;
    private float prevGyroZ = 0.0f;
    public TMP_Text angleText;

    private KalmanFilter filter;
    Vector3 prevGyro;
    private void Start()
    {
        filter = new KalmanFilter();
        MyWoawoaAdapter.OnAccReaded += GetAcceleration;
        MyWoawoaAdapter.OnGyroReaded += GetGyro;

        prevGyro = Vector3.zero;
    }

    private void Update()
    {
        pitch = Mathf.Atan(accX / Mathf.Sqrt((accY * accY) + (accZ * accZ))) * Mathf.Rad2Deg;
        roll = Mathf.Atan(accY / Mathf.Sqrt((accX * accX) + (accZ * accZ))) * Mathf.Rad2Deg;
        yaw = Mathf.Atan(accZ / Mathf.Sqrt((accX * accX) + (accZ * accZ))) * Mathf.Rad2Deg;

        if (gyroX > 0.1f || gyroX < -0.1f)
        {
            //angleX += GetAngle(prevGyroX, gyroX);
            angleX += Time.deltaTime * accX;
        }
        if (gyroY > 0.1f || gyroY < -0.1f)
        {
            //angleY += GetAngle(prevGyroY, gyroY);
            angleY += Time.deltaTime * accY;
        }
        if (gyroZ > 0.1f || gyroZ < -0.1f)
        {
            //angleZ += GetAngle(prevGyroZ, gyroZ);
            angleZ += Time.deltaTime * accZ;
        }

        

        prevGyroX = gyroX;
        prevGyroY = gyroY;
        prevGyroZ = gyroZ;

        accText.text = $"AccX: {accX}\nAccY: {accY}\nAccZ: {accZ}";
        pryText.text = $"Pitch: {pitch}\nRoll: {roll}\nYaw: {yaw}";
        gyroText.text = $"GyroX: {gyroX}\nGyroY: {gyroY}\nGyroZ: {gyroZ}";
        angleText.text = $"AngleX: {angleX}\nAngleY: {angleY}\nAngleZ: {angleZ}";
    }

    public void GetAcceleration(double x, double y, double z)
    {

        accX = (float)x;
        accY = (float)y;
        accZ = (float)z;

        //accX = filter.FilterValue(accX);
        //accY = filter.FilterValue(accY);
        //accZ = filter.FilterValue(accZ);
    }

    public void GetGyro(double x, double y, double z)
    {
        gyroX = (float)x;
        gyroY = (float)y;
        gyroZ = (float)z;

        gyroX = filter.FilterValue(gyroX);
        gyroY = filter.FilterValue(gyroY);
        gyroZ = filter.FilterValue(gyroZ);
    }

    public float GetAngle(float prev, float current)
    {
        return ((current + prev) * Time.deltaTime) / 2f;
    }
}
