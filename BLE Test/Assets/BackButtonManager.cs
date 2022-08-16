using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BackButtonManager : MonoBehaviour
{
    private UnityEvent backButtonEvent;
    private bool isHome;

    public bool IsHome { get => isHome; set => isHome = value; }

    public void ResisterBackButtonEvent(Button button)
    {
        backButtonEvent = button.onClick;
        IsHome = false;
    }

    public void InitBackButtonEvent()
    {
        backButtonEvent = null;
    }

    public bool CheckIsSameEvent(UnityEvent ev)
    {
        if (backButtonEvent == null)
            return false;
        return ev == backButtonEvent;
    }

    public void InitBackButtonEventAndSetHome()
    {
        InitBackButtonEvent();
        IsHome = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsHome = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (backButtonEvent != null)
                backButtonEvent.Invoke();
            else if (IsHome)
                Application.Quit();
        }
    }
}
