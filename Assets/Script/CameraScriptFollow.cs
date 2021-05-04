using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class CameraScriptFollow : PersonnalMethod
{
    //Public variable
    [Tooltip("vitesse de deplacement de la cam ")]
    public float VitesseDeplacement;
    [Tooltip("vitesse de rotation de la cam ")]
    public float VitesseRotation;
    [Tooltip("l'objet a suivre en position")]
    public Transform MotoToFollow;
    [Tooltip("le transform de la moto")]
    public Transform LaMoto;
    // après changement
    [Header("FOV")]
    [Tooltip(" quelle value commence le FOV ")]
    public float StartValueOfFOV;
    [Range(0,1),Tooltip("à quelle pourcentage de la vitessemax le joueur se trouve le fov commence à changer ")]
    public float PourcentageVitesseFOVChange;
    [Tooltip("Le Fov Maximum ")]
    public float FOV_Max;
    [Tooltip("la vitesse de transition du FOV entre chaque")]
    public float VitesseFOV;

    [Header("RotationSurLeCoter")]
    [Tooltip("l'angle vers lequel tends la caméra")]
    public float AngleMaximalEnPlus;
    [Tooltip("vitesse De la Rotation sur Y")]
    public float VitesseRotationSurY;

    [Header("CameraShake")]
    [Tooltip("bool pour savoir si la camera vibre ou doit vibrer")]
    public bool DoitVibrer;
    [Tooltip("La force de la vobration")]
    public float ForceCameraShake;
    [Tooltip("Le temp de la vobration")]
    public float TempsDeVibration;

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
    // Start is called before the first frame update
    Camera CamProperties;
    GestionGeneral GG;
    float directionDeRotation;
    float TempsArretCameraShake;

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

    void Start()
    {
        CamProperties = GetComponent<Camera>();
        CamProperties.fieldOfView = StartValueOfFOV;
        GetGestion(out GG, LaMoto.gameObject);
        setPostProcess();
    }

    // Update is called once per frame
    public void CameraComportement()
    {
        
        
        
        //print(PourcentageDeVitesse());
        CameraFOV();
        transform.position = Vector3.Lerp(transform.position, MotoToFollow.position, VitesseDeplacement);// bouge la camera vers l'objet à suivre
                                         //transform.rotation = Quaternion.Slerp(transform.rotation, MotoToFollow.rotation, VitesseRotation);//rotate la camera la camera
        cameraRotation();
        if (DoitVibrer)
        {
            CameraShake();
        }
    }
    

    void CameraFOV() 
    {
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

    void cameraRotation() 
    {
        /*float speedRotation = VitesseRotationSurY*directionDeRotation;
        float angleRotation =  AngleMaximalEnPlus * directionDeRotation;
        float valueYcaclculer = Mathf.Lerp(transform.rotation.eulerAngles.y, angleRotation, speedRotation*Time.deltaTime);*/
        //float angleRotation = AngleMaximalEnPlus * directionDeRotation;
        //transform.rotation = Quaternion.Euler(0, MotoToFollow.rotation.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, MotoToFollow.rotation, VitesseRotation);//rotate la camera la camera
        Quaternion angleRotation =Quaternion.Euler( 0,AngleMaximalEnPlus * directionDeRotation,0);
        //transform.rotation = Quaternion.Slerp(transform.rotation, MotoToFollow.rotation*angleRotation, VitesseRotationSurY*Time.deltaTime);//


        //Quaternion Cible = Quaternion.Euler(0, angleRotation, 0)* Quaternion.Euler(0,LaMoto.rotation.y,0);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Cible, VitesseRotationSurY*Time.deltaTime);
        // set la camera rotation // pas vraiment utile
    }
    public void InfoRotationDeLaCam(float XJoystick) 
    {
        if (XJoystick!=0)
        {
            directionDeRotation = XJoystick / Mathf.Abs(XJoystick);
        }
        else 
        {
            directionDeRotation = 0;
        }
       
    }
    public void InfoRotationDeLaCam(float XJoystick, float InputDerapage) 
    {
        float directionDuJoystick = XJoystick / Mathf.Abs(XJoystick);

        if (XJoystick != 0 && directionDuJoystick == InputDerapage && InputDerapage!=0)
        {
            directionDeRotation = directionDuJoystick;
        }
        else
        {
            directionDeRotation = 0;
        }
    }

    public void LanceLeCameraShake() 
    {
        if (!DoitVibrer)
        {
            TempsArretCameraShake = TempsDeVibration + Time.time;
        }
        else 
        {
            DoitVibrer = true;
        }
    }
    void CameraShake() 
    {

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




    float  VitesseActuelDeLaMoto() 
    {
        float ActualSpeedMoto = GG.GMC.VitesseMoto;
        return ActualSpeedMoto;
    }
    float PourcentageDeVitesse() 
    {
        float actuelpourcentage = Mathf.Abs(GG.GMC.VitesseMoto)/GG.GMC.vitesseMax;
        return actuelpourcentage;
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
