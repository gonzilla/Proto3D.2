using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GestionUI : PersonnalMethod
{
    //Public variable
    [Tooltip(" Le text qui affiche la vitesse ")]
    public TextMeshProUGUI Speed;
    [Tooltip(" Les Images pour la bar de boost ")]
    public Image[] BoostVisual;
    [Tooltip(" LesValeurs de width ")]
    public float[] widthBarMax;
    #region ValeurCompteRebour
    [Tooltip(" Le temps pour compte a rebours ")]
    public float TimeBeforeStart;
    [Tooltip(" Le temps avant de Lancer le compte à rebours ")]
    public float TimeAvantDebutChrono;
    [Tooltip(" Le temps avant de cacher le compte à rebours ")]
    public float TimeForHideCompteur;
    [Tooltip(" Le text du timer ")]
    public float TempsParPenalite;
    [Tooltip(" Le text du timer ")]
    public TextMeshProUGUI[] Timer;
    [HideInInspector] public float LeTimeInGameArrondie;
    #endregion
    [Tooltip(" Le textDu chrono ")]
    public TextMeshProUGUI Chrono;
    [Tooltip(" arrondis la valeur afficher du chrono ")]
    public int arrondisDecimalChrono = 3;
    [Tooltip(" arrondis la valeur afficher de la vitesse")]
    public int arrondisDecimal = 2;
    [Tooltip(" temps pdt lequel le text change de couleur ")]
    public float TimeFeedBackText = 1;
    #region affichageStats
    [Tooltip("l'objet montrant les stats")]
    public GameObject Stats;
    [Tooltip("Le temps du circuit")]
    public TextMeshProUGUI TempsDeCircuit;
    [Tooltip("Le meilleurs Temps que le joueur a fait")]
    public TextMeshProUGUI MeilleurTemps;
    [Tooltip("Le meilleur tour que le joueur a effectué")]
    public TextMeshProUGUI MeilleurTour;
    [Tooltip("Le temps de penalite que le joueur a effectué")]
    public TextMeshProUGUI Penalite;
    #endregion
    public LightDebut LD;
    //Local variable
    float TimeBeforeStartOriginal;
    float LeTimeInGame;
    float TimeAtStart;
    float meilleurTour = 0;
    float previousTour;
    float penalityFinal;

    float[] widthBar = new float[4];
    GestionGeneral GG;// récupére les autres script
    Color OldColor;


    void Start()
    {
        GetGestion(out GG, this.gameObject);// récupére les autres script
        TimeBeforeStartOriginal = TimeBeforeStart;// set le temps Original
        Chrono.gameObject.SetActive(false);
        foreach (TextMeshProUGUI item in Timer)
        {
            item.gameObject.SetActive(false);
        }

        GG.CanPlay = false;//indique au jeu que le joueur ne peut pas jouer
        OldColor = Speed.color;// set la old color pour garder en mémoire lors de la collisions
        Invoke("SecondDeMoins", TimeAvantDebutChrono);//Lance le chrono

    }


    void Update()
    {
        float valueVelocity = GG.GMC.VitesseMoto * 100;//multiplie la valeur de la moto 
        float arrondis = (float)System.Math.Round(valueVelocity, arrondisDecimal);//arrondis la valeur a afficher
        Speed.text = arrondis.ToString() + " Km/H";//affiche la valeur
        if (GG.CanPlay)//si le jeu est lancé
        {
            LeTimeInGame = Time.time - TimeAtStart + penalityFinal;//Le chrono en some
            LeTimeInGameArrondie = (float)System.Math.Round(LeTimeInGame, arrondisDecimalChrono);// Calcul le temps à afficher
            Chrono.text = LeTimeInGameArrondie.ToString(); //affiche le text
        }

    }

    public void setSliderBoost(float valueToShow)
    {

        // BoostVisual.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, valueToShow); // set la value du slider
        int indexBar = 0;
        float max = 0;
        float min = 0;
        if (valueToShow <= 0.25)
        {
            indexBar = 0;
            min = 0;
            max = 0.25f;
        }
        else if (valueToShow <= 0.5 && valueToShow > 0.25)
        {
            indexBar = 1;
            min = 0.25f;
            max = 0.5f;
        }
        else if (valueToShow > 0.5 && valueToShow <= 0.75)
        {
            indexBar = 2;
            min = 0.5f;
            max = 0.75f;
        }
        else if (valueToShow > 0.75)
        {
            indexBar = 3;
            min = 0.75f;
            max = 1f;
        }
        widthBar[indexBar] = widthBarMax[indexBar] * (valueToShow - min) / (max - min);
        for (int i = 0; i < widthBar.Length; i++)
        {
            if (indexBar == i)
            {
                BoostVisual[indexBar].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthBar[indexBar]);
            }
            if (i < indexBar)
            {
                BoostVisual[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthBarMax[i]);
            }
            if (i > indexBar)
            {
                BoostVisual[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            }
        }

    }

    public void setTextCouleur()
    {
        Speed.color = Color.red;// change la couleur en rouge
        Invoke("resetTextCouleur", TimeFeedBackText);// demande pour reset la couleur du text
    }

    public void CheckTimePourMeilleurTour(int tour)
    {

        if (tour > 1)
        {
            float calculDuTour = LeTimeInGame - previousTour;
            float CalculDuTourArrondie = (float)System.Math.Round(calculDuTour, arrondisDecimalChrono);
            if (calculDuTour < meilleurTour && meilleurTour != 0)
            {
                meilleurTour = CalculDuTourArrondie;
            }
            else if (meilleurTour == 0)
            {
                meilleurTour = CalculDuTourArrondie;
            }
            previousTour += calculDuTour;

        }


    }
    public void AffichageStats()
    {
        GG.EtatEtFeedback.stopAllSound();
        Stats.SetActive(true);
        MeilleurTour.text = meilleurTour.ToString();
        TempsDeCircuit.text = LeTimeInGameArrondie.ToString();
        Penalite.text = penalityFinal.ToString();

        if (PlayerPrefs.HasKey("MeilleurTemps"))
        {
            float MeilleurtempsOfAllTime = PlayerPrefs.GetFloat("MeilleurTemps");
            if (LeTimeInGameArrondie < MeilleurtempsOfAllTime)
            {
                MeilleurTemps.text = LeTimeInGameArrondie.ToString();
                PlayerPrefs.SetFloat("MeilleurTemps", LeTimeInGameArrondie);
            }
            else
            {
                MeilleurTemps.text = MeilleurtempsOfAllTime.ToString();
            }
        }
        else
        {
            MeilleurTemps.text = LeTimeInGameArrondie.ToString();
            PlayerPrefs.SetFloat("MeilleurTemps", LeTimeInGameArrondie);
        }
        PlayerPrefs.Save();



    }

    void resetTextCouleur()
    {
        Speed.color = OldColor; // remet la couleur

    }
    void SecondDeMoins()
    {
        if (TimeBeforeStart == 3 || TimeBeforeStart == 2)
        {
            LD.DesactiveTime(0.5f);
            LD.Active3();

        }
        else if (TimeBeforeStart == 1)
        {
            LD.Desactive();
            LD.Active2();
        }
        else if (TimeBeforeStart == 0)
        {
            LD.Desactive();
            LD.Active1();
            LD.DesactiveTime(3);

        }
        foreach (TextMeshProUGUI item in Timer)
        {
            item.text = TimeBeforeStart.ToString();//set le text
            if (!item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(true);//active le chrono
            }
        }
        if (TimeBeforeStart > 0) // tant qu'il reste du temps
        {
            TimeBeforeStart--;//enléve du temp
            Invoke("SecondDeMoins", 1f);// se rappel lui même
        }
        else
        {
            TimeAtStart = Time.time;//set le time at start
            GG.CanPlay = true;// lui dis qu'il peut jouer
            Chrono.gameObject.SetActive(true);
            Invoke("HideCompteARebours", TimeForHideCompteur);//cache le compe a rebours
        }


    }
    void HideCompteARebours()
    {
        print("cache");
        foreach (TextMeshProUGUI item in Timer)
        {

            if (item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(false);//active le chrono
            }
        }

    }

    public void addPenality()
    {

        penalityFinal += TempsParPenalite;
    }
}
