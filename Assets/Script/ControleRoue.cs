using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleRoue : MonoBehaviour
{
    //Public variable
    public enum TypeDeroue
    {
        Avant,
        Arriere

    };
    public TypeDeroue roue;
    public float speed;
    //Local variable
    Rigidbody RB;

    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        if (roue==TypeDeroue.Avant)
        {
            float Z = Input.GetAxis("HorizontalManette");
            float X = Input.GetAxis("VerticalManette");
            //RB.AddTorque(new Vector3(X,0,Z).normalized*speed, ForceMode.Force);
            RB.angularVelocity = new Vector3(X, 0, Z).normalized * speed;
            //
            //Vector3 rotation = new Vector3(X, 0, Z).normalized*speed;
            //RB.MoveRotation(Quaternion.Euler(rotation));
        }
        else if(roue == TypeDeroue.Arriere)
        {
            float Z = Input.GetAxis("HorizontalManetteDroit");
            float X = Input.GetAxis("VerticalManetteDroit");
            RB.angularVelocity = new Vector3(X, 0, Z).normalized * speed;

        }
    }
}
