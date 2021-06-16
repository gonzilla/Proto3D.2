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
    public float vitesserotation;
    
    
    Quaternion Cible;
    Transform LaMoto;
    void changement() 
    {
        if (NewGravity!=Vector3.zero)
        {
            //Physics.gravity = Vector3.down * forceGravity;
            LaMoto.GetComponent<GestionMotoControlleur>().FakeGravity = NewGravity;
            LaMoto.GetComponent<GestionMotoControlleur>().FakeGravityMultiplier = forceGravity;
        }
        if (RotationAngle!=0)
        {
            Cible = LaMoto.transform.rotation * Quaternion.Euler(0, 0, RotationAngle);
            rotateCam();
        }
    
    }

    void rotateCam() 
    {
        
        if (LaMoto.transform.rotation!= Cible)
        {
            LaMoto.transform.rotation = Quaternion.RotateTowards(LaMoto.transform.rotation, Cible, vitesserotation);
            Invoke("rotateCam", Time.deltaTime);
        }

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
