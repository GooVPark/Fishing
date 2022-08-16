using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public enum FishState { Idle, Bite }
    public FishState fishState;

    private Transform fishingFloat;
    private Vector3 floatPosition;

    private Vector3 runVector;

    // Start is called before the first frame update
    void Start()
    {
        fishingFloat = FindObjectOfType<FishingFloat>(true).transform;
        floatPosition = new Vector3(fishingFloat.position.x, transform.position.y, fishingFloat.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        switch (fishState)
        {
            case FishState.Idle:

                if (fishingFloat != null)
                {
                    if (fishingFloat.gameObject.activeSelf)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, floatPosition, Time.deltaTime);
                    }
                }

                break;
            case FishState.Bite:

                transform.position += runVector;

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FishingFloat"))
        {
            other.GetComponent<FishingFloat>().Hit();
            fishState = FishState.Bite;

            runVector = transform.position.normalized * Time.deltaTime * 1.3f;
        }
    }
}
