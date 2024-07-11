using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCModuleScript : MonoBehaviour
{
    public AudioSource step;
    public void StepSound()
    {
        step.Play();
    }

    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
