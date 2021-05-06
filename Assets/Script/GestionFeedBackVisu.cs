using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionFeedBackVisu : PersonnalMethod
{
    // public 
    public GameObject[] BoostLV;
    public GameObject WindTrail;
    public GameObject Smoke;
    public GameObject RedLightTrail;
    public GameObject WheelTrail;
    public GameObject Straff;

    //private
    bool[] BoostLVEtat;
    GestionGeneral GG;
    

    // Start is called before the first frame update
    void Start()
    {
        GetGestion(out GG, this.gameObject);
        setBoolArrayBoostLV();
    }

    public void gestionBoost(int LevelDeBoost, bool Etat) 
    {
    
    }

    public void GestionWindTrail(bool Etat) 
    {
    
    }
    public void GestionSmoke(bool Etat) 
    {
    
    }
    public void GestionRedLight(bool Etat) 
    {
    
    }

    public void GestionWheelTrail(bool Etat) 
    {
    
    }
    public void GestionStraff(bool Etat) 
    {
    
    }



    void setBoolArrayBoostLV() 
    {

        BoostLVEtat= new bool[BoostLV.Length];
        for (int i = 0; i < BoostLVEtat.Length; i++)
        {
            BoostLVEtat[i] = false;
        }
    }
}
