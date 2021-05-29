using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;



[System.Serializable]
public class PostFXEffet //Scipt pour les infos des effets de post process
{

    public enum PostProcessEffect //les différents post process possible
    {
        AmbientOcclusions,
        AutoExposure,
        Bloom,
        ChromaticAbberration,
        ColorGrading,
        DepthOfField,
        Grain,
        LensDistortion,
        MotionBlur,
        ScreenSpaceRefecltion,
        Vignette

    };
    [Tooltip("les effets que je vais utiliser")]
    public PostProcessEffect MesPostProcessEffect;
    [Tooltip("les values max que je vais utiliser lors des effets")]
    public float[] LesValuesMax;
    [Tooltip("vitesse de transition de l'effet")]
    public float VitesseDeleffet;

}
public class GestionFeedBackVisu : PersonnalMethod
{
    //Scipt qui gére les feedbacks visuels
    //public
    // les gameobjets contenant les effets
    #region GameObject
    [Header("GameOjects")]
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
    #endregion 
    
    #region PostProcess
    [Header("PostProcess")]
    [Tooltip("l'objetContenant le post process Volume")]
    public PostProcessVolume monPostProcess;
    [Tooltip("liste des effets du post process Volume")]
    public List<PostFXEffet> MesEffets = new List<PostFXEffet>();
    [Tooltip("la vitesse de retour à leur valeur normal")]
    public float vitesseretourMultiplicateur;
    #endregion

    //private
    bool[] BoostLVEtat;
    bool[] SmokesEtat;
    bool[] StraffEtat;
    bool[] ParticleRoueEtat;
    bool windTrailPrevious;

