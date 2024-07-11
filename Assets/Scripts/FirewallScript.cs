using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallScript : MonoBehaviour
{
    public GameObject[] targetObjects;
    public float activeTime = 2f; // Time in seconds to keep the objects active
    public float inactiveTime = 1f; // Time in seconds to keep the objects inactive

    void Start()
    {
        // Call ActivateDeactivateRoutine() every (activeTime + inactiveTime) seconds
        InvokeRepeating("ActivateDeactivateRoutine", 0f, activeTime + inactiveTime);
    }

    void ActivateDeactivateRoutine()
    {
        // Activate all target objects in the array
        SetObjectsActive(true);

        // Deactivate all target objects in the array after activeTime
        Invoke("DeactivateObjects", activeTime);
    }

    void DeactivateObjects()
    {
        // Deactivate all target objects in the array
        SetObjectsActive(false);
    }

    void SetObjectsActive(bool active)
    {
        foreach (GameObject target in targetObjects)
        {
            target.SetActive(active);
        }
    }
}
