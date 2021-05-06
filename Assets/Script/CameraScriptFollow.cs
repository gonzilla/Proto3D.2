using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


[System.Serializable]
public class ScreenShakeInfo
{
    
    [Tooltip("La force de la vobration")]
    public float ForcePourCameraShake;
    [Tooltip("Le temp de la vobration")]
    public float TempsPourVibration;
    public enum Action
    {
        BoostLV1,
        BoostLV2,
        BoostLV3,
        Fusion,
        Collisions

    };
    public Action ListeDesActions;

}

    public class CameraScriptFollow : PersonnalMethod
{
    //Public variable
    #region Cam
    [Tooltip("vitesse de deplacement de la cam ")]
    public float VitesseDeplacement;
    [Tooltip("vitesse de deplacement de la cam lors d'une transition avant arriere")]
    public float VitesseDeplacementTransition;
    [Tooltip("DistanceMaxcam ")]
    public float distanceMaxTransition;
    [Tooltip("vitesse de rotation de la cam ")]
    public float VitesseRotation;
    [Tooltip("l'objet a suivre en position de marche avant")]
    public Transform MotoToFollowAvant;
    [Tooltip("l'objet a suivre en position de marche arriere")]
    public Transform MotoToFollowArreire;
    [Tooltip("le transform de la moto")]
    public Transform LaMoto;
    #endregion
    #region FOV
    [Header("FOV")]
    [Tooltip(" quelle value commence le FOV ")]
    public float StartValueOfFOV;
    [Range(0,1),Tooltip("à quelle pourcentage de la vitessemax le joueur se trouve le fov commence à changer ")]
    public float PourcentageVitesseFOVChange;
    [Tooltip("Le Fov Maximum ")]
    public float FOV_Max;
    [Tooltip("Le Multiplicateur de FOV lorsque je dépasse la vitesse de 100 ")]
    public float MultiplicateurFOV;
    [Tooltip("la vitesse de transition du FOV entre chaque")]
    public float VitesseFOV;
    #endregion
    #region rotation
    [Header("Rotation")]
    [Tooltip("l'angle vers lequel tends la caméra")]
    public float AngleMaximalEnPlus;
    [Tooltip("l'angle vers lequel tends la caméra en dérapage")]
    public float AngleMaximalEnPlusEnDerapage;
    [Tooltip("vitesse De la Rotation sur Y")]
    public float VitesseRotationSurY;
    [Tooltip("vitesse De retour sur Y")]
    public float VitesseRetourSurY;
    [Tooltip("Angle Max Sur Z")]
    public float AngleMaxZ;
    [Tooltip("vitesse De la Rotation sur Y")]
    public float VitesseRotationSurZ;
    [Tooltip("vitesse De retour sur Y")]
    public float VitesseRetourSurZ;
    #endregion
    #region shake
    [Header("CameraShake")]
    public List<ScreenShakeInfo> InfoPourScreenShake = new List<ScreenShakeInfo>();
    [Tooltip("bool pour savoir si la camera vibre ou doit vibrer")]
    public bool DoitVibrer;
    #endregion
    #region PostProcess
    [Header("PostProcess")]
    [Tooltip("l'objetContenant le post process Volume")]
    public PostProcessVolume monPostProcess;
    public enum PostProcessEffect 
    {
        AmbientOcclusions,
        AutoExposure,
        Bloom,
        ChromaticAbberration,
        ColorGrading,
        DepthOfField,
        Grain,
        LensDistortion,
        MotionBlur,
        ScreenSpaceRefecltion,
        Vignette
    
    };
    [Tooltip("les effets que je vais utiliser")]
    public PostProcessEffect[] MesPostProcessEffect;
    #endregion

    Camera CamProperties;
    GestionGeneral GG;
    bool TransitionCible = false;
    float directionDeRotation;
    float TempsArretCameraShake;
    float vitesseRotationMaxMoto;
    float FOVmaxOriginelle;
    float ForceCameraShake;
    float TempsDeVibration;
    float DistanceForCam;
    Transform MotoToFollow;

