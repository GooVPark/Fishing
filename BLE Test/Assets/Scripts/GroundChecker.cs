using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public GameObject warnningUI;

    public LayerMask layerMask;
    private static bool isWater;
    public static bool IsWater 
    { 
        get => isWater; 
    }
    
    void Update()
    {
        if(Physics.Raycast(transform.position, Vector3.down, 10f, layerMask))
        {
            isWater = false;
        }
        else
        {
            isWater = true;
        }

        Debug.DrawRay(transform.position, Vector3.down * 10f);
        Debug.Log(isWater);
        warnningUI.SetActive(!isWater);
    }

}
