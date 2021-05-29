using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyYourself : MonoBehaviour
{
    //Script pour ce détruire sois même
    //Public variable
    [Tooltip(" temps avant que l'objet se détruise ")]
    public float tempsAvantDestruction;
    [Tooltip("si l'objet doit être immortel")]
    public bool Immortel;
    //Local variable

    public void DestroyToi() 
    {
        if (!Immortel)
        {
            Invoke("DestroyToiMtn", tempsAvantDestruction);
        }
    }//lance le destroy dans un certain temps

    void DestroyToiMtn() 
    {
        Destroy(this.gameObject);//se detruit
    }
}
