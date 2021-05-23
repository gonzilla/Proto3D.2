using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionDesInputs : PersonnalMethod
{
    //Public variable
    //public int PlayerIndex;//later
    [Header("Touche Dans Axes")]
    [Tooltip("liste des axes utilisé")]
    public string[] Axes; //Tableau des Inputs qui ont besoins d'une valeur de pression
    [HideInInspector] public float[] TimeMaintenueAxes;//le temps que le joueur maintien cette input
    [HideInInspector] public float[] TimePressionAxes;// le temps pressé
    [HideInInspector] public bool [] InUse;// Pour Savoir si le bouton est utilisé ou nan
    [Tooltip("temps a partir duquel on considére un maintien")]
    public float timeMinMaintien;// temps à partir duquel on décide qu'une touche est maintenue volontairement 
    [Tooltip("temps a partir duquel un maintient n'est plus nécessaire")]
    public float TempsMaxMaintien;

    [HideInInspector] public bool CanPlay;
    //Local variable
    GestionGeneral GG;//stock les script




    void Start()
    {
        GetGestion(out GG, this.gameObject);// récupére les autres script
        setTableauLength();
        GG.Start = true;
    }

    
    void FixedUpdate()
    {
        
            GG.GMC.SetByNormal(); // check le sol 
            GG.CSF.CameraComportement(); // fais les comportements de la camera
        
        
        
            GG.GMC.RotateMotoInWorld();
            if (!InUse[7])
            {
                GG.GMC.tourne(Input.GetAxis(Axes[3])); //Lance void pour Tourner
            if (GG.CanPlay)
            {
                GG.CSF.InfoRotationDeLaCam(Input.GetAxis(Axes[3])); //envois des info pour la cam
            }    
          


            }
            if (!InUse[7] && Input.GetAxis(Axes[3]) == 0 && GG.CanPlay)
            {
                GG.GMC.straff(Input.GetAxis(Axes[5]));//lance le straff
                if (Input.GetAxis(Axes[5]) == 0)
                {
                    GG.FeedBackVisu.GestionStraff(false, false);
                }
                GG.CSF.InfoRotationDeLaCam(0); //envois des info pour la cam
            }
            if (GG.CanPlay)
            {
            GG.GMC.avance(Input.GetAxis(Axes[1]));
            }
            else 
            {
            GG.GMC.avance(0);
            }
            if (GG.CanPlay)
            {
            if (Input.GetAxisRaw(Axes[0]) != 0 && !InUse[0])//si cette touche est utilisé
            {
                GG.GB.UseBoost();//Lance le boost
                SetBoolArray(0, true);
            }
            if (Input.GetAxisRaw(Axes[0]) == 0 && InUse[0])
            {
                SetBoolArray(0, false);
            }
            if (Input.GetAxisRaw(Axes[7]) != 0)
            {

                bool state = true;
                if (!InUse[7])
                {
                    SetBoolArray(7, state);
                }
                bool LoseSpeed = false;

                GG.GMC.TourneDerapage(Input.GetAxisRaw(Axes[7]), Input.GetAxis(Axes[3]), out LoseSpeed);
                GG.GMC.derapage(state, Input.GetAxis(Axes[3]), LoseSpeed);
                GG.CSF.InfoRotationDeLaCam(Input.GetAxis(Axes[3])); //envois des info pour la cam



            }
            if (Input.GetAxisRaw(Axes[7]) == 0 && InUse[7])
            {
                bool state = false;
                GG.CSF.InfoRotationDeLaCam(Input.GetAxis(Axes[3])); //envois des info pour la cam
                GG.GMC.derapage(state, 0, false);
                SetBoolArray(7, state);
                GG.FeedBackVisu.GestionStraff(state, false);
                GG.FeedBackVisu.GestionSmoke(state);
                GG.FeedBackVisu.GestionWheelTrail(state);
                GG.FeedBackVisu.GestionParticleRoue(state);
            }
        }

        if (Input.GetAxis(Axes[9]) != 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetAxis(Axes[8]) != 0)
        {
            GG.GMC.ResetLastPosition();
        }


    }

    void setTableauLength() 
    {
        TimeMaintenueAxes = new float[Axes.Length];
        TimePressionAxes = new float[Axes.Length];
        InUse = new bool[Axes.Length];
    }

    void SetBoolArray(int IndexAxes,bool value) 
    {

        InUse[IndexAxes] = value;

    }

}
/* ancien code
 *  //GG.GC.RotateRoue();
        //GG.GC.RotateVehiculeByNormal();
        //GG.GC.Mouvement(Input.GetAxis(Axes[1]));

        if (Input.GetAxisRaw(Axes[0])!=0)
        {
            GG.GB.UseBoost();
        }
        // roue gauche
       /* if (Input.GetAxis(Axes[3]) != 0 || Input.GetAxis(Axes[4]) != 0)
        {
            GG.GC.RotateRoue(Input.GetAxis(Axes[3]), Input.GetAxis(Axes[4]));

        }
        // roue droit
        
        GG.GC.RotateRoue(Input.GetAxis(Axes[5]), Input.GetAxis(Axes[6]));
       

//accélération/Deceleration
if (Input.GetAxis(Axes[1]) != 0)
{
    //print(Input.GetAxis(Axes[1]));
    //float value = Input.GetAxis(Axes[1]) + Input.GetAxis(Axes[2]);


}*/
