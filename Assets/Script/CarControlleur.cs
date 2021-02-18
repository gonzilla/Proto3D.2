using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    public enum TypeDeroue
    {
        Avant,
        Arriere

    };
    public TypeDeroue roue;
}

public class CarControlleur : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public bool RoueJoystick = false;
    
    // finds the corresponding visual wheel
    // correctly applies the transform
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
        //float steering = maxSteeringAngle * Input.GetAxis("HorizontalManette");
        float X = Input.GetAxis("HorizontalManette");
        float Z = Input.GetAxis("VerticalManette");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.roue == AxleInfo.TypeDeroue.Arriere)
            {
                X = Input.GetAxis("HorizontalManetteDroit");
                Z = Input.GetAxis("VerticalManetteDroit");
                //steering = maxSteeringAngle * Input.GetAxis("HorizontalManetteDroit");
            }
            else if (axleInfo.roue == AxleInfo.TypeDeroue.Avant)
            {
                X = Input.GetAxis("HorizontalManette");
                Z = Input.GetAxis("VerticalManette");
                //Quaternion.Euler(0, Mathf.Atan2(X, Z) * Mathf.Rad2Deg, 0);
                //steering = maxSteeringAngle * Input.GetAxis("HorizontalManette");
            }
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = Mathf.Atan2(X, Z) * Mathf.Rad2Deg;
                axleInfo.rightWheel.steerAngle = Mathf.Atan2(X, Z) * Mathf.Rad2Deg;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            //ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            //ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

    }

    void MoveSansRoue()
    {
        float motor = maxMotorTorque * Input.GetAxis("Acceleration");
        float steering = maxSteeringAngle * Input.GetAxis("HorizontalManette");
        

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.roue == AxleInfo.TypeDeroue.Arriere)
            {
               steering = maxSteeringAngle * Input.GetAxis("HorizontalManetteDroit");
            }
            else if (axleInfo.roue == AxleInfo.TypeDeroue.Avant)
            {
               
                
                steering = maxSteeringAngle * Input.GetAxis("HorizontalManette");
            }
            if (axleInfo.steering)
            {
                
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

    }

}
