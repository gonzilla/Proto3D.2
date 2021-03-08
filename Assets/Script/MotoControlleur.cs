using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfoMoto
{
    public WheelCollider Wheely;
    public bool motor;
    public bool steering;
    public float Torque=0;
    public enum TypeDeroue
    {
        Avant,
        Arriere

    };
    public TypeDeroue roue;
}
public class MotoControlleur : PersonnalMethod
{
    public List<AxleInfoMoto> axleInfosMoto;
    public float maxMotorTorque;
    public float TorqueAcceleration;
    public float maxSteeringAngle;
    public bool RoueJoystick = true;
    public bool Inclinaison = true;
    public float AngleInlinaisonMax;
    public Transform siInclinaison;
    public float SpeedSteering;
    public float Accelerationcoeff;
    public float forceResistance;
    public float PuissanceFreinage;
    public float FreinageCoeff;

    //public Vector3 offsetCenterMass;
    //public Rigidbody Rb;

    float ActualStreeringFront;
    float ActualStreeringBack;
    float PuissanceFreinCumule;
    GestionGeneral GG;
    Rigidbody Rb;
    /*enum StatuMotor
    {
        Accélére,
        Recule,
        Arret

    };
    StatuMotor statuDuMotor;*/


    private void Start()
    {
        // Rb.centerOfMass = offsetCenterMass + transform.position;
        GetGestion(out GG, this.gameObject);
        Rb = GetComponent<Rigidbody>();
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        if (RoueJoystick)
        {
            MoveAvecRoue();
            //MotoDeuxRoux();
        }
        else if (!RoueJoystick)
        {
            MoveSansRoue();
        }
        if (Rb.velocity.magnitude>GG.GB.maxSpeedInBoost)
        {
            Rb.velocity = Rb.velocity.normalized * GG.GB.maxSpeedInBoost;
        }
        //Rb.centerOfMass = offsetCenterMass + transform.position;
    }

    void MotoDeuxRoux() 
    {
        float PourcentageTorqueToAdd= TorqueAcceleration * Input.GetAxis("Acceleration");
        float X = Input.GetAxis("HorizontalManette");
        float Z = Input.GetAxis("VerticalManette");
        foreach (AxleInfoMoto axleInfoM in axleInfosMoto)
        {
            if (axleInfoM.roue == AxleInfoMoto.TypeDeroue.Arriere)
            {
                X = Input.GetAxis("HorizontalManetteDroit");
                Z = Input.GetAxis("VerticalManetteDroit");
                
            }
            else if (axleInfoM.roue == AxleInfoMoto.TypeDeroue.Avant)
            {
                X = Input.GetAxis("HorizontalManette");
                Z = Input.GetAxis("VerticalManette");
                

            }

            if (axleInfoM.steering) 
            {
                calculDusteeringDeuxRoue(Mathf.Atan2(X, Z) * Mathf.Rad2Deg, axleInfoM);//le calcul devrait être améliorer
                if (axleInfoM.roue== AxleInfoMoto.TypeDeroue.Avant)
                {
                    axleInfoM.Wheely.steerAngle = ActualStreeringFront;
                }
                else 
                {
                    axleInfoM.Wheely.steerAngle = ActualStreeringBack;
                }

            }
            if (axleInfoM.motor) 
            {
                
                
            }

        }
    } // a regler plus tard


   






