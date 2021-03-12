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
    public float VitesseAccelerationRoux;
    [Tooltip("0, pas de frein. 1, Frein au max")]
    public float CoeffFreinage;
    public float PuissanceFreinage;
    public float maxVelocity;
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

    
    /*void FixedUpdate()
    {
        //RotateVehiculeByNormal()
        //RotateRoue();

    }*/
    public void RotateVehiculeByNormal() 
    {
        grounded = false;
        Debug.DrawRay(detecteur.position, -transform.up * GroundRayLength, Color.magenta);
        RaycastHit hit;
        if (Physics.Raycast(detecteur.position, -transform.up, out hit, GroundRayLength, whatIsGround))
        {
            grounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

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
        //voir si la torque est en adéquation avec direction
        float DirectionNormalise = 0;
        float TorqueNormalise = 0;
        
        foreach (InfoDelaRoue InfoDuRoux in Roux)
        {
            if (InfoDuRoux.Torque!=0)//check si le motor a une force si oui positif ou negatif?
            {
                 TorqueNormalise = InfoDuRoux.Torque / Mathf.Abs(InfoDuRoux.Torque);
                 
            }
            if (Direction!=0)//check le sens dans la quel le joueur souhaite aller
            {
                DirectionNormalise = Direction / Mathf.Abs(Direction);
            }
            
            if (TorqueNormalise==0 || TorqueNormalise==DirectionNormalise)//si le moteur est à l'arret  ou le joueur souhaite aller dans la même direction que le moteur
            {
                InfoDuRoux.ComponentWheelCollider.brakeTorque = 0;
                float PourcentageTorque = InfoDuRoux.Torque / MaxTorque;
                InfoDuRoux.Torque += Direction* VitesseAccelerationRoux * AccelerationSelonVitesseMax.Evaluate(PourcentageTorque) * Time.deltaTime;
                checkTorque(InfoDuRoux);
                InfoDuRoux.ComponentWheelCollider.motorTorque = InfoDuRoux.Torque;
                //accélere dans direction
            }
            else if ( TorqueNormalise != DirectionNormalise)
            {
                InfoDuRoux.ComponentWheelCollider.brakeTorque = PuissanceFreinage;
                float calculDuNouveauTorque = PuissanceFreinage* CoeffFreinage * Time.deltaTime;
                checkTorque(InfoDuRoux);
                float TorqueEnfrainage = Mathf.Lerp(InfoDuRoux.Torque, 0, calculDuNouveauTorque);
                InfoDuRoux.Torque = TorqueEnfrainage;
                //InfoDuRoux.Torque = Mathf.Clamp(InfoDuRoux.Torque, 0, MaxTorque);
            }
        }

        //float TorqueNormalise = 
       

    }

    void checkTorque(InfoDelaRoue MonRoux) 
    {
        if (MonRoux.Torque < MaxTorque || MonRoux.Torque > -MaxTorque)
        {
          
            if (MonRoux.Torque > MaxTorque)
            {
                MonRoux.Torque = MaxTorque;
            }
            if (MonRoux.Torque < -MaxTorque)
            {
                MonRoux.Torque = -MaxTorque;
            }
        }
        if (MonRoux.Torque < 0.01f && MonRoux.Torque > -0.01f)
        {
            MonRoux.Torque = 0;
        }

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
