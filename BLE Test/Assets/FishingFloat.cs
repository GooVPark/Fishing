using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingFloat : MonoBehaviour
{
    public TestLauncher fishingSystem;

    public void Hit()
    {
        GetComponent<SphereCollider>().enabled = false;

        fishingSystem.OnHit();
    }
}