    void MoveAvecRoue() 
    {
        float motor=0;
        if (!GG.GB.boosting)
        {
            motor = maxMotorTorque * Input.GetAxis("Acceleration");
        }
         
        float X = Input.GetAxis("HorizontalManette");
        float Z = Input.GetAxis("VerticalManette");
        bool roueavant = false;

        foreach (AxleInfoMoto axleInfoM in axleInfosMoto)
        {
            if (axleInfoM.roue == AxleInfoMoto.TypeDeroue.Arriere)
            {
                X = Input.GetAxis("HorizontalManetteDroit");
                Z = Input.GetAxis("VerticalManetteDroit");
                roueavant = false;
            }
            else if (axleInfoM.roue == AxleInfoMoto.TypeDeroue.Avant)
            {
                X = Input.GetAxis("HorizontalManette");
                Z = Input.GetAxis("VerticalManette");
                roueavant = true;
                
            }
            if (axleInfoM.steering)
            {

                calculDusteeringDeuxRoue(Mathf.Atan2(X, Z) * Mathf.Rad2Deg, roueavant);
                if (roueavant)
                {
                    axleInfoM.Wheely.steerAngle = ActualStreeringFront;
                }
                else if (!roueavant)
                {
                    axleInfoM.Wheely.steerAngle = ActualStreeringBack;
                }
                
                
            }
            if (axleInfoM.motor)
            {
                if (Input.GetAxisRaw("Acceleration")==0)
                {
                    axleInfoM.Wheely.brakeTorque = forceResistance;
                }
                else 
                {
                    axleInfoM.Wheely.brakeTorque = 0;
                }
                if (!GG.GB.boosting)
                {
                    print("je boost pas");
                    if (axleInfoM.Torque < maxMotorTorque || axleInfoM.Torque > -maxMotorTorque)
                    {
                        axleInfoM.Torque += motor * Accelerationcoeff * Time.deltaTime;
                        if (axleInfoM.Torque > maxMotorTorque)
                        {
                            axleInfoM.Torque = maxMotorTorque;
                        }
                        if (axleInfoM.Torque < -maxMotorTorque)
                        {
                            axleInfoM.Torque = -maxMotorTorque;
                        }
                    }
                }
                
                axleInfoM.Wheely.motorTorque = axleInfoM.Torque;
                
            }
            
        }
    }
    void MoveSansRoue()
    {
        float motor = maxMotorTorque * Input.GetAxis("Acceleration");
        float steering = maxSteeringAngle * Input.GetAxis("HorizontalManette");
        float inclinaison = AngleInlinaisonMax * Input.GetAxis("HorizontalManetteDroit");
        string axepoursteering = "HorizontalManette";
        foreach (AxleInfoMoto axleInfoM in axleInfosMoto)
        {
            if (axleInfoM.roue == AxleInfoMoto.TypeDeroue.Arriere)
            {
                axepoursteering = "HorizontalManetteDroit";

                //steering = maxSteeringAngle * Input.GetAxis("HorizontalManetteDroit");
            }
            else if (axleInfoM.roue == AxleInfoMoto.TypeDeroue.Avant)
            {
                axepoursteering = "HorizontalManette";
                //steering = maxSteeringAngle * Input.GetAxis("HorizontalManette");
            }
            if (axleInfoM.steering)
            {

                calculDusteering(axepoursteering);
                axleInfoM.Wheely.steerAngle = ActualStreeringFront;
                //axleInfoM.Wheely.steerAngle = Mathf.Lerp(-maxSteeringAngle, maxSteeringAngle, steering * Time.deltaTime * SpeedSteering); ;
            }
            if (axleInfoM.motor)
            {
                axleInfoM.Wheely.motorTorque = motor;
               
            }
            ApplyLocalPositionToVisuals(axleInfoM.Wheely);
           
        }
        if (Inclinaison)
        {
            siInclinaison.rotation = Quaternion.Euler(0, 0, -inclinaison);
        }
        
    }

