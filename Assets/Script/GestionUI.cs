using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionUI : MonoBehaviour
{
    //Public variable
    public Text Speed;
    public Rigidbody Moto;
    public int arrondisDecimal = 2;
    //Local variable

    void Start()
    {
        
    }

    
    void Update()
    {
        float valueVelocity = Moto.velocity.magnitude *3.6f;
        float arrondis = (float)System.Math.Round(valueVelocity, arrondisDecimal);
        Speed.text = arrondis.ToString();
    }
}
