using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionDesInputs : PersonnalMethod
{
    
    //Public variable
    [Header("Touche Dans Axes")]
    [Tooltip("liste des axes utilisé")]
    public string[] Axes; //Tableau des Inputs qui ont besoins d'une valeur de pression
    [Tooltip("temps a partir duquel on considére un maintien")]
    public float timeMinMaintien;// temps à partir duquel on décide qu'une touche est maintenue volontairement 
    [Tooltip("temps a partir duquel un maintient n'est plus nécessaire")]
    public float TempsMaxMaintien;
    public string SceneToLoad;
    [HideInInspector] public float[] TimeMaintenueAxes;//le temps que le joueur maintien cette input
    [HideInInspector] public float[] TimePressionAxes;// le temps pressé
    [HideInInspector] public bool[] InUse;// Pour Savoir si le bouton est utilisé ou nan
    [HideInInspector] public bool CanPlay;
    //Local variable
    GestionGeneral GG;//stock les script




    void Start()
    {
        GetGestion(out GG, this.gameObject);// récupére les autres script
        setTableauLength();//set la length des tableaux
        GG.Start = true;
    }

    
    void FixedUpdate()
    {
        
            GG.GMC.SetByNormal(); // check le sol 
            GG.CSF.CameraComportement(); // fais les comportements de la camera
            GG.GMC.RotateMotoInWorld();//rotate le modéle de la moto
            if (!InUse[7])//s'il ne drift pas
            {
                GG.GMC.tourne(Input.GetAxis(Axes[3])); //Lance void pour Tourner
                if (GG.CanPlay)//si le joueur peut jouer
                {
                    GG.CSF.InfoRotationDeLaCam(Input.GetAxis(Axes[3])); //envois des info pour la cam
                }    
            }

            if (!InUse[7] && Input.GetAxis(Axes[3]) == 0 && GG.CanPlay)//s'il ne drift pas et ne tourne pas et le joueur peut jouer
            {
                GG.GMC.straff(Input.GetAxis(Axes[5]));//lance le straff
                if (Input.GetAxis(Axes[5]) == 0)//s'il ne straff pas
                {
                    GG.FeedBackVisu.GestionStraff(false, false);//lance le feedback de straff
                }
                GG.CSF.InfoRotationDeLaCam(0); //envois des info pour la cam
            }

            if (GG.CanPlay)//si le joueur peut jouer
            {
                GG.GMC.avance(Input.GetAxis(Axes[1]));//lance la marche avant
            }
            else 
            {
                GG.GMC.avance(0);//lui dis de pas avancer
            }
            if (GG.CanPlay)//si le joueur peut jouer
            {
                if (Input.GetAxisRaw(Axes[0]) != 0 && !InUse[0])//si cette touche est utilisé
                {
                    GG.GB.UseBoost();//Lance le boost
                    SetBoolArray(0, true);//set le bool
                }

                if (Input.GetAxisRaw(Axes[0]) == 0 && InUse[0])// s'il ne boost pas
                {
                    SetBoolArray(0, false);//set le bool
                }

                if (Input.GetAxisRaw(Axes[7]) != 0)//s'il ne drift pas
                {

                    bool state = true;
                    if (!InUse[7])//s'il n'utilise pas le drift
                    {
                    SetBoolArray(7, state);
                    }
                    bool LoseSpeed = false;
                    GG.GMC.TourneDerapage(Input.GetAxisRaw(Axes[7]), Input.GetAxis(Axes[3]), out LoseSpeed);//tourner la moto par le dérapage
                    GG.GMC.derapage(state, Input.GetAxis(Axes[3]), LoseSpeed);//effectue le dérapage
                    GG.CSF.InfoRotationDeLaCam(Input.GetAxis(Axes[3])); //envois des info pour la cam
                }
                if (Input.GetAxisRaw(Axes[7]) == 0 && InUse[7])
                {
                    bool state = false;
                    GG.CSF.InfoRotationDeLaCam(Input.GetAxis(Axes[3])); //envois des info pour la cam
                    GG.GMC.derapage(state, 0, false);// lance le dérapage 
                    SetBoolArray(7, state);//set le bool
                    GG.FeedBackVisu.GestionStraff(state, false);//lance le feedback straff
                    GG.FeedBackVisu.GestionSmoke(state);//lance le feedback smoke
                    GG.FeedBackVisu.GestionWheelTrail(state);// lance le feedback de wheeltrail
                    GG.FeedBackVisu.GestionParticleRoue(state);// lance le feedback gestion particle roue
                }
            GG.FeedBackVisu.UpdateLesFX();// update les FX
            }

        if (Input.GetAxis(Axes[9]) != 0)//si le joueur reset la scene
        {
            GG.EtatEtFeedback.stopAllSound(); //stop les sons
            Physics.gravity= new Vector3(0, -9.81f, 0); //Change la gravity
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload la scene
        }
        if (Input.GetAxis(Axes[8]) != 0)
        {
            GG.GMC.ResetLastPosition();//lance le reset de la position
        }
        if (Input.GetAxis(Axes[10]) != 0)//si le joueur reset la scene
        {
            GG.EtatEtFeedback.stopAllSound(); //stop les sons
            Physics.gravity = new Vector3(0, -9.81f, 0); //Change la gravity
            SceneManager.LoadScene(SceneToLoad); //reload la scene
        }

    }

    void setTableauLength() 
    {
        TimeMaintenueAxes = new float[Axes.Length];
        TimePressionAxes = new float[Axes.Length];
        InUse = new bool[Axes.Length];
    }// set la length des tableaux

    void SetBoolArray(int IndexAxes,bool value) 
    {
        InUse[IndexAxes] = value;
    }// set le bool array

}
