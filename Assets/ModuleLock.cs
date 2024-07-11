using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ModuleLock : MonoBehaviour
{
    // Start is called before the first frame update
    public string ModuleNameVar;
    private int percentage;
    public GameObject locker;
    void OnEnable()
    {
        percentage = DialogueLua.GetVariable(ModuleNameVar).AsInt;
        Debug.Log(ModuleNameVar + " "+ percentage);
        if (percentage >= 70)
        {
            locker.SetActive(false);
        }
        else
        {
            locker.SetActive(true);
        }
    }

}
