using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[System.Serializable]
public class SoundInfo
{
    [FMODUnity.EventRef]
    public string LeSonAJouer;
    public UnityEvent LaMethode;
    public string LeVoid;
    public FMOD.Studio.EventInstance MonEvenementFMOD;
    //public FMOD.Studio. MonParamettre;
    public bool BoucleDansFmod;
    public bool BoucleParCode;
    public bool event3D;
    public bool OneShot;
    public bool StopQuandChangementDetat;
    public float TempsDuSons;
    public string[] ParameterToModify;
    [HideInInspector] public int IndexDuSon;
    [HideInInspector]public float tempsPourArret;
}

public class GestionEtatEtFeedback : PersonnalMethod
{
    //Public variable
    public List<SoundInfo> LesInfosDuSon;
   

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


    //Local variable

    List<SoundInfo> SonAvecParameter = new List<SoundInfo>();
    List<SoundInfo> SonEntrainDeJouer = new List<SoundInfo>();
    void Start()
    {
        SetAllSound();
        //changementDetat(MotoActualState.Stationnaire);
        //int nombreEvent = LesInfosDuSon.Count;

    }

    public void changementDetat(MotoActualState etat)
    {

        StateOfMoto = etat;
        GereLeBonsonSaMere();

    }

     void Update()
    {
        
    }

    void GereLeBonsonSaMere() 
    {
        if (StateOfMoto == MotoActualState.Stationnaire)
        {
            Stationnaire();
        }
        else if (StateOfMoto == MotoActualState.Avance)
        {
            Avance();
        }
        else if (StateOfMoto == MotoActualState.Boost)
        {
            Boost();
        }
        else if (StateOfMoto == MotoActualState.Derape)
        {
            Derape();
        }
        else if (StateOfMoto == MotoActualState.Freine)
        {
            Freine();
        }
        else if (StateOfMoto == MotoActualState.Ralenti)
        {
            Ralenti();
        }
        else if (StateOfMoto == MotoActualState.Recule)
        {
            Recule();
        }
        else if (StateOfMoto == MotoActualState.Surchauffe)
        {
            Surchauffe();
        }
        else if (StateOfMoto == MotoActualState.Tourne)
        {
            Tourne();
        }

    }
    public void Stationnaire() 
    {
        int IndexDusons = 0;
        setInfoSurSonByVoid("Stationnaire", out IndexDusons);
        if (!LesInfosDuSon[IndexDusons].BoucleDansFmod)
        {
            LanceLeSon(IndexDusons);
        }
    }
    public void Avance()
    {
        int IndexDusons = 0;
        setInfoSurSonByVoid("Avance", out IndexDusons);
        if (!LesInfosDuSon[IndexDusons].BoucleDansFmod)
        {
            LanceLeSon(IndexDusons);
        }
    }
    public void Boost()
    {
        int IndexDusons = 0;
        setInfoSurSonByVoid("Boost", out IndexDusons);
        if (!LesInfosDuSon[IndexDusons].BoucleDansFmod)
        {
            LanceLeSon(IndexDusons);
        }
    }
    public void Derape() 
    {
        int IndexDusons = 0;
        setInfoSurSonByVoid("Derape", out IndexDusons);
        if (!LesInfosDuSon[IndexDusons].BoucleDansFmod)
        {
            LanceLeSon(IndexDusons);
        }
    }
    public void Freine()
    {

        
        int IndexDusons = 0;
        setInfoSurSonByVoid("Freine", out IndexDusons);
        if (!LesInfosDuSon[IndexDusons].BoucleDansFmod)
        {
            LanceLeSon(IndexDusons);
        }
        


    }

    public void Ralenti() 
    {
        int IndexDusons = 0;
        setInfoSurSonByVoid("Ralenti", out IndexDusons);
        if (!LesInfosDuSon[IndexDusons].BoucleDansFmod)
        {
            LanceLeSon(IndexDusons);
        }
    }

    public void Recule() 
    {
        int IndexDusons = 0;
        setInfoSurSonByVoid("Recule", out IndexDusons);
        if (!LesInfosDuSon[IndexDusons].BoucleDansFmod)
        {
            LanceLeSon(IndexDusons);
        }
    }

    public void Surchauffe() 
    {
        int IndexDusons = 0;
        setInfoSurSonByVoid("Surchauffe", out IndexDusons);
        if (!LesInfosDuSon[IndexDusons].BoucleDansFmod)
        {
            LanceLeSon(IndexDusons);
        }
    }
    public void Tourne()
    {
        int IndexDusons = 0;
        setInfoSurSonByVoid("Tourne", out IndexDusons);
        if (!LesInfosDuSon[IndexDusons].BoucleDansFmod)
        {
            LanceLeSon(IndexDusons);
        }
    }
    void LanceLeSon(int index) 
    {
        
        if (LesInfosDuSon[index].OneShot)
        {
            if (LesInfosDuSon[index].event3D)
            {
                FMODUnity.RuntimeManager.PlayOneShot(LesInfosDuSon[index].LeSonAJouer,transform.position);
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
    void setInfoSurSonByVoid(string LeVoid, out int IndexSon) 
    {
        IndexSon = 0;
        for (int i = 0; i < LesInfosDuSon.Count; i++)
        {
            if (LesInfosDuSon[i].LeVoid == LeVoid)
            {
                IndexSon = i;
                LesInfosDuSon[i].IndexDuSon = i;
                break;
            }

        }
        

    }
    void UpdateSoundWithParamater() 
    {
    
    
    }
    void SetAllSound()
    {
    foreach(SoundInfo Info in LesInfosDuSon) 
        {
        
            
            Info.MonEvenementFMOD = FMODUnity.RuntimeManager.CreateInstance(Info.LeSonAJouer);
            if (Info.event3D)
            {
                Info.MonEvenementFMOD.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
            }
            if (Info.ParameterToModify.Length>0)
            {
                SonAvecParameter.Add(Info);
            }
        }
    }

   

    void soundsToStop() 
    {
        foreach (SoundInfo Info in LesInfosDuSon)
        {
            if (Info.tempsPourArret>=Time.time)
            {
                Info.tempsPourArret = 0;
                Info.MonEvenementFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
    }
}

