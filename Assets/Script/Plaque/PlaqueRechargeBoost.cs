using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaqueRechargeBoost : MonoBehaviour
{
    //Public variable
    
    //Local variable

    void Start()
    {
        
    }

    void setValueRecharge(Transform ObjetAcheck,bool leboolAVerifier) 
    {
        GestionBoost GB = ObjetAcheck.GetComponentInParent<GestionBoost>();
        
        bool toCompare = GB.Recharge;
        //print(toCompare);
        //print(leboolAVerifier);
        if (leboolAVerifier!= toCompare)
        {
            GB.Recharge = leboolAVerifier;
        }
        //GB.test(true);
    }
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag=="Player")
        {
            //print("rentre dans la plaque");
            setValueRecharge(other.transform, true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //print("sors dans la plaque");
            setValueRecharge(other.transform, false);
        }
    }
}
