using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishingFloat : MonoBehaviour
{
    public delegate void NibbleEvent();
    public NibbleEvent NibbleBegin;

    public TestLauncher fishingSystem;
    public Transform linePivot;

    private LineRenderer lineRenderer;
    private Vector3[] positions = new Vector3[2];

    public TMP_Text distanceUI;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        positions[0] = transform.position;
        positions[1] = linePivot.position;

        lineRenderer.SetPositions(positions);

        distanceUI.text = Vector3.Distance(positions[0], Vector3.zero).ToString("0.0");
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
            rigid.velocity = Vector3.zero;
            rigid.useGravity = false;
            rigid.isKinematic = true;
        }
    }
}
