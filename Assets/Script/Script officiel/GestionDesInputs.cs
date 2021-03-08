using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionDesInputs : PersonnalMethod
{
    //Public variable
    //public int PlayerIndex;//later
    public string[] Axes; //Tableau des Inputs qui ont besoins d'une valeur de pression
    [HideInInspector] public float[] TimeMaintenueAxes;//le temps que le joueur maintien cette input
    [HideInInspector] public float[] TimePressionAxes;
    public float timeMinMaintien;// temps à partir duquel on décide qu'une touche est maintenue volontairement 
    public float TempsMaxMaintien;
    //Local variable
    GestionGeneral GG;




    void Start()
    {
        GetGestion(out GG, this.gameObject);
    }

    
    void FixedUpdate()
    {
        if (Input.GetAxisRaw(Axes[0])!=0)
        {
            GG.GB.UseBoost();
        }
    }
}
