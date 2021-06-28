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
        int IndexGamePad = 0;
        if (!GG.AmIPlayerOne)
        {
            IndexGamePad = 1;
        }
        float resetPos = 0;
        if (Hinput.gamepad[IndexGamePad].X)
        {
            resetPos = 1;
        }
        float boostVal = 0;
        if (Hinput.gamepad[IndexGamePad].A)
        {
            boostVal = 1;
        }
        float driftVal = 0;
        if (Hinput.gamepad[IndexGamePad].leftBumper)
        {
            driftVal--;
        }
        if (Hinput.gamepad[IndexGamePad].rightBumper)
        {
            driftVal++;
        }
        float DirectionJoystick = Hinput.gamepad[IndexGamePad].leftStick.horizontal;
        float DirectionJoystickDroit = Hinput.gamepad[IndexGamePad].rightStick.horizontal;
        float DirectionVal = 0;
        if (Hinput.gamepad[IndexGamePad].leftTrigger)
        {
            DirectionVal -= Hinput.gamepad[IndexGamePad].leftTrigger.position;
        }
        if (Hinput.gamepad[IndexGamePad].rightTrigger)
        {
            DirectionVal += Hinput.gamepad[IndexGamePad].rightTrigger.position;
        }

        GG.GMC.SetByNormal(); // check le sol 
        GG.CSF.CameraComportement(); // fais les comportements de la camera
        GG.GMC.RotateMotoInWorld();//rotate le modéle de la moto

        if (!InUse[7])//s'il ne drift pas
        {
            GG.GMC.tourne(DirectionJoystick); //Lance void pour Tourner
            if (GG.CanPlay)//si le joueur peut jouer
            {
                GG.CSF.InfoRotationDeLaCam(DirectionJoystick); //envois des info pour la cam
            }
        }

        if (!InUse[7] && DirectionJoystick == 0 && GG.CanPlay)//s'il ne drift pas et ne tourne pas et le joueur peut jouer
        {
            GG.GMC.straff(DirectionJoystickDroit);//lance le straff
            if (DirectionJoystickDroit == 0)//s'il ne straff pas
            {
                GG.FeedBackVisu.GestionStraff(false, false);//lance le feedback de straff
            }
            GG.CSF.InfoRotationDeLaCam(0); //envois des info pour la cam
        }

        if (GG.CanPlay)//si le joueur peut jouer
        {
            GG.GMC.avance(DirectionVal);//lance la marche avant
        }
        else
        {
            GG.GMC.avance(0);//lui dis de pas avancer
        }
        if (GG.CanPlay)//si le joueur peut jouer
        {
            if (boostVal != 0 && !InUse[0])//si cette touche est utilisé
            {
                GG.GB.UseBoost();//Lance le boost
                SetBoolArray(0, true);//set le bool
            }

            if (boostVal == 0 && InUse[0])// s'il ne boost pas
            {
                SetBoolArray(0, false);//set le bool
            }

            if (driftVal != 0)//s'il ne drift pas
            {

                bool state = true;
                if (!InUse[7])//s'il n'utilise pas le drift
                {
                    SetBoolArray(7, state);
                }
                bool LoseSpeed = false;
                GG.GMC.TourneDerapage(driftVal, DirectionJoystick, out LoseSpeed);//tourner la moto par le dérapage
                GG.GMC.derapage(state, DirectionJoystick, LoseSpeed);//effectue le dérapage
                GG.CSF.InfoRotationDeLaCam(DirectionJoystick); //envois des info pour la cam
            }
            if (driftVal == 0 && InUse[7])
            {
                bool state = false;
                GG.CSF.InfoRotationDeLaCam(DirectionJoystick); //envois des info pour la cam
                GG.GMC.derapage(state, 0, false);// lance le dérapage 
                SetBoolArray(7, state);//set le bool
                GG.FeedBackVisu.GestionStraff(state, false);//lance le feedback straff
                GG.FeedBackVisu.GestionSmoke(state);//lance le feedback smoke
                GG.FeedBackVisu.GestionWheelTrail(state);// lance le feedback de wheeltrail
                GG.FeedBackVisu.GestionParticleRoue(state);// lance le feedback gestion particle roue
            }
            GG.FeedBackVisu.UpdateLesFX();// update les FX
        }
        if (Hinput.anyGamepad.Y)
        {
            GG.EtatEtFeedback.stopAllSound(); //stop les sons
            GG.GestionPointDeControle.stopSound();
            Physics.gravity = new Vector3(0, -9.81f, 0); //Change la gravity
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload la scene
        }

        if (!GG.TwoPlayer)
        {
            if (Input.GetAxis(Axes[9]) != 0)//si le joueur reset la scene
            {
                GG.EtatEtFeedback.stopAllSound(); //stop les sons
                GG.GestionPointDeControle.stopSound();
                Physics.gravity = new Vector3(0, -9.81f, 0); //Change la gravity
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload la scene
            }
            if (Input.GetAxis(Axes[10]) != 0)//si le joueur reset la scene
            {
                //GG.EtatEtFeedback.stopAllSound(); //stop les sons
                //Physics.gravity = new Vector3(0, -9.81f, 0); //Change la gravity
                // SceneManager.LoadScene(SceneToLoad); //reload la scene
            }
        }

        if (resetPos != 0)
        {
           
            GG.GMC.ResetLastPosition();//lance le reset de la position
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
/* 

/*
       float avancer = GG.newsInput.Manette.Avancer.ReadValue<float>();

       float tourner = GG.newsInput.Manette.Tourner.ReadValue<float>();

       float Derapage = GG.newsInput.Manette.Derapage.ReadValue<float>();

       float Straff = GG.newsInput.Manette.Straffer.ReadValue<float>();

       float Booster = GG.newsInput.Manette.Boost.ReadValue<float>();

       float ResetPosition = GG.newsInput.Manette.ResetPosition.ReadValue<float>();

       float ResetScene = GG.newsInput.Manette.ResetScene.ReadValue<float>();
float avancer = 0;
float Booster = 0;
float tourner = 0;
float Derapage = 0;
float Straff = 0;
float ResetPosition = 0;
float ResetScene = 0;
if (GG.AmIPlayerOne)
{

    avancer = 0;
    Booster = 0;
    tourner = 0;
    Derapage = 0;
    Straff = 0;
    Booster = 0;
    ResetPosition = 0;
    if (Hinput.gamepad[0].A)
    {

        Booster = 1;
    }
    if (Hinput.gamepad[0].X)
    {
        ResetPosition = 1;
    }
    if (Hinput.gamepad[0].leftBumper)
    {
        Derapage--;
    }
    if (Hinput.gamepad[0].rightBumper)
    {
        Derapage++;
    }
    if (Hinput.gamepad[0].leftTrigger)
    {
        avancer -= Hinput.gamepad[0].leftTrigger.position;
    }
    if (Hinput.gamepad[0].rightTrigger)
    {
        avancer += Hinput.gamepad[0].rightTrigger.position;
    }
    if (Hinput.gamepad[0].Y && !GG.TwoPlayer)
    {
        ResetScene = 1;
    }
    Vector2 Montourner = Hinput.gamepad[0].leftStick;
    tourner = Montourner.x;
    Vector2 MonStraff = Hinput.gamepad[0].rightStick;
    Straff = MonStraff.x;
}
if (!GG.AmIPlayerOne && GG.TwoPlayer)
{
    avancer = 0;
    Booster = 0;
    tourner = 0;
    Derapage = 0;
    Straff = 0;
    ResetPosition = 0;
    if (Hinput.gamepad[1].A)
    {
        Booster = 1;
    }
    if (Hinput.gamepad[1].X)
    {
        ResetPosition = 1;
    }
    if (Hinput.gamepad[1].leftBumper)
    {
        Derapage--;
    }
    if (Hinput.gamepad[1].rightBumper)
    {
        Derapage++;
    }
    if (Hinput.gamepad[1].leftTrigger)
    {
        avancer -= Hinput.gamepad[1].leftTrigger.position;
    }
    if (Hinput.gamepad[1].rightTrigger)
    {
        avancer += Hinput.gamepad[1].rightTrigger.position;
    }
    Vector2 Montourner = Hinput.gamepad[1].leftStick;
    tourner = Montourner.x;
    Vector2 MonStraff = Hinput.gamepad[1].rightStick;
    Straff = MonStraff.x;



}
if (!InUse[7])//s'il ne drift pas
{
    GG.GMC.tourne(tourner); //Lance void pour Tourner
    if (GG.CanPlay)//si le joueur peut jouer
    {
        GG.CSF.InfoRotationDeLaCam(tourner); //envois des info pour la cam
    }
}

if (!InUse[7] && tourner == 0 && GG.CanPlay)//s'il ne drift pas et ne tourne pas et le joueur peut jouer
{
    GG.GMC.straff(Straff);//lance le straff
    if (Straff == 0)//s'il ne straff pas
    {
        GG.FeedBackVisu.GestionStraff(false, false);//lance le feedback de straff
    }
    GG.CSF.InfoRotationDeLaCam(0); //envois des info pour la cam
}

if (GG.CanPlay)//si le joueur peut jouer
{
    GG.GMC.avance(avancer);//lance la marche avant
}
else
{
    GG.GMC.avance(0);//lui dis de pas avancer
}
if (GG.CanPlay)//si le joueur peut jouer
{
    if (Booster != 0 && !InUse[0])//si cette touche est utilisé
    {
        GG.GB.UseBoost();//Lance le boost
        SetBoolArray(0, true);//set le bool
    }

    if (Booster == 0 && InUse[0])// s'il ne boost pas
    {
        SetBoolArray(0, false);//set le bool
    }

    if (Derapage != 0)//s'il ne drift pas
    {

        bool state = true;
        if (!InUse[7])//s'il n'utilise pas le drift
        {
            SetBoolArray(7, state);
        }
        bool LoseSpeed = false;
        GG.GMC.TourneDerapage(Derapage, tourner, out LoseSpeed);//tourner la moto par le dérapage
        GG.GMC.derapage(state, tourner, LoseSpeed);//effectue le dérapage
        GG.CSF.InfoRotationDeLaCam(tourner); //envois des info pour la cam
    }
    if (Derapage == 0 && InUse[7])
    {
        bool state = false;
        GG.CSF.InfoRotationDeLaCam(tourner); //envois des info pour la cam
        GG.GMC.derapage(state, 0, false);// lance le dérapage 
        SetBoolArray(7, state);//set le bool
        GG.FeedBackVisu.GestionStraff(state, false);//lance le feedback straff
        GG.FeedBackVisu.GestionSmoke(state);//lance le feedback smoke
        GG.FeedBackVisu.GestionWheelTrail(state);// lance le feedback de wheeltrail
        GG.FeedBackVisu.GestionParticleRoue(state);// lance le feedback gestion particle roue
    }
    GG.FeedBackVisu.UpdateLesFX();// update les FX
}
if (!GG.TwoPlayer)
{
    if (ResetScene != 0)//si le joueur reset la scene
    {
        GG.EtatEtFeedback.stopAllSound(); //stop les sons
        GG.GestionPointDeControle.stopSound();
        Physics.gravity = new Vector3(0, -9.81f, 0); //Change la gravity
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload la scene
    }
    if (Input.GetAxis(Axes[10]) != 0)//si le joueur reset la scene
    {
        //GG.EtatEtFeedback.stopAllSound(); //stop les sons
        //Physics.gravity = new Vector3(0, -9.81f, 0); //Change la gravity
        // SceneManager.LoadScene(SceneToLoad); //reload la scene
    }
}

if (ResetPosition != 0)
{
    GG.GMC.ResetLastPosition();//lance le reset de la position
}*/