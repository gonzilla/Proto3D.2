using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScriptFollow : MonoBehaviour
{
    //Public variable
    public float VitesseDeplacement;
    public float VitesseRotation;
    public Transform MotoToFollow;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void CameraMovement()
    {
        transform.position = Vector3.Lerp(transform.position, MotoToFollow.position, VitesseDeplacement);
        transform.rotation = Quaternion.Slerp(transform.rotation, MotoToFollow.rotation, VitesseRotation);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
