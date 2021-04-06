using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionUI : PersonnalMethod
{
    //Public variable
    [Tooltip(" Le text qui affiche la vitesse ")]
    public Text Speed;
    [Tooltip(" Le slider pour la jauge de boost ")]
    public Slider BoostVisual;
    [Tooltip(" Le rigidbody de la moto pour la physique ")]
    public Rigidbody Moto;
    [Tooltip(" arrondis la valeur afficher ")]
    public int arrondisDecimal = 2;
    //Local variable
    GestionGeneral GG;// récupére les autres script

    void Start()
    {
        GetGestion(out GG, this.gameObject);// récupére les autres script
    }

    
    void Update()
    {
        float valueVelocity = GG.GMC.VitesseMoto * 100;//multiplie la valeur de la moto 
        float arrondis = (float)System.Math.Round(valueVelocity, arrondisDecimal);//arrondis la valeur a afficher
        Speed.text = arrondis.ToString();//affiche la valeur
    }

    public void setSliderBoost(float valueToShow) 
    {
        
        BoostVisual.value = valueToShow; // set la value du slider
    
    }
}
