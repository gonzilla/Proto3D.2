using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GestionBoost : PersonnalMethod
{
    //Public variable
    //public float LimiteBoost,VitesseBoost,BoostParFrame, ForceBoost, NewTorqueMax, vitesseDiminutionSurchauffe,TempsAprèsSurchauffe;
    //Boost
    [Header("Boost")]
    public float ReserveDenergieBoostMax; public float VitesseRechargeEnergie, NouvelleVitesseMax, TempsDeBoosting;
    //Surchauffe
    [Header("Surchauffe")]
    public float TempsAprèsSurchauffe; public float VitesseMaxSurchauffe, AccelerationSurchauffe, vitesseDiminutionSurchauffe;
    [HideInInspector] public bool Recharge;
    [HideInInspector] public bool boosting;
    [HideInInspector] public bool Surchauffing;

    //Local variable
    bool canRecharge = true;
    float actualBoostReserve;
    float Pourcentage;
    Rigidbody Rb;
    GestionGeneral GG;
    float OldvitesseMax;

    void Start()
    {
        GetGestion(out GG, this.gameObject);
        Rb = GetComponent<Rigidbody>();
        OldvitesseMax = GG.GMC.vitesseMax;
    }

    void Update()
    {
        Pourcentage = actualBoostReserve / ReserveDenergieBoostMax;
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
        if (actualBoostReserve< ReserveDenergieBoostMax && canRecharge)
        {
            actualBoostReserve += VitesseRechargeEnergie * Time.deltaTime;
            GG.GUI.setSliderBoost(actualBoostReserve);
        }
        
    
    }
    public void GetBoost(float value) 
    {
        if (canRecharge)
        {
            actualBoostReserve += value;
            GG.GUI.setSliderBoost(actualBoostReserve);
        }
       

    }

    public void UseBoost() 
    {
        if (!Surchauffing)
        {
            if (actualBoostReserve / ReserveDenergieBoostMax >= 0.25f && !boosting)
            {

                print("Lance Le boost");
                boosting = true;
                actualBoostReserve -= 0.25f;//enlever le taux de boost
                GG.GMC.vitesseMax = NouvelleVitesseMax;
                GG.GMC.VitesseMoto = NouvelleVitesseMax;
                Invoke("finDeBoost", TempsDeBoosting);
            }
            GG.GUI.setSliderBoost(actualBoostReserve);
        }
        
    }
    void finDeBoost() 
    {
        boosting = false;
        GG.GMC.vitesseMax = OldvitesseMax;

    }
    void canRechargeAgain() 
    {
        canRecharge = true;
        CancelInvoke();
    }

    public void surchauffe() 
    {
        actualBoostReserve -= vitesseDiminutionSurchauffe * Time.deltaTime;
        GG.GUI.setSliderBoost(actualBoostReserve);
        GG.GMC.vitesseMax = VitesseMaxSurchauffe;
        GG.GMC.VitesseMoto = Mathf.Lerp(GG.GMC.VitesseMoto, VitesseMaxSurchauffe, AccelerationSurchauffe*Time.deltaTime);
        if (GG.GMC.VitesseMoto> VitesseMaxSurchauffe)
        {
            GG.GMC.VitesseMoto = VitesseMaxSurchauffe;
        }
        if (actualBoostReserve<=0)
        {
            actualBoostReserve = 0;
            Surchauffing = false;
            GG.GMC.vitesseMax = OldvitesseMax;
            Invoke("canRechargeAgain", TempsAprèsSurchauffe);
        }
    }
    
    void finDeSurchauffe() 
    {
       
       
      
    }
   
    /*void checkvelocity() 
    {
        if (Rb.velocity.magnitude > maxSpeedInBoost)
        {
            Rb.velocity = Rb.velocity.normalized * maxSpeedInBoost;
        }
    }*/

    void MakeSureBoostIsGood() 
    {
        if (actualBoostReserve<0||actualBoostReserve> ReserveDenergieBoostMax)
        {

            if (actualBoostReserve > ReserveDenergieBoostMax)
            {
                actualBoostReserve = ReserveDenergieBoostMax;
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

/*GestionControle motoC = GetComponent<GestionControle>();
                Rb.velocity = Rb.velocity.normalized * ForceBoost;

 
 
 
 
 //
  GG.GC.MaxTorque = NewTorqueMax;
        actualBoostReserve -= vitesseDiminutionSurchauffe * Time.deltaTime;
        MakeSureBoostIsGood();
        MotoControlleur MC=GetComponent<MotoControlleur>();
        if (MC.enabled)
        {
            MC.maxspeed = 90;
            foreach (AxleInfoMoto roux in MC.axleInfosMoto)
            {
                roux.Torque = NewTorqueMax;
            }
        }
        if (actualBoostReserve / LimiteBoost == 0)
        {
            Invoke("finDeSurchauffe", TempsAprèsSurchauffe);
        }
        GG.GUI.setSliderBoost(actualBoostReserve);
 
 


 if (Surchauffing)
        {
            GG.GC.MaxTorque = OldTorqueMax;
            MotoControlleur MC = GetComponent<MotoControlleur>();
            if (MC.enabled)
            {
                MC.maxspeed = 80;
                foreach (AxleInfoMoto roux in MC.axleInfosMoto)
                {
                    roux.Torque = OldTorqueMax;
                }
            }
            Surchauffing = false;
            CancelInvoke();
        }

 */