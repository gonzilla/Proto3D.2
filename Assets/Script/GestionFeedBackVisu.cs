using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionFeedBackVisu : PersonnalMethod
{
    // public 
    public GameObject[] BoostLV;
    public GameObject WindTrail;
    public GameObject[] Smoke;
    public float MaximumTailleSmoke;
    public GameObject RedLightTrail;
    public GameObject WheelTrail;
    public GameObject[] Straff;
    public GameObject[] ParticleRoue;
    public GameObject ParticleCam;
    public GameObject Fusion;
    public GameObject Recharge;

    //private
    bool[] BoostLVEtat;
    bool[] SmokesEtat;
    bool[] StraffEtat;
    bool[] ParticleRoueEtat;
    bool windTrailPrevious;

    GestionGeneral GG;
    //ParticleSystem[] Smoker;
    
    // Start is called before the first frame update
    void Start()
    {
        GetGestion(out GG, this.gameObject);
        setArrayBoost();
        
    }
    #region Gestions
    public void gestionBoost(int LevelDeBoost, bool Etat)
    {
        if (Etat)
        {
            for (int i = 0; i < BoostLVEtat.Length; i++)
            {
                BoostLVEtat[i] = false;
                if (i == LevelDeBoost)
                {
                    BoostLVEtat[i] = true;
                }
                LancementFonction(BoostLVEtat[i], BoostLV[i]);
                //BoostLV[i].SetActive(BoostLVEtat[i]);
            }

        }
        else
        {
            
            for (int i = 0; i < BoostLVEtat.Length; i++)
            {
                BoostLVEtat[i] = false;
                //BoostLV[i].SetActive(BoostLVEtat[i]);
                LancementFonction(BoostLVEtat[i], BoostLV[i]);
            }
        }
    }
    public void GestionWindTrail(bool Etat)
    {
        
        if (Etat && windTrailPrevious!=Etat )
        {
            // WindTrail.SetActive(Etat);
           // windTrailPrevious = Etat;
           // LancementFonction(Etat, WindTrail);
        }
        else if (!Etat && windTrailPrevious != Etat)
        {
           // windTrailPrevious = Etat;
           // LancementFonction(Etat, WindTrail);
            //WindTrail.SetActive(Etat);
        }
        LancementFonction(Etat, WindTrail);

    }
    public void GestionSmoke(bool Etat)
    {
        if (Etat && directionDeRotation()!=0)
        {
            int Index = 0;
            if (directionDeRotation()>0) //&& !Smoke[0].activeSelf
            {

                Index = 0;
            }
            else if (directionDeRotation() < 0 )
            {
                Index = 1;
            }
            for (int i = 0; i < SmokesEtat.Length; i++)
            {
                if (i==Index)
                {
                    //Smoke[i].SetActive(true);
                    LancementFonction(true, Smoke[i]);

                }
                else 
                {
                    //Smoke[i].SetActive(false);
                    LancementFonction(false, Smoke[i]);
                }
            }
            
        }
        else 
        {
            for (int i = 0; i < SmokesEtat.Length; i++)
            {
                // Smoke[i].SetActive(false);
                LancementFonction(false, Smoke[i]);
            }
           
            resetDeBool(SmokesEtat);

        }
    }
    public void GestionRedLight(bool Etat)
    {
        if (!RedLightTrail.activeSelf && Etat)
        {
            RedLightTrail.SetActive(Etat);
        }
        else if (!Etat && RedLightTrail.activeSelf)
        {
            RedLightTrail.SetActive(Etat);
        }
        
    }
    public void GestionWheelTrail(bool Etat)
    {
        LancementFonction(Etat, WheelTrail);
            //WheelTrail.SetActive(Etat);
        
    }
    public void GestionStraff(bool Etat, bool straffing)
    {
        if (Etat)
        {
            if (directionDuStarff() != 0 && straffing)
            {
                int Index = 0;
                int Index2 = 0;
                if (directionDuStarff() > 0)
                {

                    Index = 0;
                    Index2 = 1;
                }
                else if (directionDuStarff() < 0)
                {
                    Index = 2;
                    Index2 = 3;
                }
                for (int i = 0; i < Straff.Length; i++)
                {
                    if (i == Index || i== Index2)
                    {
                        Straff[i].SetActive(true);
                    }
                    else
                    {
                        Straff[i].SetActive(false);
                    }
                }

            }
            if (!straffing && directionDeRotation()!=0)
            {
                int index = 0;
                if (directionDeRotation()>0)
                {
                    index = 3;
                }
                if (directionDeRotation() < 0)
                {
                    index = 1;
                }
                for (int i = 0; i < Straff.Length; i++)
                {
                    if (i == index)
                    {
                        Straff[i].SetActive(true);
                    }
                    else
                    {
                        Straff[i].SetActive(false);
                    }
                }
            }
        }
        
        else
        {
            for (int i = 0; i < Straff.Length; i++)
            {
                Straff[i].SetActive(false);
            }

           

        }
    }
    public void GestionParticleRoue(bool Etat) 
    {
        if (Etat && directionDeRotation()!=0)
        {
            int index = 0;
            if (directionDeRotation() == -1)
            {
                index = 0;
            }//pour l'activation des particules
            if (directionDeRotation() == 1)
            {
                index = 1;
            }//pour l'activation des particules*/

            for (int i = 0; i < ParticleRoue.Length; i++)
            {
                if (i==index)
                {
                    ParticleRoue[i].SetActive(true);
                    //LancementFonction(true, ParticleRoue[i]);
                }
                else
                {
                    ParticleRoue[i].SetActive(false);
                    //LancementFonction(false, ParticleRoue[i]);
                }
            }
        }
        else 
        {

            for (int i = 0; i < ParticleRoue.Length; i++)
            {
                ParticleRoue[i].SetActive(false);
                //LancementFonction(false, ParticleRoue[i]);
            }

        }
        
       
    }
    public void GestionParticleCam(bool etat) 
    {
        ParticleCam.SetActive(etat);
    }
    public void GestionParticleFusion(bool Etat) 
    {
        //Fusion.SetActive(Etat);
        LancementFonction(Etat, Fusion);
       
      
        //
    }
    public void GestionParticleRecharge(bool Etat) 
    {
        LancementFonction(Etat, Recharge);
    }
    #endregion

    #region fonction
    void LancementFonction(bool Etat, GameObject Objet)
    {
        List<Transform> mesEnfants = new List<Transform>();
        foreach (Transform item in Objet.transform)
        {

            mesEnfants.Add(item);
           
        }
        foreach (Transform item in mesEnfants)
        {
            if (item.GetComponent<ParticleSystem>() != null)
            {
                FonctionParticle(Etat, item);
            }
            if (item.GetComponent<TrailRenderer>() != null)
            {
                FonctionTrail(Etat, item);
            }
        }
    }
    void FonctionTrail(bool Etat, Transform Enfant)
    {

        if (Etat)
        {
            Enfant.GetComponent<TrailRenderer>().enabled = Etat;
        }
        else
        {

            GameObject NouveauTrail = Instantiate(Enfant.gameObject);
            NouveauTrail.transform.parent = Enfant.parent;
            NouveauTrail.transform.localPosition = Enfant.localPosition;
            NouveauTrail.name = Enfant.name;
            NouveauTrail.GetComponent<TrailRenderer>().enabled = false;
            Enfant.parent = null;
            Enfant.GetComponent<DestroyYourself>().DestroyToi();
        }

    }
    void FonctionParticle(bool Etat, Transform Enfant)
    {
        if (Etat)
        {

            Enfant.GetComponent<ParticleSystem>().Play();


        }
        else
        {

            Enfant.GetComponent<ParticleSystem>().Stop();


        }
    }
    #endregion

    float VitesseActuelDeLaMoto()
    {
        float ActualSpeedMoto = GG.GMC.VitesseMoto;
        return ActualSpeedMoto;
    }

    bool Avance()
    {
        if (VitesseActuelDeLaMoto() > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    float directionDuStarff() 
    {
        float direction = 0;
        float val = Input.GetAxis(GG.GDI.Axes[5]);//GG.GDI.Axes;
        if (val != 0)
        {
            direction = Mathf.Abs(val) / val;
        }

        return direction;
    }

    float directionDeRotation() 
    {
        float direction = 0;
        float val = GG.GMC.ActuelVitesseRotation;
        if (val!=0)
        {
            direction = Mathf.Abs(val) / val;
        }
       
        return direction;

    }

    void setArrayBoost()
    {

        BoostLVEtat = new bool[BoostLV.Length];
        BoostLVEtat = resetDeBool(BoostLVEtat);
        SmokesEtat = new bool[Smoke.Length];
        SmokesEtat = resetDeBool(SmokesEtat);
        StraffEtat = new bool[Straff.Length];
        StraffEtat = resetDeBool(StraffEtat);
        ParticleRoueEtat = new bool[ParticleRoue.Length];
        ParticleRoueEtat = resetDeBool(ParticleRoueEtat);
    }

    bool[] resetDeBool (bool[] toReset)
    {

        for (int i = 0; i < toReset.Length; i++)
        {
            toReset[i] = false;
        }
        return toReset;
    }
}
/*for (int i = 0; i < SmokesEtat.Length; i++)
            {
                var smoker = Smoke[i].GetComponent<ParticleSystem>().main;
                Smoke[i].GetComponent<ParticleSystem>().Stop();
                if (i==Index)
                {
                    
                    smoker.startSize = 4;
                    //Smoke[i].GetComponent<ParticleSystem.MainModule>().startSize= 4;
                }
                else 
                {
                    smoker.startSize = 1;
                }
                Smoke[i].GetComponent<ParticleSystem>().Play();
            }*/
/*void setParticleSystemArray() 
   {
       for (int i = 0; i < Smoker.Length; i++)
       {
           Smoker[i] = Smoke[i].GetComponent<ParticleSystem.MainModule>();
       }

   }*/