using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    public GameObject fxObject;
    public ParticleSystem waterSpray;

    public GameObject fishUI;

    private void OnTriggerEnter(Collider other)
    {
        fxObject.SetActive(true);
        waterSpray.Play();

        fishUI.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        fxObject.transform.position = other.ClosestPoint(other.transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        fishUI.SetActive(false);
    }
}
