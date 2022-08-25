using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishingFloat : MonoBehaviour
{
    public delegate void NibbleEvent();
    public NibbleEvent NibbleBegin;

    public delegate void CastEndEvent();
    public CastEndEvent CastEnd;

    public TestLauncher fishingSystem;
    public Transform linePivot;

    private LineRenderer lineRenderer;
    private Vector3[] positions = new Vector3[2];

    public TMP_Text distanceUI;
    public float distance;

    public bool isFloating;
    public float speed;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
    }

    public void Flight(Vector3 destination)
    {
        StartCoroutine(FlightCo(destination));
    }

    private IEnumerator FlightCo(Vector3 destination)
    {

        while (Vector3.Distance(transform.position, destination) != Mathf.Epsilon)
        {
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);
            distance = Vector3.Distance(transform.position, destination) + 1f;
            distanceUI.text = distance.ToString("0.0");
            yield return null;
        }
    }

    //나중에 Bite로 이름 바꿀것
    public void Hit()
    {
        GetComponent<SphereCollider>().enabled = false;

        NibbleBegin?.Invoke();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SwimArea"))
        {
            Rigidbody rigid = GetComponent<Rigidbody>();
            StopAllCoroutines();
            CastEnd?.Invoke();
        }
    }
}
