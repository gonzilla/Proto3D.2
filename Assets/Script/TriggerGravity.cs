using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGravity : MonoBehaviour
{
    //Script pour les lumiére du début
    //[Tooltip("les effets que je vais utiliser")]
    public Vector3 NewGravity;
    public float forceGravity;
    public float RotationAngle;
    public float vitesseRotation;

    Quaternion Cible;
    Transform LaMoto;
    void changement() 
    {
        if (NewGravity!=Vector3.zero)
        {
            Physics.gravity = NewGravity * forceGravity;
        }
        if (RotationAngle!=0)
        {
            Cible = LaMoto.transform.rotation * Quaternion.Euler(0,0, RotationAngle);
            rotationCam();
        }
    
    }


    void rotationCam() 
    {
    

    
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            LaMoto = other.transform;
            changement();
        }
    }
}
