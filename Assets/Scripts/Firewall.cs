using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewall : MonoBehaviour
{
    public Transform spawnpoint;
    public float bounceForce = 10f;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("FIREWALL");
            // Calculate the direction from the player to the firewall
            other.gameObject.transform.position = spawnpoint.position;
        }
    }
}
