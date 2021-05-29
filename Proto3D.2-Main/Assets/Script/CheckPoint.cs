using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    //Public variable
    
    public int ordreDePassage;
    //Local variable

    


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<GestionCheckPoint>().CheckLeCheckPoint(ordreDePassage);
        }
    }
}
