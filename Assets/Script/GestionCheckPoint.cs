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
        FMODUnity.RuntimeManager.PlayOneShot("event:/Musique");
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
            MonEvenementFMOD.setParameterByName("Laps", NombreDeTour);
            //GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.);
        }
    }

    void FinDeLaPartie() 
    {
        GG.CanPlay = false;
        GG.Start = false;
    }
}
