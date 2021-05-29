using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testParam : MonoBehaviour
{
    [Range(0, 1)]
    public float RPMvalue;
    [Range(0, 1)]
    public float AcceInputValue;
    [FMODUnity.EventRef]
    public string monevent;
    
    FMOD.Studio.EventInstance MonFdamour;
    void Start()
    {
        MonFdamour=FMODUnity.RuntimeManager.CreateInstance(monevent);
        MonFdamour.start();
    }

    // Update is called once per frame
    void Update()
    {
        
        MonFdamour.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
        MonFdamour.setParameterByName("RPM", RPMvalue);
        MonFdamour.setParameterByName("AccelInput", AcceInputValue);
    }
}
