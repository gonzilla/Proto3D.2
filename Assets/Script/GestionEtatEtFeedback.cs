using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;



[System.Serializable]
public class SoundInfo
{
    [FMODUnity.EventRef]
    public string LeSonAJouer;
    public string LeVoid;
    public FMOD.Studio.EventInstance MonEvenementFMOD;
    
    //public FMOD.Studio. MonParamettre;
    public bool BoucleDansFmod;
    public bool BoucleParCode;
    public bool event3D;
    public bool OneShot;
    public bool StopQuandChangementDetat;
    
    public float TempsDuSons;
    public enum ParametreUtiliser 
    {
        vitessemoto,
        AccelInput,
        DriftInput,
        FreinInput,
        BoostActivation,
        OnPlaque, 
        Distance

    };
    public ParametreUtiliser[] ListeDesParametre;
    [HideInInspector] public string[] ParameterToModify;
    [HideInInspector] public int IndexDuSon;
    [HideInInspector] public float tempsPourArret;
    public int NombreDePlay = 0;

}

public class GestionEtatEtFeedback : PersonnalMethod
{
    //Public variable

    /// <summary>
    /// les paramétre vont de 0 à 1
    /// il n'y a que le sons de moteur qui boucle dans fmod et peut être le drift 
    /// </summary>

    public enum MotoActualState
    {

        Avance,
        Recule,
        Tourne,
        Derape,
        Surchauffe,
        Freine,
        Ralenti,
        Stationnaire,
        Straff,
        BoostLV1,
        BoostLV2,
        BoostLV3,
        RecolteDeBoost

    };
    public MotoActualState StateOfMoto;
    
    public Text AffichageEtatDebug;
    public List<SoundInfo> LesInfosDuSon;

    public GameObject Moto;
    //Local variable

     List<SoundInfo> SonAvecParameter = new List<SoundInfo>();
     List<SoundInfo> SonEntrainDeJouer = new List<SoundInfo>();
    
    public string EtatActuel;
    
    GestionGeneral GG;//stock les script
    
    void Start()
    {
        GetGestion(out GG, Moto);// Récupére les script necessaire
        SetAllSound();// set les infos du son
        
    }

    public void changementDetat(MotoActualState etat)
    {
        MotoActualState previousState = StateOfMoto;
        StateOfMoto = etat;// lui dit le nouvelle etat
        bool Check = false;
        if (previousState!=StateOfMoto)
        {
            //print("se passe un truc invisible"+StateOfMoto);
            Check = true;
        }
        GereLeBonsonSaMere();
        if (Check)
        {
            CheckSiDoitSarreter();// a test à d'autre endroit
        }
       


    }

    void Update()
    {
        UpdateSoundWithParamater();//update les sons avec les variables
    }

