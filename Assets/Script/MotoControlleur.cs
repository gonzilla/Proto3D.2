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
public class MotoControlleur : MonoBehaviour
{
    public List<AxleInfoMoto> axleInfosMoto;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public bool RoueJoystick = true;
    public bool Inclinaison = true;
    public float AngleInlinaisonMax;
    public Transform siInclinaison;
    public float SpeedSteering;
    public float Accelerationcoeff;
    public float forceResistance;

    float ActualStreeringFront;
    float ActualStreeringBack;

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
        }
        else if (!RoueJoystick)
        {
            MoveSansRoue();
        }
        
    }


    void MoveAvecRoue() 
    {
        float motor = maxMotorTorque * Input.GetAxis("Acceleration");
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
                if (axleInfoM.Torque<maxMotorTorque || axleInfoM.Torque>-maxMotorTorque)
                {
                    axleInfoM.Torque += motor*Accelerationcoeff * Time.deltaTime;
                    if (axleInfoM.Torque>maxMotorTorque)
                    {
                        axleInfoM.Torque = maxMotorTorque;
                    }
                    if (axleInfoM.Torque<-maxMotorTorque)
                    {

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

    
}