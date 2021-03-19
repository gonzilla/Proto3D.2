using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionUI : PersonnalMethod
{
    //Public variable
    public Text Speed;
    public Slider BoostVisual;
    public Rigidbody Moto;
    public int arrondisDecimal = 2;
    //Local variable
    GestionGeneral GG;

    void Start()
    {
        GetGestion(out GG, this.gameObject);
    }

    
    void Update()
    {
        float valueVelocity = GG.GMC.VitesseMoto * 100;
        float arrondis = (float)System.Math.Round(valueVelocity, arrondisDecimal);
        Speed.text = arrondis.ToString();
    }

    public void setSliderBoost(float valueToShow) 
    {
        BoostVisual.value = valueToShow;
    
    }
}
