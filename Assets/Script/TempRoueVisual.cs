using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempRoueVisual : MonoBehaviour
{
    //Public variable
    public Transform Roue;
    public Transform Moto;
    public Vector3 offset;
    //Local variable
    WheelCollider WC;
    void Start()
    {
        WC = Roue.GetComponent<WheelCollider>();
    }

    
    void Update()
    {
        transform.position = Roue.position+offset;
        transform.localRotation = Quaternion.Euler(0, WC.steerAngle, 90);
        //transform.rotation = Roue.rotation;
    }
}
