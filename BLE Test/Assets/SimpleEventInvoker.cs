using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleEventInvoker : MonoBehaviour
{
    public UnityEvent myEvent;

    public void InvokeEvent()
    {
        myEvent.Invoke();
    }

    public void InvokeEvent(float waitTime)
    {
        Invoke("InvokeEvent", waitTime);
    }
}
