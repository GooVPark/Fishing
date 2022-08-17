using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public delegate void PlayerAnimationEventDelegate();
    public static PlayerAnimationEventDelegate CastEndEvent;
    public static PlayerAnimationEventDelegate ThrowEvent;
    public static PlayerAnimationEventDelegate ReloadEndEvent;

    public void CastEnd()
    {
        CastEndEvent?.Invoke();
    }

    public void Throw()
    {
        ThrowEvent?.Invoke();
    }

    public void ReloadEnd()
    {
        ReloadEndEvent?.Invoke();
    }
}
