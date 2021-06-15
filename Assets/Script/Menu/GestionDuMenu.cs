using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GestionDuMenu : MonoBehaviour
{
    public string ContreLaMontre;
    public string Versus;
    public GameObject Credits;
    [FMODUnity.EventRef]
    public string LeSonAJouer;
    FMOD.Studio.EventInstance MonEvenementFMOD;


    private void Start()
    {
        MonEvenementFMOD = FMODUnity.RuntimeManager.CreateInstance(LeSonAJouer);
        MonEvenementFMOD.start();
    }
    public void LanceContreLaMontre() 
    {
        MonEvenementFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        SceneManager.LoadScene(ContreLaMontre);
        

    }
    public void LanceVersus()
    {
        MonEvenementFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        SceneManager.LoadScene(Versus);

    }
    public void AfficheCredits() 
    {
        Credits.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);

    }

    public void QuitteUnity() 
    {
        MonEvenementFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Application.Quit();

    }
}
