using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lighter //script pour des infos
{
    [Tooltip(" le mesh renderer a changé ")]
    public MeshRenderer Light;
    [Tooltip(" material de la lumiére ")]
    public Material LightMat;
    [Tooltip(" le material de depart ")]
    public Material Old;
    [Tooltip(" La seconde à laquelle j'applique le materiaux lumineux ")]
    public int SecondeAlumage;

}

    public class LightDebut : MonoBehaviour
{
    //Public variable
    public Lighter[] mesLights;//array des infos
                               //Local variable
    #region methodeActive
    public void Active1() // pour activer les lumiére d'int 1
    {
        foreach (Lighter ligh in mesLights)
        {
            if (ligh.SecondeAlumage==1)
            {
                ligh.Light.material = ligh.LightMat;
            }
        }
    }
    public void Active2() //pour activer les lumiére d'int 2
    {
        foreach (Lighter ligh in mesLights)
        {
            if (ligh.SecondeAlumage == 2)
            {
                ligh.Light.material = ligh.LightMat;
            }
        }
    }
    public void Active3() //pour activer les lumiére d'int 3
    {
        foreach (Lighter ligh in mesLights)
        {
            if (ligh.SecondeAlumage == 3)
            {
                ligh.Light.material = ligh.LightMat;
            }
        }
    }
    #endregion

    public void Desactive() //pour désactiver les lumiére 
    {

        foreach (Lighter ligh in mesLights)
        {
            
                ligh.Light.material = ligh.Old;
            
        }
    }
    public void DesactiveTime(float time) //pour désactiver les lumiére après un certain temps
    {
        Invoke("Desactive", time);
    
    }
}