    void calculDusteering(string axis) //, out float steeringCal
    {
    float steering = maxSteeringAngle * Input.GetAxis(axis);
        if (steering!=0)
        {
            if (ActualStreeringFront<=maxSteeringAngle && ActualStreeringFront>=-maxSteeringAngle)
            {
                ActualStreeringFront += steering * Time.deltaTime * SpeedSteering;
                if (ActualStreeringFront>maxSteeringAngle)
                {
                    ActualStreeringFront = maxSteeringAngle;
                }
                if (ActualStreeringFront<-maxSteeringAngle)
                {
                    ActualStreeringFront = -maxSteeringAngle;
                }
            }
        }
        else if (steering==0 && ActualStreeringFront !=0)
        {
            ActualStreeringFront = Mathf.Lerp(ActualStreeringFront, 0, Time.deltaTime * SpeedSteering);
        }

        //float A = Mathf.Lerp(-maxSteeringAngle, maxSteeringAngle, steering * Time.deltaTime * SpeedSteering);
    }
    void calculDusteeringDeuxRoue(float ValueViser,bool RoueAvant) 
    {
        float steering = 0;
        if (RoueAvant)
        {
            steering = ValueViser;
            ActualStreeringFront = Mathf.Lerp(ActualStreeringFront, steering, SpeedSteering * Time.deltaTime); 
        }
        if (!RoueAvant)
        {
            steering = ValueViser;
            ActualStreeringBack = Mathf.Lerp(ActualStreeringBack, steering, SpeedSteering * Time.deltaTime);

        }
        
        
        
        
        //ActualStreeringFront = Mathf.Lerp(ActualStreeringFront,ValueViser,SpeedSteering*Time.deltaTime);
        //ActualStreeringBack = Mathf.Lerp(ActualStreeringBack, ValueViser, SpeedSteering * Time.deltaTime);
    }
    void checkLeComportementMotor()
    {
        /* if (Input.GetAxisRaw("Acceleration") == 0)
         {
             axleInfoM.Wheely.brakeTorque = forceResistance;


         }
         else if (Input.GetAxisRaw("Acceleration") == 1)
         {
             if (StatuMotor.Arret == statuDuMotor)
             {
                 axleInfoM.Wheely.brakeTorque = 0;

             }

         }
         else if (Input.GetAxisRaw("Acceleration") == -1)
         {
             if (StatuMotor.Arret == statuDuMotor)
             {
                 axleInfoM.Wheely.brakeTorque = 0;

             }
         }*/
    }
    void calculDusteeringDeuxRoue(float ValueViser, AxleInfoMoto roux)
    {
        float steering = 0;
        if (roux.roue == AxleInfoMoto.TypeDeroue.Avant)
        {
            steering = ValueViser;
            ActualStreeringFront = Mathf.Lerp(ActualStreeringFront, steering, SpeedSteering * Time.deltaTime);
        }
        if (roux.roue == AxleInfoMoto.TypeDeroue.Arriere)
        {
            steering = ValueViser;
            ActualStreeringBack = Mathf.Lerp(ActualStreeringBack, steering, SpeedSteering * Time.deltaTime);

        }




        //ActualStreeringFront = Mathf.Lerp(ActualStreeringFront,ValueViser,SpeedSteering*Time.deltaTime);
        //ActualStreeringBack = Mathf.Lerp(ActualStreeringBack, ValueViser, SpeedSteering * Time.deltaTime);
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(offsetCenterMass + transform.position, 0.2f);
    }*/
}
/*Vector3 directiontest = Quaternion.Euler(0, ActualStreeringFront, 0) * Vector3.forward;
                Debug.DrawRay(axleInfoM.Wheely.transform.position, directiontest, Color.blue);
                // check la valeur axes
                float valueAxes = Input.GetAxisRaw("Acceleration");
                float sensTorque = 0;
                if (axleInfoM.Torque!=0)
                {
                    sensTorque = axleInfoM.Torque / Mathf.Abs(axleInfoM.Torque);
                }
                else if (axleInfoM.Torque == 0)
                {
                    sensTorque = 0;
                }
                
                if (valueAxes!=0)
                {
                    if (sensTorque!=valueAxes)
                    {
                        PuissanceFreinCumule += PuissanceFreinage * Time.deltaTime * FreinageCoeff;
                        axleInfoM.Wheely.brakeTorque = forceResistance +PuissanceFreinCumule ;
                        axleInfoM.Torque = axleInfoM.Wheely.motorTorque;
                    }
                    else if (sensTorque == valueAxes )
                    {
                        axleInfoM.Wheely.brakeTorque = 0;
                        axleInfoM.Torque += PourcentageTorqueToAdd * Time.deltaTime;
                        axleInfoM.Wheely.motorTorque = axleInfoM.Torque;
                    }
                   
                }
                else if (valueAxes == 0)
                {

                }*/
