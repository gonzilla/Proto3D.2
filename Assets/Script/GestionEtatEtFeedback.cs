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
        vitesseDeMoto,
        VitesseDeRotation,
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
        Boost,
        Surchauffe,
        Freine,
        Ralenti,
        Stationnaire,
        Straff

    };
    public MotoActualState StateOfMoto;
    
    public Text AffichageEtatDebug;
    public List<SoundInfo> LesInfosDuSon;

    public GameObject Moto;
    //Local variable

    List<SoundInfo> SonAvecParameter = new List<SoundInfo>();
    List<SoundInfo> SonEntrainDeJouer = new List<SoundInfo>();

    GestionGeneral GG;//stock les script
    void Start()
    {
        GetGestion(out GG, Moto);// Récupére les script necessaire
        SetAllSound();// set les infos du son
        
    }

    public void changementDetat(MotoActualState etat)
    {

        StateOfMoto = etat;
        GereLeBonsonSaMere();

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
            AffichageEtatDebug.text = "Stationnaire";
        }
        else if (StateOfMoto == MotoActualState.Avance)
        {
            // Avance();
            
            AffichageEtatDebug.text = "Avance";
        }
        else if (StateOfMoto == MotoActualState.Boost)
        {
            Boost();
            AffichageEtatDebug.text = "Boost";
        }
        else if (StateOfMoto == MotoActualState.Derape)
        {
           // Derape();
            AffichageEtatDebug.text = "Derape";
        }
        else if (StateOfMoto == MotoActualState.Freine)
        {
           // Freine();
            AffichageEtatDebug.text = "Freine";
        }
        else if (StateOfMoto == MotoActualState.Ralenti)
        {
           // Ralenti();
            AffichageEtatDebug.text = "Ralenti";
        }
        else if (StateOfMoto == MotoActualState.Recule)
        {
           // Recule();
            AffichageEtatDebug.text = "Recule";
        }
        else if (StateOfMoto == MotoActualState.Surchauffe)
        {
           // Surchauffe();
            AffichageEtatDebug.text = "Surchauffe";
        }
        else if (StateOfMoto == MotoActualState.Tourne)
        {
           // Tourne();
            AffichageEtatDebug.text = "Tourne";
        }
        else if (StateOfMoto == MotoActualState.Straff)
        {
           // Straff();
            AffichageEtatDebug.text = "Straff";
        }
    }
    public void Stationnaire()
    {
        print("stationnaire");
        int[] IndexDusons = new int[0]; // stock les int des sons à lancé à l'état
        setInfoSurSonByVoid("Stationnaire", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    public void Avance()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Avance", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    public void Boost()
    {
        print("boost");
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Boost", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    public void Derape()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Derape", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    public void Freine()
    {


        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Freine", out IndexDusons);
        LanceLeSon(IndexDusons);



    }
    public void Ralenti()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Ralenti", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    public void Recule()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Recule", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    public void Surchauffe()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Surchauffe", out IndexDusons);
        LanceLeSon(IndexDusons);
    }
    public void Tourne()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Tourne", out IndexDusons);
        LanceLeSon(IndexDusons);

    }
    public void Straff()
    {
        int[] IndexDusons = new int[0];
        setInfoSurSonByVoid("Straff", out IndexDusons);
        LanceLeSon(IndexDusons);

    }
    void LanceLeSon(int[] Lesindex)
    {
        print("lance le son");
        foreach (int index in Lesindex)//
        {
            if (!LesInfosDuSon[index].BoucleDansFmod)// si le boucle pas dans Fmod
            {
                if (LesInfosDuSon[index].OneShot)// si c'est un oneshot 
                {
                    print("one shot");
                    if (LesInfosDuSon[index].event3D)// 3D
                    {
                        print("event 3D");
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

                    }
                    /*else//!\\ Problème logique
                    {
                        if (Time.time > LesInfosDuSon[index].tempsPourArret)
                        {
                            LesInfosDuSon[index].MonEvenementFMOD.start();
                        }
                    }*/
                    float tempsPourArretCeSon = Time.time + LesInfosDuSon[index].TempsDuSons;//le temps d'arret du sons
                    LesInfosDuSon[index].tempsPourArret = tempsPourArretCeSon; // set le temps d'arret du sons 
                    Invoke("soundsToStop", tempsPourArretCeSon);//lance le son a arrété dans X seconde
                }
            }
            else // s'il boucle
            {
                print(" lance le sons qui boucle FMOD");
                FMOD.Studio.PLAYBACK_STATE state;//créer la variable
                LesInfosDuSon[index].MonEvenementFMOD.getPlaybackState(out state);//recupérer les infos
                if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)//si le son ne joue pas
                {
                    print("je lance le sons qui boucle FMOD");
                    LesInfosDuSon[index].MonEvenementFMOD.start();//lance le sons
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
                print(Info.ParameterToModify.Length);
                foreach (string Parameter in Info.ParameterToModify)
                {
                    Info.MonEvenementFMOD.setParameterByName("Parameter", LeBonParametre("Parameter"));// vas chercher le bon param et le mettre à la bonne value 
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
                Info.MonEvenementFMOD.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));// si l'event est 3D set son attribute
            }
            if (Info.ParameterToModify.Length > 0)// si le sons a des paramètres
            {
                SonAvecParameter.Add(Info);//ajoute Info dans la liste des sons avec paramètre
            }
            
        }
        print("j'ai trouvé" + SonAvecParameter.Count);
    }
    void soundsToStop()
    {
        foreach (SoundInfo Info in LesInfosDuSon)// pour chaque sons
        {
            if (Info.tempsPourArret >= Time.time) // si leurs temps d'arret sont dépassé
            {
                Info.tempsPourArret = 0; //leur remets a 0
                Info.MonEvenementFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);//stop le son
            }
        }
    }

    void getParamaterByName() 
    {
       
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

            if (Param == SoundInfo.ParametreUtiliser.vitesseDeMoto)
            {
                InfoDuSon.ParameterToModify[indexer] = "VitesseMoto";
            }
            else if (Param == SoundInfo.ParametreUtiliser.VitesseDeRotation)
            {
                InfoDuSon.ParameterToModify[indexer] = "VitesseRotation";
            }
            else if (Param == SoundInfo.ParametreUtiliser.AccelInput)
            {
                InfoDuSon.ParameterToModify[indexer] = "AccelInput";
            }
        }
    }
   

    float LeBonParametre(string LeparametreAchercher) // faire des trucs
    {
        float ValueToReturn=0;
        if (LeparametreAchercher == "VitesseMoto")
        {
            float actuel = 0;
            if (Mathf.Abs(GG.GMC.VitesseMoto)>0)
            {
                actuel = GG.GMC.VitesseMoto;
            }
            float Max = GG.GMC.vitesseMax;
            ValueToReturn = Mathf.Abs(actuel) / Max;
        }
        else if (LeparametreAchercher == "VitesseRotation")
        {
            float actuel = 0;
            if (Mathf.Abs(GG.GMC.ActuelVitesseRotation) > 0)
            {
                actuel = GG.GMC.ActuelVitesseRotation;
            }
            float Max = GG.GMC.VitesseDeRotationMax;

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



        return ValueToReturn;
    }
}
// float lavitessedemamotoSuperchouette = 0;
// lavitessedemamotoSuperchouette = (float)GMC.GetType().GetField("VitesseMoto").GetValue(GMC);
