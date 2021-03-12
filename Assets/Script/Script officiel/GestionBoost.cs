using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GestionBoost : PersonnalMethod
{
    //Public variable
    public float LimiteBoost,VitesseBoost,BoostParFrame, ForceBoost, NewTorqueMax, vitesseDiminutionSurchauffe,TempsAprèsSurchauffe;
    [HideInInspector] public bool Recharge;
    [HideInInspector] public bool boosting;
    [HideInInspector] public bool Surchauffing;
    public float maxSpeedInBoost;
    //Local variable
    float actualBoostReserve;
    float Pourcentage;
    Rigidbody Rb;
    GestionGeneral GG;
    float OldTorqueMax;

    void Start()
    {
        GetGestion(out GG, this.gameObject);
        Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Pourcentage = actualBoostReserve / LimiteBoost;
        if (Recharge)
        {
            GetBoost();
        }
        MakeSureBoostIsGood();
        if (Pourcentage==1)
        {
            Surchauffing = true;
        }
         if (Surchauffing)
        {
            surchauffe();
        }

    }
   

    public void GetBoost() 
    {
        //print("GetBoost");
        if (actualBoostReserve<LimiteBoost && !Surchauffing)
        {
            actualBoostReserve += BoostParFrame * VitesseBoost * Time.deltaTime;
            GG.GUI.setSliderBoost(actualBoostReserve);
        }
        
    
    }
    public void GetBoost(float value) 
    {
        if (!Surchauffing)
        {
            actualBoostReserve += value;
            GG.GUI.setSliderBoost(actualBoostReserve);
        }
       

    }

    public void UseBoost() 
    {
        if (!Surchauffing)
        {
            if (actualBoostReserve / LimiteBoost >= 0.25f && !boosting)
            {

                print("Lance Le boost");
                boosting = true;
                actualBoostReserve -= 0.25f;//enlever le taux de boost
                GestionControle motoC = GetComponent<GestionControle>();
                Rb.velocity = Rb.velocity.normalized * ForceBoost;

                //Invoke("checkvelocity", Time.deltaTime);

                // add boost
                Invoke("finDeBoost", 2f);
            }
            GG.GUI.setSliderBoost(actualBoostReserve);
        }
        
    }
    void finDeBoost() 
    {
        boosting = false;
    
    }

    public void surchauffe() 
    {
        GG.GC.MaxTorque = NewTorqueMax;
        actualBoostReserve -= vitesseDiminutionSurchauffe * Time.deltaTime;
        MakeSureBoostIsGood();
        if (actualBoostReserve / LimiteBoost == 0)
        {
            Invoke("finDeSurchauffe", TempsAprèsSurchauffe);
        }
        GG.GUI.setSliderBoost(actualBoostReserve);
    }
    void finDeSurchauffe() 
    {
        if (Surchauffing)
        {
            GG.GC.MaxTorque = OldTorqueMax;
            Surchauffing = false;
        }
      
    }
   
    void checkvelocity() 
    {
        if (Rb.velocity.magnitude > maxSpeedInBoost)
        {
            Rb.velocity = Rb.velocity.normalized * maxSpeedInBoost;
        }
    }

    void MakeSureBoostIsGood() 
    {
        if (actualBoostReserve<0||actualBoostReserve>LimiteBoost)
        {

            if (actualBoostReserve > LimiteBoost)
            {
                actualBoostReserve = LimiteBoost;
            }
            else if(actualBoostReserve<0) 
            {
                actualBoostReserve = 0;
            }


        }
    
    }
}
 /*foreach (InfoDelaRoue wheel in motoC.axleInfosMoto)
            {
                //wheel.Torque = ForceBoost;
                //wheel.Wheely.motorTorque = ForceBoost;

               

                //GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized* ForceBoost;//idée interressante mais ne fonctionne pas correctement
                //wheel.Torque = ForceBoost;
                //wheel.Wheely.motorTorque = ForceBoost;
                /*Vector3 positionRoue = Vector3.zero;
                Quaternion temp = Quaternion.identity;
                wheel.Wheely.GetWorldPose(out positionRoue, out temp);
                Vector3 PositionForce = new Vector3(positionRoue.x, transform.position.y, positionRoue.z);
                Vector3 directiontest = Quaternion.Euler(0, wheel.Wheely.steerAngle, 0) * Vector3.forward;
                Rb.AddForceAtPosition(directiontest.normalized * ForceBoost, PositionForce, ForceMode.Impulse);
                Debug.DrawRay(wheel.Wheely.transform.position, directiontest, Color.blue);*/

            //} 