using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReversibleEventInvoker : SimpleEventInvoker
{
    public UnityEvent reverseEvent;

    public void ReverseEvent()
    {
        reverseEvent.Invoke();
    }


    public void ReverseEvent(float waitTime)
    {
        Invoke("ReverseEvent", waitTime);
    }
}
