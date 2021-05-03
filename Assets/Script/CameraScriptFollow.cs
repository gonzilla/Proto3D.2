using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    Camera CamProperties;
    GestionGeneral GG;
    float directionDeRotation;

    void Start()
    {
        CamProperties = GetComponent<Camera>();
        CamProperties.fieldOfView = StartValueOfFOV;
        GetGestion(out GG, LaMoto.gameObject);
    }

    // Update is called once per frame
    public void CameraComportement()
    {
        
        
        
        //print(PourcentageDeVitesse());
        CameraFOV();
        transform.position = Vector3.Lerp(transform.position, MotoToFollow.position, VitesseDeplacement);// bouge la camera vers l'objet à suivre
                                         //transform.rotation = Quaternion.Slerp(transform.rotation, MotoToFollow.rotation, VitesseRotation);//rotate la camera la camera
        cameraRotation();
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
