using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyYourself : MonoBehaviour
{
    //Public variable
    public float tempsAvantDestruction;
    public bool Immortel;
    //Local variable

    public void DestroyToi() 
    {
        if (!Immortel)
        {
            Invoke("DestroyToiMtn", tempsAvantDestruction);
        }
    }
    void DestroyToiMtn() 
    {
        Destroy(this.gameObject);
    }
}
