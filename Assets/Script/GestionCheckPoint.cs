using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GestionCheckPoint : PersonnalMethod
{
    //Public variable
    public TextMeshProUGUI AffichageTour;
    public int NombrePointDePassage;
    public int NombreDeTour;

    public bool testMusique;
    //Local variable
    int passageActuel;
    [HideInInspector] public int tourActuel;
   
    [FMODUnity.EventRef]
    public string LeSonAJouer;
    FMOD.Studio.EventInstance MonEvenementFMOD;
    GestionGeneral GG;

    void Start()
    {
        GetGestion(out GG, this.gameObject);
        string affichage = tourActuel.ToString() + "/" + NombreDeTour.ToString();
        AffichageTour.text = affichage;
        passageActuel = NombrePointDePassage;
        if (!testMusique)
        {
            MonEvenementFMOD = FMODUnity.RuntimeManager.CreateInstance("event:/Musique");
            MonEvenementFMOD.start();
           
        }
       


    }

    


    public void CheckLeCheckPoint(int lecheckPoint) 
    {
        if (lecheckPoint==passageActuel+1)
        {
            
            passageActuel++;
            
        }
        if (lecheckPoint==0 && passageActuel==NombrePointDePassage)
        {
            print("set");
            tourActuel++;
            GG.GUI.CheckTimePourMeilleurTour(tourActuel);
            if (tourActuel> NombreDeTour)
            {
                tourActuel = NombreDeTour;
                GG.GUI.AffichageStats();
                
                FinDeLaPartie();
            }
            passageActuel = 0;
            string affichage = tourActuel.ToString() + "/" + NombreDeTour.ToString();
            AffichageTour.text = affichage;
            float act = 0;
            if (tourActuel == 1)
            {
                act = 0f;
                print("0");
            }
            else if (tourActuel == 2)
            {
                print("2");
                act = 1.1f;
            }
            else if (tourActuel == 3)
            {
                print("else");
                act = tourActuel;
            }
            MonEvenementFMOD.setParameterByName("Laps", act);
            //GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.);
        }
    }
    public void stopSound() 
    {
    MonEvenementFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    void FinDeLaPartie() 
    {
        GG.CanPlay = false;
        GG.Start = false;
    }
}
