using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InfoDelaRoue
{
    public WheelCollider ComponentWheelCollider;
    public bool Motorise;
    public bool Tournable;
    [Tooltip("Puissance mis sur la roue pour tourné")]
    public float Torque = 0;
    public enum TypeDeroue
    {
        Avant,
        Arriere

    };
    public TypeDeroue roue;
}
public class GestionControle : PersonnalMethod
{
    //Public variable
    [Header("Véhicule")]
    public List<InfoDelaRoue> Roux;
    public AnimationCurve AccelerationSelonVitesseMax;
    public float MaxTorque;
    public float VitesseRotationRoux;
    public float PuissanceFreinage;
    [Tooltip("Pour des test de physique du véhicule")]
    public Vector3 offsetCenterMass;
    [Header("DetectionGeound")]
    public Transform detecteur;
    public LayerMask whatIsGround;
    public float GroundRayLength;

    //Local variable
    bool grounded;
    GestionGeneral GG;
    Rigidbody Rb;
    float ActualStreeringFront;
    float ActualStreeringBack;

    void Start()
    {
        //Rb.centerOfMass = offsetCenterMass + transform.position;
        GetGestion(out GG, this.gameObject);
        Rb = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        grounded = false;
        Debug.DrawRay(detecteur.position, -transform.up * GroundRayLength, Color.magenta);
        RaycastHit hit;
        if (Physics.Raycast(detecteur.position, -transform.up, out hit, GroundRayLength, whatIsGround))
        {
            grounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        RotateRoue();

    }
    public void RotateVehiculeByNormal() 
    {
    
    
    }

    public void RotateRoue() 
    {
        float X = 0;
        float Z = 0;
         
        foreach (InfoDelaRoue InfoDuRoux in Roux)
        {
           
            

           
             if (InfoDuRoux.roue == InfoDelaRoue.TypeDeroue.Avant)
            {
                X = Input.GetAxis("HorizontalManette");
                Z = Input.GetAxis("VerticalManette");
                calculDusteeringDeuxRoue(Mathf.Atan2(X, Z) * Mathf.Rad2Deg, InfoDuRoux);


            }

            if (InfoDuRoux.roue == InfoDelaRoue.TypeDeroue.Arriere)
            {
                X = Input.GetAxis("HorizontalManetteDroit");
                Z = Input.GetAxis("VerticalManetteDroit");
                calculDusteeringDeuxRoue(Mathf.Atan2(X, Z) * Mathf.Rad2Deg, InfoDuRoux);

            }
            
            if (InfoDuRoux.roue==InfoDelaRoue.TypeDeroue.Avant)
            {
                InfoDuRoux.ComponentWheelCollider.steerAngle = ActualStreeringFront;
            }
            else if (InfoDuRoux.roue == InfoDelaRoue.TypeDeroue.Arriere)
            {
                InfoDuRoux.ComponentWheelCollider.steerAngle = ActualStreeringBack;
            }
        }
       
        

    }

    public void Mouvement(float Direction) 
    {
    
    
    }

    void calculDusteeringDeuxRoue(float ValueViser, InfoDelaRoue roux)
    {
       
        float steering = 0;
        if (roux.roue == InfoDelaRoue.TypeDeroue.Avant)
        {
            steering = ValueViser;
            ActualStreeringFront = Mathf.Lerp(ActualStreeringFront, steering, VitesseRotationRoux * Time.deltaTime);
        }
        if (roux.roue == InfoDelaRoue.TypeDeroue.Arriere)
        {
            steering = ValueViser;
            ActualStreeringBack = Mathf.Lerp(ActualStreeringBack, steering, VitesseRotationRoux * Time.deltaTime);

        }




        //ActualStreeringFront = Mathf.Lerp(ActualStreeringFront,ValueViser,SpeedSteering*Time.deltaTime);
        //ActualStreeringBack = Mathf.Lerp(ActualStreeringBack, ValueViser, SpeedSteering * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (offsetCenterMass!=Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(offsetCenterMass + transform.position, 0.2f);
        }
    }
}

// je dois pouvoir accéléré
// freiner
// tourner
//avoir une rotation selon normal
// consommer du boost
// lorsque le boost est plein passer en surcharge
