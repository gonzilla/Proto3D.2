using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GestionBoost : PersonnalMethod
{
    //Public variable
    
    [Header("Boost")]
    [Tooltip("A laisser sur 1")]//peut être passer la variable en private
    public float ReserveDenergieBoostMax; // 0=0 et 1= 100% à laisser sur 1/!\/!\/!\/!\
    [Tooltip("vitesse de recharge de la barre")]
    public float VitesseRechargeEnergie; // vitesse de recharge d'énergie sur la plaque
    [Tooltip("La Vitesse max /!/ c'est *100")]
    public float NouvelleVitesseMax; // La nouvelle vitesse max lors du boost
    [Tooltip("Durée du boost")]
    public float TempsDeBoosting; // Le temps que le véhicule vas booster
    [Tooltip("Valeur entre 0 et 1, 1 = 100% et 0.5 = 50%")]
    public float[] PourcentageVitesseParBoost;//les pourcentages selon boost cumulé
    [Tooltip("Le temps dont dispose le joeur pour rappuyer")]
    public float TimeForInput;//le temps pour rappuyer comme un singe
    //Surchauffe
    [Header("Surchauffe")]
    [Tooltip("Le temps avant de pouvoir rechager")]
    public float TempsAprèsSurchauffe;// Temps que le joueur ne peut pas recharger après surchauffe
    [Tooltip("La vitesse en surcharge")]
    public float VitesseMaxSurchauffe;//La vitessse à laquel le joueur vas en surchauffe
    [Tooltip("obsoléte")]
    public float AccelerationSurchauffe; // taux d'accéleration en surchauffe // obsoléte
    public float vitesseDiminutionSurchauffe;//taux de décéllération en surchauffe // obsoléte
    [HideInInspector] public bool Recharge; //bool pour savoir si le joueur recharge
    [HideInInspector] public bool boosting; // bool pour savoir si le joueur boost // peut être obsoléte 
    [HideInInspector] public bool Surchauffing;// bool pour savoir si c'est en surchauffe
    

    [Header("Collision")]
    [Tooltip("La perte de boost lorsqu'entre en collision pdt surchauffe")]
    public float PerteParCollisionInSurchauffe;// perte d'énergie par collision en surchauffe

    //Local variable
    bool canRecharge = true; // bool pour savoir si le joeur peut rechager
    float actualBoostReserve; // le taux de boost 
    float Pourcentage; // le taux sur reserve max
    Rigidbody Rb; //le rigidbody du joueur
    GestionGeneral GG;// ke script qui contient tous les autres script
    float OldvitesseMax;// valeur pour revenir à la vielle vitesse max
    float NouveauxTempsForInput;// comme dans le nom
    int NombreDeFoisApppuyerBoost=0; //?


    void Start()
    {
        GetGestion(out GG, this.gameObject);//récupére les autres script
        Rb = GetComponent<Rigidbody>();// récupére le rigidbody
        OldvitesseMax = GG.GMC.vitesseMax; // set la veille vitesse max
    }

    void Update()
    {
        Pourcentage = actualBoostReserve / ReserveDenergieBoostMax;// calcule le pourcentage
        if (Recharge)//si il rechage
        {
            GetBoost();//lance la fonction getboost
            GG.FeedBackVisu.GestionParticleRecharge(true);
        }
        else 
        {
            GG.FeedBackVisu.GestionParticleRecharge(false);
        }
        
        MakeSureBoostIsGood();// s'assure que la reserve de boost est bonne
        if (Pourcentage==1)
        {
            Surchauffing = true;//met la moto en surchauffe
            GG.FeedBackVisu.GestionParticleFusion(true);
        }
         if (Surchauffing)
        {
            surchauffe();//Lance levennement surchauffe
            
        }

    }
   

    public void GetBoost() //Fonction pour avoir du boost
    {
        //print("GetBoost");
        if (actualBoostReserve< ReserveDenergieBoostMax && canRecharge)// si je peux recharger puis jai pas suffisament de ressource
        {
            actualBoostReserve += VitesseRechargeEnergie * Time.deltaTime; //augmente les ressources
            GG.GUI.setSliderBoost(actualBoostReserve);//met à jour le slider
            SetNewvitesseMax();//recalcul la vitesse max
            GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.RecolteDeBoost);
        }
        
    
    }

    public void GetBoost(float value) //Fonction pour avoir du boost avec une certaine value
    {
        if (actualBoostReserve < ReserveDenergieBoostMax && canRecharge) // si je peux rechager
        {
            actualBoostReserve += value;//donne la value
            GG.GUI.setSliderBoost(actualBoostReserve);//met à jour le slider
            SetNewvitesseMax();//recalcul la vitesse max
        }
       

    }

    public void LostBoost(float value) //fais  perdre du boost 
    {
        actualBoostReserve -= value;//donne la value
        GG.GUI.setSliderBoost(actualBoostReserve);//met à jour le slider
        MakeSureBoostIsGood();//s'assure de clamp si necessaire 
        SetNewvitesseMax();//recalcul la vitesse max
    }

    public void UseBoost() // utilise le boost
    {
        if (!Surchauffing)// si je ne suis pas en surchauffe
        {
            if (actualBoostReserve / ReserveDenergieBoostMax >= 0.25f && NombreDeFoisApppuyerBoost<3 )//&& !boosting
            {
                CancelInvoke();//enléve l'invoke possible 
                boosting = true;// boosting
                actualBoostReserve -= 0.25f;//enlever le taux de boost

                if (Time.time > NouveauxTempsForInput && NouveauxTempsForInput != 0)// si le temps pour appuyer à été dépassé et le ce n'est pas la première fois que j'appuis
                {
                    NombreDeFoisApppuyerBoost = 0;//je met le nombre d'appuie a 0
                    NouveauxTempsForInput = Time.time + TimeForInput;// le temps + le temps 
                }
                if (NombreDeFoisApppuyerBoost==0)
                {
                    GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.BoostLV1);
                    GG.CSF.GestionCameraShake(ScreenShakeInfo.Action.BoostLV1);
                    GG.FeedBackVisu.gestionBoost(0, true);
                }
                if (NombreDeFoisApppuyerBoost == 1)
                {
                    GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.BoostLV2);
                    GG.CSF.GestionCameraShake(ScreenShakeInfo.Action.BoostLV2);
                    GG.FeedBackVisu.gestionBoost(1, true);
                }
                if (NombreDeFoisApppuyerBoost == 2)
                {
                    GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.BoostLV3);
                    GG.CSF.GestionCameraShake(ScreenShakeInfo.Action.BoostLV3);
                    GG.FeedBackVisu.gestionBoost(2, true);
                }
                
                float newVitesse = NouvelleVitesseMax + ( PourcentageVitesseParBoost[NombreDeFoisApppuyerBoost] * NouvelleVitesseMax); // calcule la nouvelle vitesse
                GG.GMC.vitesseMax = newVitesse;//donne la vitesse max
                GG.GMC.VitesseMoto = newVitesse;//met la moto à la vitesse
                //Debug.Break();
                if (Time.time < NouveauxTempsForInput || NouveauxTempsForInput == 0) // si le temps pour appuyer n'a pas été dépassé ou c'est pas la première fois que j'appuis
                {
                    NouveauxTempsForInput = Time.time + TimeForInput;// le temps + le temps 
                    NombreDeFoisApppuyerBoost++;//augmente le nombre d'appuie
                }
                

                Invoke("finDeBoost", TempsDeBoosting);//lance la fin de boost 
            }
            GG.GUI.setSliderBoost(actualBoostReserve);//met a jour la reserve
            //SetNewvitesseMax();//recalcul la vitesse max
        }
        
    }
    void finDeBoost() // Lance la fin du boost
    {
        boosting = false;//ne boost plus
        SetNewvitesseMax();//recalcul la vitesse max
        //GG.GMC.vitesseMax = OldvitesseMax;// reset la vitesse max
        NombreDeFoisApppuyerBoost = 0;//reset le nombre d'appuie
        GG.FeedBackVisu.gestionBoost(0, false);
    }

    public void surchauffe()// fonction de surchauffe
    {
        actualBoostReserve -= vitesseDiminutionSurchauffe * Time.deltaTime;// diminue la reserve de boost
        GG.GUI.setSliderBoost(actualBoostReserve); // met à jour le slider
        GG.GMC.vitesseMax = VitesseMaxSurchauffe; // set la nouvelle vitesse max
        GG.GMC.VitesseMoto = VitesseMaxSurchauffe; // set la vitesse
        GG.CSF.GestionCameraShake(ScreenShakeInfo.Action.Fusion);
        GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.Surchauffe);
        if (actualBoostReserve <= 0)//si la reserve est tombé à 0
        {
            GG.FeedBackVisu.GestionParticleFusion(false);
            actualBoostReserve = 0;//la remet a 0
            Surchauffing = false;// n'est pas en surchauffe
            GG.GMC.vitesseMax = OldvitesseMax;//remet la vitesse max
            
            Invoke("canRechargeAgain", TempsAprèsSurchauffe);//
        }
    }

    public void ActiveSurchauffe() 
    {
        actualBoostReserve = ReserveDenergieBoostMax;


    }


    void canRechargeAgain() //dis au systéme que je peut lancer la recharge // voir surchauffe
    {
        canRecharge = true;//peut recharger
        CancelInvoke();//enléve l'afflus de Invoke
    }

   
    
    void SetNewvitesseMax() //
    {
        GG.GMC.vitesseMax = OldvitesseMax + OldvitesseMax * Pourcentage /2 ;//calcul de la nouvelle vitesse max
    }
   
   

    void MakeSureBoostIsGood() 
    {
        if (actualBoostReserve<0||actualBoostReserve> ReserveDenergieBoostMax)//si ma réserve de boost 0> ou ma reserve est trop grande
        {

            if (actualBoostReserve > ReserveDenergieBoostMax) //si ma reserve est trop grande
            {
                actualBoostReserve = ReserveDenergieBoostMax; // met au max 
            }
            else if(actualBoostReserve<0) //si ma réserve de boost 0>
            {
                actualBoostReserve = 0; //remet a zero
            }


        }

    }//check le boost
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
//public float LimiteBoost,VitesseBoost,BoostParFrame, ForceBoost, NewTorqueMax, vitesseDiminutionSurchauffe,TempsAprèsSurchauffe;
    //Boost
 */
/*void checkvelocity() 
   {
       if (Rb.velocity.magnitude > maxSpeedInBoost)
       {
           Rb.velocity = Rb.velocity.normalized * maxSpeedInBoost;
       }
   }*/
//GG.GMC.VitesseMoto = Mathf.Lerp(GG.GMC.VitesseMoto, VitesseMaxSurchauffe, AccelerationSurchauffe*Time.deltaTime);
/*if (GG.GMC.VitesseMoto > VitesseMaxSurchauffe)// obsoléte
{
    GG.GMC.VitesseMoto = VitesseMaxSurchauffe;
}*/