    #region PostProcess2
    AmbientOcclusion _AmbientOclu;
    AutoExposure _AutoExpo;
    Bloom _bloom;
    ChromaticAberration _Chromatic;
    ColorGrading _ColorGrad;
    DepthOfField _DepthOf;
    Grain _Grain;
    LensDistortion _Lens;
    MotionBlur _MotionBlur;
    ScreenSpaceReflections _ScreenSpace;
    Vignette _Vignette;
    #endregion
    void Start()
    {
        DistanceForCam = Vector3.Distance(MotoToFollowArreire.position, MotoToFollowAvant.position);
         MotoToFollow = MotoToFollowAvant;
        FOVmaxOriginelle = FOV_Max;
        CamProperties = GetComponent<Camera>();
        CamProperties.fieldOfView = StartValueOfFOV;
        GetGestion(out GG, LaMoto.gameObject);
        setPostProcess();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    public void CameraComportement()
    {
        
        
        
        //print(PourcentageDeVitesse());
        CameraFOV();
        checkCible();

        if (!TransitionCible)
        {
            cameraRotation();
        }
        if(TransitionCible)
        {
            print("loook");
            transform.LookAt(LaMoto.position);//new Vector3(LaMoto.position.x, transform.position.y,LaMoto.position.z));
        }
        if (DoitVibrer)
        {
            CameraShake();
        }
    }
    

    void CameraFOV() 
    {
        if (Mathf.Abs(vitesseDeRotationDeLaMoto())>1)
        {
            FOV_Max = FOVmaxOriginelle * MultiplicateurFOV;
        }
        else
        if (PourcentageVitesseFOVChange < PourcentageDeVitesse())
        {
            //print("je fais des trucs");
            float FOVCible = StartValueOfFOV + ((FOV_Max - StartValueOfFOV) * PourcentageDeVitesse());
            CamProperties.fieldOfView = Mathf.Lerp(CamProperties.fieldOfView, FOVCible, VitesseFOV*Time.deltaTime);
            
        }
        else 
        {
            CamProperties.fieldOfView = Mathf.Lerp(CamProperties.fieldOfView, StartValueOfFOV, VitesseFOV * Time.deltaTime); 
        }
       
    }

    void checkCible() 
    {
        Transform previousCible = null;
        float speedTransition = VitesseDeplacement;
        if (!TransitionCible)
        {
            previousCible = MotoToFollow;
            
        }
        else 
        {
            speedTransition = VitesseDeplacementTransition;
        }
        if (Vector3.Distance(transform.position, MotoToFollow.position) > distanceMaxTransition + DistanceForCam && TransitionCible)
        {
            speedTransition = VitesseDeplacementTransition * 100;
        }
        if (Avance() || VitesseActuelDeLaMoto()==0)
        {
            MotoToFollow = MotoToFollowAvant;
        }
        else if (!Avance()) 
        {
            MotoToFollow = MotoToFollowArreire;
        }
        if (MotoToFollow != previousCible)
        {
            TransitionCible = true;
        }
        transform.position = Vector3.Lerp(transform.position, MotoToFollow.position, speedTransition*Time.deltaTime);// bouge la camera vers l'objet à suivre
        if (Vector3.Distance(transform.position,MotoToFollow.position)<1f && TransitionCible)
        {
            TransitionCible = false;
            transform.position = MotoToFollow.position;
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
       
    }

    void cameraRotation() 
    {
        float angleCibleY = 0;
        if (deraping())
        {
             angleCibleY = LaMoto.rotation.eulerAngles.y + AngleMaximalEnPlusEnDerapage * directionDeRotation;
        }
        else 
        {
             angleCibleY = LaMoto.rotation.eulerAngles.y + AngleMaximalEnPlus * directionDeRotation;
        }
        
        float angleY = 0;
        float vitesseAngleY = VitesseRotationSurY;
        float angleCibleZ = AngleMaxZ*-directionDeRotation;
        float angleZ =0;
        float vitesseAngleZ = VitesseRotationSurZ;
        float angleX = 0;
        float vitesseAngleX = (vitesseAngleY + vitesseAngleZ) / 2;
        

        if (directionDeRotation != 0)
        {
            vitesseAngleY = VitesseRotationSurY * PourcentageRotationSpeed() * Time.deltaTime;
            vitesseAngleZ = VitesseRotationSurZ * PourcentageRotationSpeed() * Time.deltaTime;
        }
        if (directionDeRotation == 0)
        {

            if (angleY != LaMoto.rotation.eulerAngles.y)
            {
                vitesseAngleY = VitesseRetourSurY * Time.deltaTime; //VitesseRotationSurY *Mathf.Abs( PourcentageRotationSpeed()-1 )

            }
            if (angleZ == 0 && transform.rotation.eulerAngles.z!=0)
            {
                
                vitesseAngleZ = VitesseRetourSurZ * Time.deltaTime;
            }
        }
        // essayer de trouver une méthode pour faire la meme chose que en marche avant

        if (VitesseActuelDeLaMoto() != 0 && !Avance())
        {
            angleY = Mathf.LerpAngle(transform.rotation.eulerAngles.y, angleCibleY+180, vitesseAngleY);
        }
        else 
        {
            angleY = Mathf.LerpAngle(transform.rotation.eulerAngles.y, angleCibleY, vitesseAngleY);
        }
        if (transform.rotation.eulerAngles.x !=0)
        {
            angleX = Mathf.LerpAngle(transform.rotation.eulerAngles.z, 0, vitesseAngleX);
        }
        angleZ = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angleCibleZ, vitesseAngleZ);
        transform.rotation = Quaternion.Euler(angleX, angleY, angleZ);
    }
    public void InfoRotationDeLaCam(float XJoystick) 
    {

        float directionDuJoystick = 0;
        if (XJoystick != 0)
        {
            directionDuJoystick = XJoystick / Mathf.Abs(XJoystick);
        }
        directionDeRotation = directionDuJoystick;

    }
    /*public void InfoRotationDeLaCam(float XJoystick, float InputDerapage) 
    {
        
        float directionDuJoystick = 0;
        if (XJoystick != 0)
        {
            directionDuJoystick = XJoystick / Mathf.Abs(XJoystick);
        }
        
        
        if (deraping())
        {
            directionDeRotation = directionDuJoystick;
            

        }
        else
        {
            directionDeRotation = directionDuJoystick;
        }




    }*/

    #region cameraShake
    public void GestionCameraShake(ScreenShakeInfo.Action ActionProduite) 
    {
        print("gestion");
        foreach (ScreenShakeInfo Act in InfoPourScreenShake)
        {
            if (ActionProduite == Act.ListeDesActions && Act.ListeDesActions== ScreenShakeInfo.Action.BoostLV1)
            {
                print("Level 1");
                LanceLeCameraShake(Act.ForcePourCameraShake, Act.TempsPourVibration);
            }
            else if (ActionProduite == Act.ListeDesActions && Act.ListeDesActions == ScreenShakeInfo.Action.BoostLV2)
            {
                print("Level 2");
                LanceLeCameraShake(Act.ForcePourCameraShake, Act.TempsPourVibration);
            }
            else if (ActionProduite == Act.ListeDesActions && Act.ListeDesActions == ScreenShakeInfo.Action.BoostLV3)
            {
                print("Level 3");
                LanceLeCameraShake(Act.ForcePourCameraShake, Act.TempsPourVibration);
            }
            else if (ActionProduite == Act.ListeDesActions && Act.ListeDesActions == ScreenShakeInfo.Action.Collisions)
            {
                LanceLeCameraShake(Act.ForcePourCameraShake, Act.TempsPourVibration);
            }
            else if (ActionProduite == Act.ListeDesActions && Act.ListeDesActions == ScreenShakeInfo.Action.Fusion)
            {
                LanceLeCameraShake(Act.ForcePourCameraShake, Act.TempsPourVibration);
            }
        }
        

    }

    void LanceLeCameraShake(float Force,float TempsOfVibration) 
    {
        print("Lance");
        if (!DoitVibrer)
        {
            print("Doit vibrer");
            DoitVibrer = true;
        }
        if (DoitVibrer)
        {
            print(TempsOfVibration);
            TempsDeVibration = TempsOfVibration;
            TempsArretCameraShake = TempsDeVibration + Time.time;
            ForceCameraShake = Force;
        }
        
    }

    void CameraShake() 
    {

        print("Shake");
        if (TempsArretCameraShake ==0)
        {
            TempsArretCameraShake = TempsDeVibration + Time.time;
        }
        else 
        {
            
            float x = Random.Range(-1f, 1f) * ForceCameraShake;
            float y = Random.Range(-1f, 1f) * ForceCameraShake;
            transform.localPosition = new Vector3(transform.localPosition.x+x, transform.localPosition.y + y, transform.localPosition.z);
        }
        if (Time.time>TempsArretCameraShake)
        {
            DoitVibrer = false;
            TempsArretCameraShake = 0;
        }

    }

    #endregion
    public void setRotationSpeedMax(float newspeed)
    {
        vitesseRotationMaxMoto = newspeed;
    }

    void setPostProcess() 
    {
        foreach (PostProcessEffect Effect in MesPostProcessEffect)
        {
            if (Effect==PostProcessEffect.AmbientOcclusions)
            {
               _AmbientOclu = monPostProcess.GetComponent<AmbientOcclusion>();
            }
            else if (Effect == PostProcessEffect.AutoExposure)
            {
               _AutoExpo = monPostProcess.GetComponent<AutoExposure>();
            }
            else if (Effect == PostProcessEffect.Bloom)
            {
              _bloom = monPostProcess.GetComponent<Bloom>();
            }
            else if (Effect == PostProcessEffect.ChromaticAbberration)
            {
              _Chromatic = monPostProcess.GetComponent<ChromaticAberration>();
            }
            else if (Effect == PostProcessEffect.ColorGrading)
            {
              _ColorGrad = monPostProcess.GetComponent<ColorGrading>();
            }
            else if (Effect == PostProcessEffect.DepthOfField)
            {
              _DepthOf = monPostProcess.GetComponent<DepthOfField>();
            }
            else if (Effect == PostProcessEffect.Grain)
            {
              _Grain = monPostProcess.GetComponent<Grain>();
            }
            else if (Effect == PostProcessEffect.LensDistortion)
            {
              _Lens = monPostProcess.GetComponent<LensDistortion>();
            }
            else if (Effect == PostProcessEffect.MotionBlur)
            {
              _MotionBlur = monPostProcess.GetComponent<MotionBlur>();
            }
            else if (Effect == PostProcessEffect.ScreenSpaceRefecltion)
            {
              _ScreenSpace = monPostProcess.GetComponent<ScreenSpaceReflections>();
            }
            else if (Effect == PostProcessEffect.Vignette)
            {
              _Vignette = monPostProcess.GetComponent<Vignette>();
            }
        }
    
    }


    bool deraping() 
    {
        bool derape;
        if (vitesseRotationMaxMoto==GG.GMC.VitesseDeDerapageMax)
        {
            derape = true;
        }
        else 
        {
            derape = false;
        }
        return derape;
    }



    float  VitesseActuelDeLaMoto() 
    {
        float ActualSpeedMoto = GG.GMC.VitesseMoto;
        return ActualSpeedMoto;
    }
    float PourcentageDeVitesse() 
    {
        float value = GG.GMC.VitesseMoto;
        float actuelpourcentage = 0;
        if (value !=0)
        {
            actuelpourcentage = Mathf.Abs(value) / value;
        }
       
        return actuelpourcentage;
    }
    
    float vitesseDeRotationDeLaMoto() 
    {
        float actuelvitesseRotat = GG.GMC.ActuelVitesseRotation;
        return actuelvitesseRotat;
    }

    float PourcentageRotationSpeed() 
    {
        float pourcentage = Mathf.Abs(vitesseDeRotationDeLaMoto()) / vitesseRotationMaxMoto;
        return pourcentage;
    }

    float rotationDelaMoto() 
    {

        float YDelaMoto = UnityEditor.TransformUtils.GetInspectorRotation(LaMoto).y;
        return YDelaMoto;
    }
    
  
    
    bool Avance() 
    {
        if (VitesseActuelDeLaMoto()>0)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }




}
/*float speedRotation = VitesseRotationSurY*directionDeRotation;
        float angleRotation =  AngleMaximalEnPlus * directionDeRotation;
        float valueYcaclculer = Mathf.Lerp(transform.rotation.eulerAngles.y, angleRotation, speedRotation*Time.deltaTime);*/
//float angleRotation = AngleMaximalEnPlus * directionDeRotation;
//transform.rotation = Quaternion.Euler(0, MotoToFollow.rotation.y, 0);
//transform.rotation = Quaternion.Slerp(transform.rotation, MotoToFollow.rotation*angleRotation, VitesseRotationSurY*Time.deltaTime);//
//Quaternion Cible = Quaternion.Euler(0, angleRotation, 0)* Quaternion.Euler(0,LaMoto.rotation.y,0);
//transform.rotation = Quaternion.Lerp(transform.rotation, Cible, VitesseRotationSurY*Time.deltaTime);
// set la camera rotation // pas vraiment utile
//float angleYDelacam =  Mathf.LerpAngle(transform.rotation.y, MotoToFollow.rotation.y + AngleMaximalEnPlus,Time.deltaTime*VitesseRotationSurY);
//Quaternion.AngleAxis(angleYDelacam, transform.up)
//transform.rotation = Quaternion.Slerp(transform.rotation, MotoToFollow.rotation, VitesseRotation);//rotate la camera la camera
//Quaternion angleRotation = Quaternion.Euler(0, AngleMaximalEnPlus * directionDeRotation, 0);
/*float anglecibleY = LaMoto.rotation.eulerAngles.y + AngleMaximalEnPlus * directionDeRotation;
float anglePourY = Mathf.LerpAngle(transform.rotation.eulerAngles.y, anglecibleY, (vitesseDeRotationDeLaMoto()+VitesseRotationSurY)*Time.deltaTime);
transform.rotation = Quaternion.AngleAxis(anglePourY, transform.up);*/
//transform.rotation = Quaternion.Slerp(transform.rotation, MotoToFollow.rotation, VitesseRotation);//rotate la camera la camera
/*float directionDeLaMoto() 
    {
        float direction = Mathf.Abs(GG.GMC.VitesseMoto) / GG.GMC.VitesseMoto;
        return direction;
    }*/

/*if (XJoystick != 0 && directionDuJoystick == InputDerapage && InputDerapage != 0)
            {
                directionDeRotation = directionDuJoystick;
            }
            else if (XJoystick != 0 && directionDuJoystick != InputDerapage)
            {
                InfoRotationDeLaCam(XJoystick);
            }*/