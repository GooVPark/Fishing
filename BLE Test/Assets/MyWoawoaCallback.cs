using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWoawoaCallback : MonoBehaviour
{
    public static MyWoawoaCallback ins = null;

    private void Awake()
    {
        ins = this;
        willHandleDatas = new List<string>();
    }

    public MyAndroidWrapper myAndroidWrapper;
    private List<string> willHandleDatas;

    private float defaultLoadCellVal = 16267000f;
    private float loadCellInitVal = 16267000f;
    private float defaultLoadCellMidVal = 0f;
    private float loadCellMidVal = 0f;
    private float loadCellMaxVal = 12600000f;
    private float slope = -78000f;

    private int loadCellLowValue = 0;
    private int loadCellHighValue = 100;

    private double acc_x = 0.0;
    private double acc_y = 0.0;
    private double acc_z = 0.0;
    private double gyro_x = 0.0;
    private double gyro_y = 0.0;
    private double gyro_z = 0.0;

    private int deltaX = 0;
    private int deltaY = 0;

    private int BMI2_GYR_RANGE_2000 = 0;

    private Vector3 gyroValue;
    private Vector3 accValue;

    /*
    public MooziCallback(Application application)
    {
        bleManager = BleManager.getInstance();
        bleManager.init(application);
        bleManager.enableLog(false)
                .setReConnectCount(1, 5000)
                .setConnectOverTime(20000)
                .setOperateTimeout(5000);
    }
    */


    /*
    public void setScanRule()
    {
        BleScanRuleConfig scanRuleConfig = new BleScanRuleConfig.Builder()
                .setServiceUuids(new UUID[] { BleUUIDs.Service.USERVICE })      // 지정된 서비스의 장치 만 검색, 선택 사항
                .setScanTimeOut(20000)              // 스캔 제한 시간, 선택 사항, 기본값 10 초
                .build();
        BleManager.getInstance().initScanRule(scanRuleConfig);
    }
    */


    private short TwoCompleteToDecimal(string hexStr)
    {
        char dec = (char)Int32.Parse(hexStr, System.Globalization.NumberStyles.HexNumber);
        if ((dec & 0x8000) == 0)
        {
            return (short)dec;
        }
        else
        {
            return (short)((~dec + 0x01) * -1);
        }

    }

    private static string[] seperatorLIFront = new string[] { "LI[" };
    private static string[] seperatorLMFront = new string[] { "LM[" };

    private void Update()
    {
        if (willHandleDatas.Count > 0)
        {
            for (int i = 0; i < willHandleDatas.Count; i++)
            {
                DataProcessReal(willHandleDatas[i]);
            }
            willHandleDatas.Clear();
        }
    }

    public void DataProcess(string str)
    {
        //Debug.Log("DataProcessDataProcessDataProcessDataProcessDataProcessDataProcessDataProcessDataProcessDataProcessDataProcessDataProcessDataProcessDataProcessDataProcess");
        willHandleDatas.Add(str);
    }

    public void DataProcessReal(string str)
    {
        //Debug.Log("m + " + str);
        //return;
        //Debug.Log("WOWWOW + " + str);
        //myAndroidWrapper.AddLog(str);
        //return;

        //Dlog.i("Debug :: " + str);
        switch (str[0])
        {
            case 'L':
               // Debug.Log("악력 측정 중");
                //악력크기 값
                if (str.Contains("L["))
                {
                //    Debug.Log("================ Value: L ================");
                    //                    string[] frontSplit = str.Split("L\\[");
                    //                    string[] backSplit = frontSplit[1].Split(seperatorBack, StringSplitOptions.None);
                    //                    if (grabPowerCallback != null)
                    //                        grabPowerCallback.outCallbackString(backSplit[0]);
                }
                else if (str.Contains("LI["))
                {
             //       Debug.Log("================ Value: LI ================");
                    string[] frontSplit = str.Split(seperatorLIFront, StringSplitOptions.None);
                    string[] backSplit = frontSplit[1].Split(']');
                    loadCellInitVal = float.Parse(backSplit[0]);
                    //                    loadCellMaxVal = loadCellInitVal + (slope) * 50.0f;
                    //Dlog.w("loadCellInitVal = " + loadCellInitVal);
                    //                    //Dlog.w("loadCellMaxVal = " + loadCellMaxVal);
                }
                else if (str.Contains("LM["))
                {
             //       Debug.Log("================ Value: LM ================");
                    string[] frontSplit = str.Split(seperatorLMFront, StringSplitOptions.None);
                    string[] backSplit = frontSplit[1].Split(']');
                    string[] array1 = backSplit[0].Split(',');
                    float kg = float.Parse(array1[0]);
                    if (!array1[1].Equals("0") && !array1[0].Equals("0"))
                    {
                        loadCellMidVal = float.Parse(array1[1]);
                        //Dlog.w("loadCellMidVal = " + loadCellInitVal);

                        // mac locdcell value 계산
                        loadCellMaxVal = loadCellInitVal - ((loadCellInitVal - loadCellMidVal) / kg * 50.0f);
                        //Dlog.w("loadCellMaxVal = " + loadCellMaxVal);
                    }
                    else
                    {
                        //Dlog.e("float.Parse(array1[1]) = 0 exception ");
                    }
                }
                else
                {
              //      Debug.Log("======================= Grab Raw Data =======================");
                    float low = 0.0f;
                    float high = 0.0f;

                    if (loadCellLowValue + (100 - loadCellHighValue) <= 100)
                    {
                        if (loadCellLowValue != 0)
                        {
                            low = (loadCellInitVal - loadCellMaxVal) * loadCellLowValue / 100.0f;
                            //Dlog.w("low = " + low);

                        }
                        if (loadCellHighValue != 100)
                        {
                            high = (loadCellInitVal - loadCellMaxVal) * (100 - loadCellHighValue) / 100.0f;
                            //Dlog.w("high = " + high);
                        }
                    }
                    float loadCellMaxValue = loadCellMaxVal + high;
                    float loadCellMinValue = loadCellInitVal - low;

                    float tempval = float.Parse(str.Substring(1)) - loadCellMaxValue;
                    //                    Float denominator = loadCellInitVal - loadCellMaxVal;

                    float res = 1.0f - tempval / (loadCellMinValue - loadCellMaxValue);
                    //                    Float value = 1.0f - numerator / denominator;
                    //Dlog.w("res = " + res);

                    /*
                    if (res < 0.0f) {
                        res = 0.0f;
                    } else if (res > 1.0f) {
                        res = 1.0f;
                    }
    //                    //Dlog.w("res = " + res);
                    */

                    myAndroidWrapper.OnGrab(-res);
                }
                break;

            case 'A':
                {
               //     Debug.Log("=======================Acc Activated=======================");
                    string[] array = str.Split(',');
                    try
                    {
                        int x = TwoCompleteToDecimal(array[0].Substring(2));
                        int y = TwoCompleteToDecimal(array[1]);
                        int z = TwoCompleteToDecimal(array[2]);
                        this.acc_x = lsb_to_mps2(x, 2.0, 16);
                        this.acc_y = lsb_to_mps2(y, 2.0, 16);
                        this.acc_z = lsb_to_mps2(z, 2.0, 16);

                        string d = array[3].Substring(0, 1);
                     //   Debug.Log(d);



                        myAndroidWrapper.OnAcc(acc_x, acc_y, acc_z, d);
                    }
                    catch (Exception e)
                    {
                        int x = TwoCompleteToDecimal(array[0].Substring(2));
                        int y = TwoCompleteToDecimal(array[1]);
                        int z = TwoCompleteToDecimal(array[2].Substring(0, 4));

                        this.acc_x = lsb_to_mps2(x, 2.0, 16);
                        this.acc_y = lsb_to_mps2(y, 2.0, 16);
                        this.acc_z = lsb_to_mps2(z, 2.0, 16);

                        myAndroidWrapper.OnAcc(acc_x, acc_y, acc_z, "0");
                    }
                }
                break;
            case 'G':
                {
                    //자이로
                    //Debug.Log("G " + str);
                    string[] array = str.Split(',');
                    int parseStart = 2;
                    int state = 0;
                    if (str[1] == 'S')
                    {
                        state = 0;
                        parseStart = 3;
                    }
                    else if (str[1] == 'E')
                    {
                        state = 2;
                        parseStart = 3;
                    }
                    else
                    {
                        state = 1;
                        parseStart = 2;
                    }

                    int x = TwoCompleteToDecimal(array[0].Substring(parseStart));
                    int y = TwoCompleteToDecimal(array[1]);
                    int z = TwoCompleteToDecimal(array[2].Substring(0, 4));

                    this.gyro_x = lsb_to_dps(x, 180.0, 16);
                    this.gyro_y = lsb_to_dps(y, 180.0, 16);
                    this.gyro_z = lsb_to_dps(z, 180.0, 16);

                    myAndroidWrapper.OnGyro3D(gyro_x, gyro_y, gyro_z);
                }
                break;

            case 'D':
                {
                    string[] array = str.Split(',');
                    int state = 1;
                    int parseStart = 2;
                    if (str[1] == 'S')
                    {
                        state = 0;
                        parseStart = 3;
                    }
                    else if (str[1] == '[')
                    {
                        state = 1;
                        parseStart = 2;
                    }
                    else if (str[1] == 'E')
                    {
                        state = 2;
                        parseStart = 3;
                    }


                    int dx = int.Parse(array[0].Substring(parseStart));
                    int dy = int.Parse(array[1].Substring(0, 4));

                    if (Mathf.Abs(deltaX) < 5 && Mathf.Abs(dx) > 5 || Mathf.Abs(deltaY) < 5 && Mathf.Abs(dy) > 5)
                    {
                        state = 0;
                    }
                    if (Mathf.Abs(deltaX) > 5 && Mathf.Abs(dx) < 5 || Mathf.Abs(deltaY) > 5 && Mathf.Abs(dy) < 5)
                    {
                        state = 2;
                    }

                    myAndroidWrapper.OnMotion(dx, dy);
                    break;
                }
            case 'N':
                {
                    Debug.Log("======================================Walk=================================");
                    Debug.Log("A " + str);
                    myAndroidWrapper.OnWalk(int.Parse(str.Substring(1)));
                    break;
                }
            default:
                //Dlog.w("Data Process null");
                break;
        }

    }


    public void GetConfigList()
    {
        myAndroidWrapper.SendData("CL");
    }

    public void GetLoadCells()
    {
        myAndroidWrapper.SendData("LI");
        myAndroidWrapper.SendData("LM");
    }


    public void SetGrabPower(int min, int max)
    {
        myAndroidWrapper.SendData("LL" + min);
        myAndroidWrapper.SendData("LH" + max);

        loadCellLowValue = min;
        loadCellHighValue = max;
    }

    public void SetGyroscopeType(string mode)//, IntInterface intInterface)
    {
        myAndroidWrapper.SendData(mode);//, intInterface);
    }

    private const float GRAVITY_EARTH = 9.80665F;

    private double lsb_to_mps2(int loc, double g_range, int bit_width)
    {
        double half_scale = (double)(1 << bit_width) / 2.0;
        return (GRAVITY_EARTH * (double)loc * g_range) / half_scale;
    }

    private double lsb_to_dps(int loc, double dps, int bit_width)
    {
        double half_scale = (double)(1 << bit_width) / 2.0;
        return (dps / (half_scale) + (double)BMI2_GYR_RANGE_2000) * (double)loc;
    }

}
