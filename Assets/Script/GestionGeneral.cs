using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionGeneral : PersonnalMethod
{
    [HideInInspector] public bool CanPlay;
    //Public variable
    [Tooltip(" Script pour input ")]
    public GestionDesInputs GDI;
    [Tooltip(" script du boost ")]
    public GestionBoost GB;
    [Tooltip(" script de l'UI ")]
    public GestionUI GUI;
    [Tooltip(" Script des déplacements ")]
    public GestionMotoControlleur GMC;
    [Tooltip(" Script de la cam ")]
    public CameraScriptFollow CSF;
    [Tooltip(" Pour Les sons pour le moment ")]
    public GestionEtatEtFeedback EtatEtFeedback;
    [Tooltip(" Pour Les feedBack pour le moment ")]
    public GestionFeedBackVisu FeedBackVisu;
    [Tooltip(" Pour Les feedBack pour le moment ")]
    public GestionCheckPoint GestionPointDeControle;
    //Local variable


}
