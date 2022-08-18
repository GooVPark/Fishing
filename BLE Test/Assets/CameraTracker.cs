using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public GameObject camera;

    private Vector3 startScale;
    public float distance = 3;

    private void Start()
    {
        startScale = transform.localScale;
    }

    private void Update()
    {
        float distance = Vector3.Distance(camera.transform.position, transform.position);
        Vector3 newScale = startScale * distance / this.distance;
        transform.localScale = newScale;

        transform.rotation = camera.transform.rotation;
        transform.position = new Vector3(transform.position.x, transform.position.y * newScale.y, transform.position.z);
    }
}
