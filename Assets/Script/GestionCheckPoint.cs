using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionCheckPoint : PersonnalMethod
{
    //Public variable
    public Text AffichageTour;
    public int NombrePointDePassage;
    public int NombreDeTour;
    //Local variable
    int passageActuel;
    int tourActuel;

    GestionGeneral GG;

    void Start()
    {
        GetGestion(out GG, this.gameObject);
        string affichage = tourActuel.ToString() + "/" + NombreDeTour.ToString();
        AffichageTour.text = affichage;
        passageActuel = NombrePointDePassage;
    }

    
    public void CheckLeCheckPoint(int lecheckPoint) 
    {
        if (lecheckPoint==passageActuel+1)
        {
            
            passageActuel++;
        }
        if (lecheckPoint==0 && passageActuel==NombrePointDePassage)
        {
            tourActuel++;
            passageActuel = 0;
            string affichage = tourActuel.ToString() + "/" + NombreDeTour.ToString();
            AffichageTour.text = affichage;
        }
    }

    void FinDeLaPartie() 
    {
    // Désactiver 
    //Gestion Input + Gestion Cam
    }
}
