using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowYRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.identity;

        // Set the rotation of the object based on the target's y rotation
        transform.rotation = Quaternion.Euler(90f, 0f, target.eulerAngles.y);
     
    }
}