    float bloomValueDepart;
    float ChromaticValueDepart;
    float LensDistortionValueDepart;
    GestionGeneral GG;
    //les différents postProcess
    #region PostProcess2
    AmbientOcclusion _AmbientOclu;
    AutoExposure _AutoExpo;
    Bloom _bloom;
    ChromaticAberration _Chromatic;
    ColorGrading _ColorGrad;
    DepthOfField _DepthOf;
    Grain _Grain;
    LensDistortion _Lens;
    MotionBlur _MotionBlur;
    ScreenSpaceReflections _ScreenSpace;
    Vignette _Vignette;
    #endregion
    

   
    void Start()
    {
        GetGestion(out GG, this.gameObject);// récupére la gestion general
        setArrayBoost();// set les bonnes données
        setPostProcess();//  set les données de post process

    }//au start
    #region Gestions
    public void gestionBoost(int LevelDeBoost, bool Etat)//Gére les effets de boost
    {
        if (Etat)// si etat = true
        {
            for (int i = 0; i < BoostLVEtat.Length; i++)
            {
                BoostLVEtat[i] = false;//lui dis que les autre etat sont faux
                if (i == LevelDeBoost)//si c'est ce level de boost
                {
                    BoostLVEtat[i] = true;//le passe a true
                }
                LancementFonction(BoostLVEtat[i], BoostLV[i]); //lance un autre void           
            }

        }
        else //si etat =false
        {

            for (int i = 0; i < BoostLVEtat.Length; i++)// desactive tous les effets de boosts
            {
                BoostLVEtat[i] = false;//set les etats dans son tableau          
                LancementFonction(BoostLVEtat[i], BoostLV[i]);//lance le void
            }
        }
    }
    public void GestionWindTrail(bool Etat)
    {
        LancementFonction(Etat, WindTrail);//lance le void
    }//gére les effets de vents
    public void GestionSmoke(bool Etat)//Gère les smokes
    {
        if (Etat && directionDeRotation() != 0) // si etat = true et je tourne
        {
            int Index = 0;
            //détermine quel smoke a utiliser
            if (directionDeRotation() > 0) 
            {
                Index = 0; 
            }
            else if (directionDeRotation() < 0)
            {
                Index = 1;
            }
            // pour chaque smoke dans le tableau
            for (int i = 0; i < SmokesEtat.Length; i++)
            {
                if (i == Index)
                {
                   
                    LancementFonction(true, Smoke[i]);//lance le void

                }
                else
                {
                   
                    LancementFonction(false, Smoke[i]);//lance le void
                }
            }

        }
        else //sinon
        {
            for (int i = 0; i < SmokesEtat.Length; i++)//pour chaque smoke
            {
                LancementFonction(false, Smoke[i]);//lance le void
            }
            resetDeBool(SmokesEtat);//lance le void
        }
    }
    public void GestionRedLight(bool Etat)
    {
        if (!RedLightTrail.activeSelf && Etat)
        {
            RedLightTrail.SetActive(Etat);//active le gameobject de la relight
        }
        else if (!Etat && RedLightTrail.activeSelf)
        {
            RedLightTrail.SetActive(Etat);//désactive le gameobject de la relight
        }
    }//gére la redlight
    public void GestionWheelTrail(bool Etat)
    {
        LancementFonction(Etat, WheelTrail); //lance le void
    }
    public void GestionStraff(bool Etat, bool straffing)
    {
        if (Etat)//si etat = true
        {
            if (directionDuStarff() != 0 && straffing)//s'il straff 
            {
                //Détermine les feedbacks a activer
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

                for (int i = 0; i < Straff.Length; i++)// par chaque objet dans straff
                {
                    if (i == Index || i == Index2)
                    {
                        Straff[i].SetActive(true);//acive l'objet
                    }
                    else
                    {
                        Straff[i].SetActive(false);//désactive l'objet
                    }
                }

            }
            if (!straffing && directionDeRotation() != 0)
            {
                int index = 0;
                if (directionDeRotation() > 0)
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
            }//si le joueur ne straf plus et il tourne
        }
        else //sinon
        {
            for (int i = 0; i < Straff.Length; i++)
            {
                Straff[i].SetActive(false); // desactive l'objet
            }



        }
    }//gére les feedbacks de straff
    public void GestionParticleRoue(bool Etat)
    {
        if (Etat && directionDeRotation() != 0)
        {
            int index = 0;
            if (directionDeRotation() == -1)
            {
                index = 0;
            }
            if (directionDeRotation() == 1)
            {
                index = 1;
            }

            for (int i = 0; i < ParticleRoue.Length; i++)
            {
                if (i == index)
                {
                    ParticleRoue[i].SetActive(true);
                   
                }
                else
                {
                    ParticleRoue[i].SetActive(false);
                    
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


    }//Gestion des particules de la roue
    public void GestionParticleCam(bool etat)
    {
        ParticleCam.SetActive(etat);//lance le void
    }//Gestion des particules de la cam
    public void GestionParticleFusion(bool Etat) //gestion des particules de fusion
    {
        LancementFonction(Etat, Fusion);//lance le void
    }
    public void GestionParticleRecharge(bool Etat)
    {
        
        LancementFonction(Etat, Recharge);//lance le void
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
    }//détermine quel fonction lancé
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

    }//si l'effet est un trail
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
    }//si l'effet est une particule
    #endregion

    public void UpdateLesFX() //update les FX
    {
        if (GG.GB.boosting)//gére les effets si le joueur boost
        {
            
            if (_bloom.intensity.value != MesEffets[0].LesValuesMax[1])
            {
                _bloom.intensity.value = MesEffets[0].LesValuesMax[1]; 
                
            }//bloom
            if (_Chromatic.intensity.value != MesEffets[2].LesValuesMax[1])
            {
                _Chromatic.intensity.value = MesEffets[2].LesValuesMax[1];
                
            }//chromatic aberation
            if (_Vignette.intensity.value!= MesEffets[3].LesValuesMax[1])
            {
                _Vignette.intensity.value = MesEffets[3].LesValuesMax[1]; 
               
            }//vignette
            if (_Lens.intensity.value!= MesEffets[6].LesValuesMax[1])
            {
                _Lens.intensity.value = MesEffets[6].LesValuesMax[1]; 
                
            }//lens
            
        }
        else if (!GG.GB.boosting && !GG.GB.Surchauffing)//calcul des effets au quotidien
        {
            
            _bloom.intensity.value = CheckParamettreFX(_bloom.intensity.value, MesEffets[0].LesValuesMax[0]*pourcentageVitesse(), MesEffets[0].VitesseDeleffet);//bloom
            _Chromatic.intensity.value = CheckParamettreFX(_Chromatic.intensity.value, MesEffets[2].LesValuesMax[0] * pourcentageVitesse(), MesEffets[2].VitesseDeleffet);//chromatic
            _Vignette.intensity.value = CheckParamettreFX(_Chromatic.intensity.value, MesEffets[6].LesValuesMax[0] * pourcentageVitesse(), MesEffets[6].VitesseDeleffet);//vignette
           _Lens.intensity.value = CheckParamettreFX(_Lens.intensity.value, MesEffets[6].LesValuesMax[0] * pourcentageVitesse(), MesEffets[6].VitesseDeleffet);////lens
        }
        else if (GG.GB.Surchauffing)//si le joueur surchauffe
        {
            _bloom.intensity.value = MesEffets[0].LesValuesMax[2];//bloom
            _Chromatic.intensity.value = MesEffets[2].LesValuesMax[2];//chroma
            _Vignette.intensity.value = MesEffets[3].LesValuesMax[2];//vignette
            _Lens.intensity.value = MesEffets[6].LesValuesMax[2];//Lens
           
        }
        
    }
    float pourcentageVitesse() 
    {
        float Pourcentage = 0;
        if (GG.GMC.VitesseMoto != 0)
        {
            Pourcentage = Mathf.Abs(GG.GMC.VitesseMoto) / GG.GMC.vitesseMax;
        }
        return Pourcentage;
    } //récupére le pourcentage vitesse/vitessemax
    float CheckParamettreFX(float actuel,float max,float speed) //checks les paramétres post effect
    {
        if (actuel>max)
        {
            actuel = Mathf.Lerp(actuel, max, speed * Time.deltaTime * vitesseretourMultiplicateur) ; //si l"effet est en dehors
        }
        else 
        {
            actuel = Mathf.Lerp(actuel, max, speed * Time.deltaTime );
        }
        return actuel;
    }
    float directionDuStarff()
    {
        float direction = 0;
        float val = Input.GetAxis(GG.GDI.Axes[5]);//récuppére l'input
        if (val != 0)
        {
            direction = Mathf.Abs(val) / val;
        }
        return direction;
    }// détermine la direction du straff
    float directionDeRotation() 
    {
        float direction = 0;
        float val = GG.GMC.ActuelVitesseRotation;
        if (val!=0)
        {
            direction = Mathf.Abs(val) / val;
        }
        return direction;
    }//récupére la direction de la rotation
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
    }//set les arrays des bools
    bool[] resetDeBool (bool[] toReset)
    {
        for (int i = 0; i < toReset.Length; i++)
        {
            toReset[i] = false;
        }
        return toReset;
    }//reset tous les bools a false
    void setPostProcess()
    {
        foreach (PostFXEffet Effect in MesEffets)
        {
            if (Effect.MesPostProcessEffect == PostFXEffet.PostProcessEffect.AmbientOcclusions)
            {
                 monPostProcess.profile.TryGetSettings(out _AmbientOclu);
            }
            else if (Effect.MesPostProcessEffect == PostFXEffet.PostProcessEffect.AutoExposure)
            {
                monPostProcess.profile.TryGetSettings(out _AutoExpo);
            }
            else if (Effect.MesPostProcessEffect == PostFXEffet.PostProcessEffect.Bloom)
            {
                 monPostProcess.profile.TryGetSettings(out _bloom);
                 bloomValueDepart =_bloom.intensity.value;
            }
            else if (Effect.MesPostProcessEffect == PostFXEffet.PostProcessEffect.ChromaticAbberration)
            {
                monPostProcess.profile.TryGetSettings(out _Chromatic) ;
                ChromaticValueDepart = _Chromatic.intensity.value;
            }
            else if (Effect.MesPostProcessEffect == PostFXEffet.PostProcessEffect.ColorGrading)
            {
                monPostProcess.profile.TryGetSettings(out _ColorGrad);
            }
            else if (Effect.MesPostProcessEffect == PostFXEffet.PostProcessEffect.DepthOfField)
            {
                monPostProcess.profile.TryGetSettings(out _DepthOf);
            }
            else if (Effect.MesPostProcessEffect == PostFXEffet.PostProcessEffect.Grain)
            {
                monPostProcess.profile.TryGetSettings(out _Grain);
            }
            else if (Effect.MesPostProcessEffect == PostFXEffet.PostProcessEffect.LensDistortion)
            {
                monPostProcess.profile.TryGetSettings(out _Lens) ;
                LensDistortionValueDepart = _Lens.intensity.value;
            }
            else if (Effect.MesPostProcessEffect == PostFXEffet.PostProcessEffect.MotionBlur)
            {
                monPostProcess.profile.TryGetSettings(out _MotionBlur) ;
            }
            else if (Effect.MesPostProcessEffect == PostFXEffet.PostProcessEffect.ScreenSpaceRefecltion)
            {
                monPostProcess.profile.TryGetSettings(out _ScreenSpace) ;
            }
            else if (Effect.MesPostProcessEffect == PostFXEffet.PostProcessEffect.Vignette)
            {
                monPostProcess.profile.TryGetSettings(out _Vignette) ;
            }
        }

    }//set les post process selon ceux de mis
}


