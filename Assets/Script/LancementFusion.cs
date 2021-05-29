using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancementFusion : MonoBehaviour
{
    //scipt pour lancer la fusion depuis un trigger
    void lancementFusion(Transform objet) 
    {
        GestionBoost GB = objet.GetComponentInParent<GestionBoost>();
        if (!GB.Surchauffing)
        {
            GB.ActiveSurchauffe();
        }
    }

    private void OnTriggerEnter(Collider other) //lorsqu'un objet quitte le trigger
    {
        if (other.transform.tag == "Player")//si l'objet est player
        {
            
            lancementFusion(other.transform);//lance le void
        }
    }
    private void OnTriggerExit(Collider other) //lorsqu'un objet rentre
    {
        if (other.transform.tag == "Player")//si l'objet est player
        {
            
            lancementFusion(other.transform); // lance le void
        }
    }
}