    void GereLeBonsonSaMere() // compare les états de la moto pour lancer le bon son
    {
        if (StateOfMoto == MotoActualState.Stationnaire)
        {
            Stationnaire();
            EtatActuel = "Stationnaire";
            AffichageEtatDebug.text = EtatActuel;
        }
        else if (StateOfMoto == MotoActualState.Avance)
        {
            EtatActuel = "Avance";
            AffichageEtatDebug.text = EtatActuel;
            Avance();
           
        }
        else if (StateOfMoto == MotoActualState.Derape)
        {
            EtatActuel = "Derape";
            AffichageEtatDebug.text = EtatActuel;
            Derape();
          
        }
        else if (StateOfMoto == MotoActualState.Freine)
        {
            EtatActuel = "Freine";
            AffichageEtatDebug.text = EtatActuel;
            Freine();
            
        }
        else if (StateOfMoto == MotoActualState.Ralenti)
        {
            EtatActuel = "Ralenti";
            AffichageEtatDebug.text = EtatActuel;
            Ralenti();
           
        }
        else if (StateOfMoto == MotoActualState.Recule)
        {
            EtatActuel = "Recule";
            AffichageEtatDebug.text = EtatActuel;
            Recule();
            
        }
        else if (StateOfMoto == MotoActualState.Surchauffe)
        {
            EtatActuel = "Surchauffe";
            AffichageEtatDebug.text = EtatActuel;
            Surchauffe();
           
        }
        else if (StateOfMoto == MotoActualState.Tourne)
        {
            EtatActuel = "Tourne";
            AffichageEtatDebug.text = EtatActuel;
            Tourne();
           
        }
        else if (StateOfMoto == MotoActualState.Straff)
        {
            EtatActuel = "Straff";
            AffichageEtatDebug.text = EtatActuel;
            Straff();
           
        }
        else if (StateOfMoto == MotoActualState.BoostLV1)
        {
            EtatActuel = "BoostLV1";
            AffichageEtatDebug.text = EtatActuel;
            BoostLV1();
           
        }
        else if (StateOfMoto == MotoActualState.BoostLV2)
        {
            EtatActuel = "BoostLV2";
            AffichageEtatDebug.text = EtatActuel;
            BoostLV2();
          
        }
        else if (StateOfMoto == MotoActualState.BoostLV3)
        {
            EtatActuel = "BoostLV3";
            AffichageEtatDebug.text = EtatActuel;
            BoostLV3();
            
        }
        else if (StateOfMoto == MotoActualState.RecolteDeBoost)
        {

            EtatActuel = "RecolteDeBoost";
            AffichageEtatDebug.text = EtatActuel;
            RecolteDeBoost();

        }
    }
    #region voidPourEtat
    void Stationnaire()
    {
        
        int[] IndexDusons = new int[0]; // stock les int des sons à lancé à l'état
        setInfoSurSonByVoid("Stationnaire", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    void Avance()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Avance", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    void BoostLV1()
    {
        
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("BoostLV1", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    void BoostLV2()
    {

        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("BoostLV2", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    void BoostLV3()
    {

        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("BoostLV3", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    void Derape()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Derape", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    void Freine()
    {


        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Freine", out IndexDusons);
        LanceLeSon(IndexDusons);



    }
    void Ralenti()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Ralenti", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    void Recule()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Recule", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    void Surchauffe()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Surchauffe", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    void Tourne()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Tourne", out IndexDusons);
        LanceLeSon(IndexDusons);

    }
    void Straff()
    {
        
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Straff", out IndexDusons);
        
        LanceLeSon(IndexDusons);

    }
    void RecolteDeBoost()
    {

        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("RecolteDeBoost", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    #endregion
    void LanceLeSon(int[] Lesindex)
    {
        
        foreach (int index in Lesindex)//
        {
            if (!LesInfosDuSon[index].BoucleDansFmod)// si le boucle pas dans Fmod
            {
                if (LesInfosDuSon[index].OneShot && LesInfosDuSon[index].NombreDePlay==0)// si c'est un oneshot 
                {
                   
                    if (LesInfosDuSon[index].event3D)// 3D
                    {
                        LesInfosDuSon[index].NombreDePlay++;
                        
                        FMODUnity.RuntimeManager.PlayOneShot(LesInfosDuSon[index].LeSonAJouer, Moto.transform.position);// joue le sons
                        
                       
                    }
                    else
                    {
                        FMODUnity.RuntimeManager.PlayOneShot(LesInfosDuSon[index].LeSonAJouer); // joue le sons
                    }
                }
                else // si ce n'est pas un oneshot
                {
                    if (LesInfosDuSon[index].BoucleParCode)// s'il  boucle  par code
                    {
                        LesInfosDuSon[index].MonEvenementFMOD.start();//start l'event
                        SonEntrainDeJouer.Add(LesInfosDuSon[index]);

                    }
                    
                    float tempsPourArretCeSon = Time.time + LesInfosDuSon[index].TempsDuSons;//le temps d'arret du sons
                    LesInfosDuSon[index].tempsPourArret = tempsPourArretCeSon; // set le temps d'arret du sons 
                    Invoke("soundsToStop", tempsPourArretCeSon);//lance le son a arrété dans X seconde
                }
            }
            else // s'il boucle
            {
                
                FMOD.Studio.PLAYBACK_STATE state;//créer la variable
                LesInfosDuSon[index].MonEvenementFMOD.getPlaybackState(out state);//recupérer les infos
                if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)//si le son ne joue pas
                {
                    
                    LesInfosDuSon[index].MonEvenementFMOD.start();//lance le sons
                    int nombreDeFoisLeSon = 0;
                    foreach (SoundInfo item in SonEntrainDeJouer)
                    {
                        if (item == LesInfosDuSon[index])
                        {
                            nombreDeFoisLeSon++;
                        }
                    }
                    if (nombreDeFoisLeSon==0)
                    {
                        SonEntrainDeJouer.Add(LesInfosDuSon[index]);
                       
                    }
                    
                }
            }


        }
       

    }
    void setInfoSurSonByVoid(string LeVoid, out int[] IndexSon)// gére le sons lorsque je change d'état
    {
        List<int> IndexDunSon = new List<int>();// stock une list des ints
        for (int i = 0; i < LesInfosDuSon.Count; i++) // regarde dans tous les sons 
        {
            if (LesInfosDuSon[i].LeVoid == LeVoid)//si le void de lancement correspond a letat 
            {
                IndexDunSon.Add(i);// ajoute l'index à la list
                LesInfosDuSon[i].IndexDuSon = i; // set l'index du son
                
            }

        }

        IndexSon = IndexDunSon.ToArray();// set la list en array

    }
    void UpdateSoundWithParamater()
    {
       
        foreach (SoundInfo Info in SonEntrainDeJouer)//pour chaque sons avec des paramètres
        {
            if (Info.ParameterToModify.Length > 0)//s'il y des param
            {
                
                foreach (string Parameter in Info.ParameterToModify)
                {
                    if (Info.event3D)
                    {
                        Info.MonEvenementFMOD.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Moto.gameObject));// si l'event est 3D set son attribute
                    }
                    Info.MonEvenementFMOD.setParameterByName(Parameter, LeBonParametre(Parameter));// vas chercher le bon param et le mettre à la bonne value 
                }
            }
        }

    }
    void SetAllSound()
    {
        foreach (SoundInfo Info in LesInfosDuSon) // pour chaque 
        {
            
            Info.MonEvenementFMOD = FMODUnity.RuntimeManager.CreateInstance(Info.LeSonAJouer);// set Event Fmod
            setParameter(Info); // set les paramètre necessaire
            if (Info.event3D)
            {
                Info.MonEvenementFMOD.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Moto.gameObject));// si l'event est 3D set son attribute
            }
            if (Info.ParameterToModify.Length > 0)// si le sons a des paramètres
            {
                SonAvecParameter.Add(Info);//ajoute Info dans la liste des sons avec paramètre
            }
           
            
        }
        
    }// Bon
    void soundsToStop()
    {
        foreach (SoundInfo Info in SonEntrainDeJouer)// pour chaque sons entrain d'être jouer
        {
            if (Info.tempsPourArret >= Time.time) // si leurs temps d'arret sont dépassé
            {
                Info.tempsPourArret = 0; //leur remets a 0
                Info.MonEvenementFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);//stop le son
                SonEntrainDeJouer.Remove(Info);
            }
        }
    }
    void StopASound(int IndexDuSonAStop) 
    {
        SonEntrainDeJouer.RemoveAt(IndexDuSonAStop);
        foreach (SoundInfo Info in LesInfosDuSon)
        {
            if (Info.IndexDuSon == IndexDuSonAStop)
            {
                Info.MonEvenementFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
    }
    void setParameter(SoundInfo InfoDuSon) 
    {
        int indexer = 0;
        if (InfoDuSon.ListeDesParametre.Length>0)
        {
            InfoDuSon.ParameterToModify = new string[InfoDuSon.ListeDesParametre.Length];// je refais le tableau des variables
        }
       
        foreach (SoundInfo.ParametreUtiliser Param in InfoDuSon.ListeDesParametre)// pour chaque param utiliser
        {

            if (Param == SoundInfo.ParametreUtiliser.vitessemoto)
            {
                InfoDuSon.ParameterToModify[indexer] = "vitessemoto";
            }
            else if (Param == SoundInfo.ParametreUtiliser.AccelInput)
            {
                InfoDuSon.ParameterToModify[indexer] = "AccelInput";
            }
            else if (Param == SoundInfo.ParametreUtiliser.DriftInput)
            {
                InfoDuSon.ParameterToModify[indexer] = "DriftInput";
            }
            else if (Param == SoundInfo.ParametreUtiliser.FreinInput)
            {
                InfoDuSon.ParameterToModify[indexer] = "FreinInput";
            }
            else if (Param == SoundInfo.ParametreUtiliser.BoostActivation)
            {
                InfoDuSon.ParameterToModify[indexer] = "BoostActivation";
            }
            else if (Param == SoundInfo.ParametreUtiliser.OnPlaque)
            {
                InfoDuSon.ParameterToModify[indexer] = "OnPlaque";
            }
            else if (Param == SoundInfo.ParametreUtiliser.Distance)
            {
                InfoDuSon.ParameterToModify[indexer] = "Distance";
            }
            indexer++;
        }
    }

     void ResetOneShot(SoundInfo Info)
    {
       
            if (Info.NombreDePlay!=0)
            {
                Info.NombreDePlay = 0;
            }
        
    }

    void CheckSiDoitSarreter() //SoundInfo SonsACheck
    {
        //print("Je check");
        foreach (SoundInfo Info in LesInfosDuSon)
        {
            if (Info.StopQuandChangementDetat && EtatActuel != Info.LeVoid)
            {
                //print("j'ai changé d'état et le void n'est pas le bon");
                Info.MonEvenementFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                SonEntrainDeJouer.Remove(Info);
                
                
            }
            if (EtatActuel!= Info.LeVoid)
            {
                Info.NombreDePlay = 0;
            }
            

        }
       

        
    }

    void getParamaterByName()
    {

    }

    float LeBonParametre(string LeparametreAchercher) // faire des trucs
    {
        float ValueToReturn = 0;
        if (LeparametreAchercher == "vitessemoto")
        {
            float actuel = 0;
            if (Mathf.Abs(GG.GMC.VitesseMoto) > 0)
            {
                actuel = GG.GMC.VitesseMoto;
            }
            float Max = GG.GMC.vitesseMax;
            ValueToReturn = Mathf.Abs(actuel) / Max;
        }

        else if (LeparametreAchercher == "AccelInput")
        {
            float actuel = 0;
            if (Mathf.Abs(Input.GetAxis(GG.GDI.Axes[1])) > 0)
            {
                actuel = Input.GetAxis(GG.GDI.Axes[1]);
            }
            float Max = 1;

            ValueToReturn = Mathf.Abs(actuel) / Max;
        }
        else if (LeparametreAchercher == "DriftInput")
        {
            float actuel = 0;
            if (Mathf.Abs(Input.GetAxis(GG.GDI.Axes[7])) > 0)
            {
                actuel = Input.GetAxis(GG.GDI.Axes[7]);
            }
            float Max = 1;

            ValueToReturn = Mathf.Abs(actuel) / Max;
        }
        else if (LeparametreAchercher == "FreinInput")
        {
            float actuel = 0;
            float max = 1;
            float valueInput = 0;
            if (Input.GetAxis(GG.GDI.Axes[1]) != 0)
            {
                valueInput = Mathf.Abs(Input.GetAxis(GG.GDI.Axes[1])) / Input.GetAxis(GG.GDI.Axes[1]);
            }

            if (GG.GMC.VitesseMoto > 0 && valueInput == -1)
            {
                actuel = Mathf.Abs(Input.GetAxis(GG.GDI.Axes[1]));
            }
            else if (GG.GMC.VitesseMoto < 0 && valueInput == 1)
            {
                actuel = Input.GetAxis(GG.GDI.Axes[1]);
            }
            print(actuel + "actuel");
            ValueToReturn = actuel / max;


        }
        else if (LeparametreAchercher == "OnPlaque")
        {
            if (GG.GB.Recharge)
            {
                ValueToReturn = 1;
            }
            else 
            {
                ValueToReturn = 0;
            }
                
            
        }
        else if (LeparametreAchercher == "BoostActivation")
        {
            if (GG.GB.boosting)
            {
                ValueToReturn = 1;
            }
            else 
            {
                ValueToReturn = 0;
            }
            
        }

        return ValueToReturn;
    }

    public void stopAllSound() 
    {
        foreach (SoundInfo GoStop in SonEntrainDeJouer)
        {
            GoStop.MonEvenementFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    
    }
}
// float lavitessedemamotoSuperchouette = 0;
// lavitessedemamotoSuperchouette = (float)GMC.GetType().GetField("VitesseMoto").GetValue(GMC);
/* else if (LeparametreAchercher == "VitesseRotation")
        {
            float actuel = 0;
            if (Mathf.Abs(GG.GMC.ActuelVitesseRotation) > 0)
            {
                actuel = GG.GMC.ActuelVitesseRotation;
            }
            float Max = GG.GMC.VitesseDeRotationMax;

            ValueToReturn = Mathf.Abs(actuel) / Max;
        }*/
/*else//!\\ Problème logique
                    {
                        if (Time.time > LesInfosDuSon[index].tempsPourArret)
                        {
                            LesInfosDuSon[index].MonEvenementFMOD.start();
                        }
                    }*/
/*for (int i = 0; i < InfoDuSon.ListeDesParametre.Length; i++)
       {
           if (InfoDuSon.ListeDesParametre[i] == SoundInfo.ParametreUtiliser.vitessemoto)
           {
               InfoDuSon.ParameterToModify[indexer] = "vitessemoto";
           }
           else if (InfoDuSon.ListeDesParametre[i] == SoundInfo.ParametreUtiliser.AccelInput)
           {
               InfoDuSon.ParameterToModify[indexer] = "AccelInput";
           }
       }*/