using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingObject : MonoBehaviour
{
    public Text accText;

    private delegate Vector2 GetDirectionEv(float x, float y);
    private GetDirectionEv GetDirection;

    public bool isControlXReversed;
    public bool isControlYReversed;

    [Header("- Controll Settings")]
    public float acc_m;// = 0.2f;
    public float acc_speed;// = 1.2f;

    public float mot_m;// = 0.002f;
    public float mot_speed;// = 1.2f;

    public float grabMinThreshold;// = 0.015f;
    public float grabMaxThreshold;// = 0.04f;
    public float grabUpThreshold;// = 0.013f;


    private float my_acc_x;
    private float my_acc_y;
    private float my_mot_x;
    private float my_mot_y;
    private float my_gyr_x;
    private float my_gyr_y;
    private float my_gyr_z;
    private float grabVal;
    private bool isGrabDowned;

    private Vector2 direction;



    private Vector2 inputValue;


    private Vector3 orgVel;

    float speed2 = 7f;

    Rigidbody myrb;
    public Transform crashedSpeed;
    public Transform boostSpeed;
    public Transform rotateTarget;
    float radius = 0.5f;
    private void Awake()
    {
        isGrabDowned = false;
        SetGetDirection(isControlXReversed, isControlYReversed);
        StartGettingInput();

        Debug.Log((double)(1 << 16));

    }

    void Update()
    {
        //HandleInput();
        //Move();
    }


    private void Move()
    {
        Vector3 direction = new Vector3(inputValue.x, 0, inputValue.y);
       // transform.rotation *= Quaternion.Euler(inputValue.x, 0, 0);
    }

    public void StartGettingInput()
    {
        MyWoawoaAdapter.OnAccReaded += GetAccValue;
        MyWoawoaAdapter.OnMotionAccumulatedReaded += GetMotionValue;
        MyWoawoaAdapter.OnGrabReaded += GetGrabValue;
        
    }

    private void GetGyroValue(double x, double y, double z)
    {

    }

    private void GetAccValue(double x, double y, double z)
    {
        my_acc_x = (float)z;
        my_acc_y = (float)-x;

        accText.text = new Vector2(my_acc_x, my_acc_y).ToString();
    }

    private void GetMotionValue(float x, float y)
    {
        my_mot_x = -x;
        my_mot_y = y;

        accText.text = new Vector2(my_mot_x, my_mot_y).ToString();
    }

    public void GetGrabValue(float val)
    {
        grabVal = val;
        if (!isGrabDowned)
        {
            if (val > grabMinThreshold)
            {
                if (val > grabMaxThreshold)
                    val = grabMaxThreshold;
                OnGrabDown((val - grabMinThreshold) / (grabMaxThreshold - grabMinThreshold));
            }
        }
        else
        {
            if (val < grabUpThreshold)
            {
                OnGrabUp();
            }
            else
            {
                //if (boostControl != null)
                //{
                //    if (val > grabMaxThreshold)
                //        val = grabMaxThreshold;

                //    boostControl.SetGrabValue((val - grabMinThreshold) / (grabMaxThreshold - grabMinThreshold));
                //}
            }
        }
    }

    public void OnGrabDown(float val)
    {
        isGrabDowned = true;
    }

    public void OnGrabUp()
    {
        isGrabDowned = false;
    }


    public void SetGetDirection(bool xr, bool yr)
    {
        if (!xr && !yr)
            GetDirection = GetDirectionXoYo;
        else if (xr && !yr)
            GetDirection = GetDirectionXrYo;
        else if (!xr && yr)
            GetDirection = GetDirectionXoYr;
        else
            GetDirection = GetDirectionXrYr;
    }

    private Vector2 GetDirectionXoYo(float x, float y)
    {
        return new Vector2(x, y);
    }

    private Vector2 GetDirectionXrYo(float x, float y)
    {
        return new Vector2(-x, y);
    }

    private Vector2 GetDirectionXoYr(float x, float y)
    {
        return new Vector2(x, -y);
    }

    private Vector2 GetDirectionXrYr(float x, float y)
    {
        return new Vector2(-x, -y);
    }

    void HandleInput()
    {

        if (GetDirection == null)
            return;

        if (MyWoawoaAdapter.ins.isGyro3DMode)
        {
            direction = GetDirection(my_acc_x, my_acc_y) * acc_m;
            if (direction.sqrMagnitude > 1f)
                direction.Normalize();
            direction *= acc_speed;
        }
        else
        {
            direction = GetDirection(my_mot_x, my_mot_y) * mot_m;
            if (direction.sqrMagnitude > 1f)
                direction.Normalize();
            direction *= mot_speed;
        }

        inputValue = direction;
    }
}
