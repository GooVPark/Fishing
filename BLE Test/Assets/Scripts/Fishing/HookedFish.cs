using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookedFish : MonoBehaviour
{
    public Transform hook;

    private void Start()
    {
        GameManager.Catch += OnCatched;
        GameManager.OnRestart += OnRestart;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = hook.position;
    }

    public void OnCatched()
    {
        gameObject.SetActive(true);
    }

    public void OnRestart()
    {
        gameObject.SetActive(false);
    }
}
