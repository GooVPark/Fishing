using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void CatchEvent();
    public static CatchEvent Catch;

    public delegate void MissEvent();
    public static MissEvent Miss;

    public delegate void StageEndEvent();
    public static StageEndEvent StageEnd;

    public delegate void RestartEvent();
    public static RestartEvent OnRestart;

    public GameObject catchUI;
    public GameObject missUI;

    // Start is called before the first frame update
    void Start()
    {
        Catch += OnCatch;
        Miss += OnMiss;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        catchUI.SetActive(false);
        missUI.SetActive(false);
        OnRestart?.Invoke();
    }

    public void OnCatch()
    {
        catchUI.SetActive(true);
    }

    public void OnMiss()
    {
        missUI.SetActive(true);
    }
}
