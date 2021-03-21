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
        //Ce qui est sur de ce faire à chaque frame
        GG.GMC.SetByNormal();
        GG.GMC.avance(Input.GetAxis(Axes[1]));
        GG.GMC.tourne(Input.GetAxis(Axes[3]));

        //Input

        if (Input.GetAxisRaw(Axes[0]) != 0)
        {
            GG.GB.UseBoost();
        }

    }
}
/*
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
