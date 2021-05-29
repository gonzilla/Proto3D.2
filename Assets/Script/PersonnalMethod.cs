using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonnalMethod : MonoBehaviour
{
    // Scipt Mére, pour faire des méthode personnalisé qui ne sont pas faites sur unity

    public static void GetGestion(out GestionGeneral GG, GameObject Contenant) //methode pour aller chercher le script de gestion general
    {
        GG=Contenant.GetComponent<GestionGeneral>();
    }

  
}
