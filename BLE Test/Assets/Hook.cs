using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public GameObject fxObject;
    public ParticleSystem waterSplash;

    private void OnTriggerEnter(Collider other)
    {
        fxObject.transform.position = other.ClosestPoint(transform.position);
        waterSplash.Play();
    }
}
