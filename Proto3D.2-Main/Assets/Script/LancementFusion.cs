using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancementFusion : MonoBehaviour
{
    //Public variable
    
    //Local variable

    void lancementFusion(Transform objet) 
    {
        GestionBoost GB = objet.GetComponentInParent<GestionBoost>();
        if (!GB.Surchauffing)
        {
            GB.ActiveSurchauffe();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //print("rentre dans la plaque");
            lancementFusion(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //print("sors dans la plaque");
            lancementFusion(other.transform);
        }
    }
}
