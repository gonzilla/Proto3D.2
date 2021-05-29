using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lighter
{
    public MeshRenderer Light;
    public Material LightMat;
    public Material Old;
    public int SecondeAlumage;

}

    public class LightDebut : MonoBehaviour
{
    //Public variable
    public Lighter[] mesLights;
    //Local variable

    public void Active1() 
    {
        foreach (Lighter ligh in mesLights)
        {
            if (ligh.SecondeAlumage==1)
            {
                ligh.Light.material = ligh.LightMat;
            }
        }
    }
    public void Active2()
    {
        foreach (Lighter ligh in mesLights)
        {
            if (ligh.SecondeAlumage == 2)
            {
                ligh.Light.material = ligh.LightMat;
            }
        }
    }
    public void Active3()
    {
        foreach (Lighter ligh in mesLights)
        {
            if (ligh.SecondeAlumage == 3)
            {
                ligh.Light.material = ligh.LightMat;
            }
        }
    }

    public void Desactive() 
    {

        foreach (Lighter ligh in mesLights)
        {
            
                ligh.Light.material = ligh.Old;
            
        }
    }

    public void DesactiveTime(float time) 
    {
        Invoke("Desactive", time);
    
    }
}
