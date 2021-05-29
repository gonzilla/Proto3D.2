using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    //script du check point
    [Tooltip("l'odre dans la course ")]
    public int ordreDePassage;
    

    


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<GestionCheckPoint>().CheckLeCheckPoint(ordreDePassage);
        }
    }
}
