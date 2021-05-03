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
        VitesseDeRotation

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
        Stationnaire

    };
    public MotoActualState StateOfMoto;
    
    public Text AffichageEtatDebug;
    public List<SoundInfo> LesInfosDuSon;
    //Local variable

    List<SoundInfo> SonAvecParameter = new List<SoundInfo>();
    List<SoundInfo> SonEntrainDeJouer = new List<SoundInfo>();

    GestionGeneral GG;//stock les script
    void Start()
    {
        GetGestion(out GG, this.gameObject);
        SetAllSound();
        
    }

    public void changementDetat(MotoActualState etat)
    {

        StateOfMoto = etat;
        GereLeBonsonSaMere();

    }

    void Update()
    {
        UpdateSoundWithParamater();
    }

    void GereLeBonsonSaMere()
    {
        if (StateOfMoto == MotoActualState.Stationnaire)
        {
            Stationnaire();
            AffichageEtatDebug.text = "Stationnaire";
        }
        else if (StateOfMoto == MotoActualState.Avance)
        {
            Avance();
            AffichageEtatDebug.text = "Avance";
        }
        else if (StateOfMoto == MotoActualState.Boost)
        {
            Boost();
            AffichageEtatDebug.text = "Boost";
        }
        else if (StateOfMoto == MotoActualState.Derape)
        {
            Derape();
            AffichageEtatDebug.text = "Derape";
        }
        else if (StateOfMoto == MotoActualState.Freine)
        {
            Freine();
            AffichageEtatDebug.text = "Freine";
        }
        else if (StateOfMoto == MotoActualState.Ralenti)
        {
            Ralenti();
            AffichageEtatDebug.text = "Ralenti";
        }
        else if (StateOfMoto == MotoActualState.Recule)
        {
            Recule();
            AffichageEtatDebug.text = "Recule";
        }
        else if (StateOfMoto == MotoActualState.Surchauffe)
        {
            Surchauffe();
            AffichageEtatDebug.text = "Surchauffe";
        }
        else if (StateOfMoto == MotoActualState.Tourne)
        {
            Tourne();
            AffichageEtatDebug.text = "Tourne";
        }

    }
    public void Stationnaire()
    {
        int[] IndexDusons = new int[0];
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
    void LanceLeSon(int[] Lesindex)
    {
        foreach (int index in Lesindex)
        {
            if (!LesInfosDuSon[index].BoucleDansFmod)
            {
                if (LesInfosDuSon[index].OneShot)
                {
                    if (LesInfosDuSon[index].event3D)
                    {
                        FMODUnity.RuntimeManager.PlayOneShot(LesInfosDuSon[index].LeSonAJouer, transform.position);
                    }
                    else
                    {
                        FMODUnity.RuntimeManager.PlayOneShot(LesInfosDuSon[index].LeSonAJouer);
                    }
                }
                else
                {
                    if (!LesInfosDuSon[index].BoucleParCode)
                    {
                        LesInfosDuSon[index].MonEvenementFMOD.start();

                    }
                    else
                    {
                        if (Time.time > LesInfosDuSon[index].tempsPourArret)
                        {
                            LesInfosDuSon[index].MonEvenementFMOD.start();
                        }
                    }
                    float tempsPourArretCeSon = Time.time + LesInfosDuSon[index].TempsDuSons;
                    LesInfosDuSon[index].tempsPourArret = tempsPourArretCeSon;
                    Invoke("soundsToStop", tempsPourArretCeSon);
                }
            }


        }


    }
    void setInfoSurSonByVoid(string LeVoid, out int[] IndexSon)
    {
        List<int> IndexDunSon = new List<int>();
        for (int i = 0; i < LesInfosDuSon.Count; i++)
        {
            if (LesInfosDuSon[i].LeVoid == LeVoid)
            {
                IndexDunSon.Add(i);
                LesInfosDuSon[i].IndexDuSon = i;

            }

        }

        IndexSon = IndexDunSon.ToArray();

    }
    void UpdateSoundWithParamater()
    {
        foreach (SoundInfo Info in SonEntrainDeJouer)
        {
            if (Info.ParameterToModify.Length > 0)
            {
                foreach (string Parameter in Info.ParameterToModify)
                {
                    Info.MonEvenementFMOD.setParameterByName("Parameter", LeBonParametre());
                }
            }
        }

    }
    void SetAllSound()
    {
        foreach (SoundInfo Info in LesInfosDuSon)
        {

            Info.MonEvenementFMOD = FMODUnity.RuntimeManager.CreateInstance(Info.LeSonAJouer);
            if (Info.event3D)
            {
                Info.MonEvenementFMOD.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
            }
            if (Info.ParameterToModify.Length > 0)
            {
                SonAvecParameter.Add(Info);
            }
            setParameter(Info);
        }
    }
    void soundsToStop()
    {
        foreach (SoundInfo Info in LesInfosDuSon)
        {
            if (Info.tempsPourArret >= Time.time)
            {
                Info.tempsPourArret = 0;
                Info.MonEvenementFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
            InfoDuSon.ParameterToModify = new string[InfoDuSon.ListeDesParametre.Length];
        }
        foreach (SoundInfo.ParametreUtiliser Param in InfoDuSon.ListeDesParametre)
        {

            if (Param == SoundInfo.ParametreUtiliser.vitesseDeMoto)
            {
                InfoDuSon.ParameterToModify[indexer] = "VitesseMoto";
            }
            else if (Param == SoundInfo.ParametreUtiliser.VitesseDeRotation)
            {
                InfoDuSon.ParameterToModify[indexer] = "VitesseRotation";
            }
        }
    }
   

    float LeBonParametre() // faire des trucs
    {
        // float lavitessedemamotoSuperchouette = 0;
        // lavitessedemamotoSuperchouette = (float)GMC.GetType().GetField("VitesseMoto").GetValue(GMC);
        return 0;
    }
}

