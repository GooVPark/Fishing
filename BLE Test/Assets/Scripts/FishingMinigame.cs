using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    [Header("Fishing Area")]
    [SerializeField] private Transform topBounds;
    [SerializeField] private Transform bottomBounds;


    [Header("Fish Setting")]
    [SerializeField] private Transform fish;
    [SerializeField] private float smoothMotion = 3f;
    [SerializeField] private float fishTimeRandomizer = 3f;

    private float fishPosition;
    private float fishSpeed;
    private float fishTimer;
    private float fishTargetPosition;


    [Header("Hook Setting")]
    [SerializeField] private Transform hook;
    [SerializeField] private float hookSize = 0.18f;
    [SerializeField] private float hookSpeed = 0.1f;
    [SerializeField] private float hookGravity = 0.05f;

    private float hookPosition;
    private float hookPullVelocity;

    private bool actionEnabled;

    private void FixedUpdate()
    {
        MoveFish();
        MoveHook();
    }

    private void MoveFish()
    {
        fishTimer -= Time.deltaTime;

        if(fishTimer < 0)
        {
            fishTimer = Random.value * fishTimeRandomizer;
            fishTargetPosition = Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishTargetPosition, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomBounds.position, topBounds.position, fishPosition);
    }

    private void MoveHook()
    {
        if(actionEnabled)
        {
            hookPullVelocity += hookSpeed * Time.deltaTime;
        }

        hookPullVelocity -= hookGravity * Time.deltaTime;

        hookPosition += hookPullVelocity;
        hookPosition = Mathf.Clamp(hookPosition, 0, 1);

        hook.position = Vector3.Lerp(bottomBounds.position, topBounds.position, hookPosition);
    }
}
