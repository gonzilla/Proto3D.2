using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonnalMethod : MonoBehaviour
{
    //Public variable
    
    //Local variable

    public static void GetGestion(out GestionGeneral GG, GameObject Contenant) 
    {

        GG=Contenant.GetComponent<GestionGeneral>();
    
    }

    /*public static void GetVariableByname(string Name, MonoBehaviour script) 
    {
       var achercher =(float)script.GetType().GetField(Name).GetValue(script);
    }*/
}
