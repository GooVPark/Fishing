using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState gameState;
    public GameState GameState
    {
        get { return gameState; }
        set
        {
            if(gameState != null)
            {
                gameState.OnStateExit();
            }

            gameState = value;
            gameState.OnStateEnter();
        }
    }

    public GameState ready;

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

    public GameObject locomotion;
    

    // Start is called before the first frame update
    void Start()
    {
        Catch += OnCatch;
        Miss += OnMiss;

        GameState = ready;
    }

    // Update is called once per frame
    void Update()
    {
        GameState.OnStateUpdate();
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
