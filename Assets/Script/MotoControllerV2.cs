using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoControllerV2 : MonoBehaviour
{
    //Public
    public Transform arriere;
    public Transform Avant;
    public float MaxSpeed;
    public float Forceteco;
    public bool DeuxRoue;
    //Local
    Rigidbody RBMoto;

    void Start()
    {
        RBMoto = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        float speed = MaxSpeed * Input.GetAxis("Acceleration");
        print(speed);
        if (Input.GetAxisRaw("Acceleration")!=0)
        {
            RBMoto.AddForceAtPosition(DirectionMoto(true) * speed,Avant.position,ForceMode.Force);
            
            if (DeuxRoue)
            {
                RBMoto.AddForceAtPosition(DirectionMoto(false) * speed,arriere.position, ForceMode.Force);
            }
        }
        
        //DirectionMoto();
    }


    Vector3 DirectionMoto(bool AvantDeMoto) 
    {
        Vector3 DirectionDeBase = transform.forward;
        Vector3 DirectionForceAvant = Vector3.zero;
        Vector3 DirectionFroceArrière = Vector3.zero; 
        float X = Input.GetAxis("HorizontalManette");
        float Z = Input.GetAxis("VerticalManette");
        float X2 = Input.GetAxis("HorizontalManetteDroit");
        float Z2 = Input.GetAxis("VerticalManetteDroit");
        
        //Dans le calcul voir pour prendre en compte selon rotation du parent
        if (X != 0|| Z != 0)
        {
            Vector3 PositionJoystick = new Vector3(X, 0, Z).normalized+ Avant.position;
            DirectionForceAvant =  PositionJoystick- Avant.position ;
            Debug.DrawRay(Avant.position, DirectionForceAvant, Color.red);
            //RBMoto.AddForceAtPosition(DirectionForce.normalized*Forceteco*Time.deltaTime, Avant.position,ForceMode.VelocityChange);
         
        }
        if (X2 != 0 || Z2 != 0)
        {
            Vector3 PositionJoystick = new Vector3(X2, 0, Z2).normalized + arriere.position;
            DirectionFroceArrière = PositionJoystick - arriere.position;
            Debug.DrawRay(arriere.position, DirectionFroceArrière, Color.red);
        }
        if (X2 != 0 || Z2 != 0 || X != 0 || Z != 0)
        {
            if (AvantDeMoto)
            {
                return DirectionForceAvant;
            }
            else if (!AvantDeMoto)
            {
                return DirectionFroceArrière;
            }
            else
            {
                return transform.forward;


            }
        }
       
        
        else 
        {
            return transform.forward;


        } 
        
    }

     void OnDrawGizmos()
    {
        //Direction de la force
        Gizmos.color = Color.blue;
        float X = Input.GetAxis("HorizontalManette");
        float Z = Input.GetAxis("VerticalManette");
        Vector3 PositionJoystick = new Vector3(X, 0, Z).normalized;                  //Mathf.Atan2(X, Z) * Mathf.Rad2Deg;
        //Vector3 rotation = Quaternion.ToEulerAngles(transform.rotation);

        Vector3 PositionOfSphere = Avant.position + PositionJoystick;
        Gizmos.DrawWireSphere(PositionOfSphere,0.2f);
        //
        Gizmos.color = Color.red;
        Vector3 Forcedepart = Avant.position + -PositionJoystick; 
        Gizmos.DrawWireSphere(Forcedepart, 0.2f);
        //
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(Forcedepart, Avant.position);
    }

}
