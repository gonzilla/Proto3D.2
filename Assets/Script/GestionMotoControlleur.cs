﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionMotoControlleur : PersonnalMethod
{
    #region publicVariable
    //Public variable
    [Header("Vitesse")]
    [Tooltip("vitesse actuelle de la moto")]
    public float VitesseMoto;// vitesse de la moto
    [Tooltip("vitesse max de la moto")]
    public float vitesseMax;
    [Tooltip("Valeur d'accélération par seconde")]
    public float AccélérationMoto;
    [Tooltip("Valeur d'accélérationselon vitesse en %")]
    public AnimationCurve AccelerationSelonVitesseMax;
    [Tooltip("Valeur de puissance de freinage")]
    public float PuissanceFrainage;
    [Tooltip("Valeur du freinage selon vitesse en %")]
    public AnimationCurve FreinageSelonVitesse;
    [Tooltip("Force de ralentissement naturelle")]
    public float ForceRalentissement;
    [Tooltip("Force de ralentissement En l'air")]
    public float ForceRalentissementEnLair;
    [Tooltip("Valeur du ralentissement selon vitesse en %")]
    public AnimationCurve Ralentissement;
    [Tooltip("vitesse de descente de la vitessemoto lorsque celle-ci est au dessus de la vitesse max")]
    public float DecellerationVitesseElleve;
    [Tooltip("Value à partir de laquel la vitesse clamp a 0")]
    public float SnapVitesse = 0.001f;

    [Header("Tourner/Déraper")]
    [Tooltip("Le taux de pression du joystick pour débuter la rotation ou le dérapage")]
    public float ValeurJoysticPourRotation;
    [Tooltip("jusqu'à quelle angle la moto s'incline")]
    public float AngleMotoLorsDeRotation;
    [Tooltip("Transform contenant le modele de la moto")]
    public Transform ModeleMoto;

    [HideInInspector]
    public float ActuelVitesseRotation;//variable commune qui sera utiliser pour déterminé le sens ainsi que la vitesse de rotation
    [Header("Tourner")]
    [Tooltip("Vitesse à partir de laquelle la moto peut tourner")]
    public float VitesseMinimumPourTourner;
    [Tooltip("Vitesse à la quelle augmente sa rotation")]
    public float VitesseDeProgressionDeLaRotation ;
    [Tooltip("Vitesse de rotation pour tourner au maximum")]
    public float VitesseDeRotationMax;


    [Header("Dérapage")]
    
    [Tooltip("Vitesse  à partir de laquelle la moto peut deraper")]
    public float VitesseMinimumPourDeraper;
    [Tooltip("Vitesse à la quelle augmente son derapage")]
    public float VitesseDeProgressionDuDerapage;
    [Tooltip("Vitesse de rotation pour deraper au maximum")]
    public float VitesseDeDerapageMax;
    [Range(0,1),Tooltip("Force de ralentissement lorsderapage")]
    public float ForceRalentissementDerapage;
    [Tooltip("Valeur du ralentissement lors du derapage selon vitesse en %")]
    public AnimationCurve RalentissementDerapage;

    [Header("Pour Raycast")]
    [Tooltip("l'objet d'ou part le raycast")]
    public Transform detecteur;
    public Transform ResetPosition;
    [Tooltip("la longueur du raycast")]
    public float GroundRayLength;
    [Tooltip("permet de choisir le layer du ground")]
    public LayerMask whatIsGround;
   
    

    [Header("Collision")]
    [Tooltip("La perte de boost lorsqu'entre en collision pdt surchauffe")]
    public float PerteParCollision; // perte de vitesse lors d'une collision

    [Header("Autre")]
    [Tooltip("Le tempspour que le systéme estime le joueur dans les airs")]
    public float TempsPourEtreEnLair;
    public float TempsAvantRespawn;
    [Tooltip("l'angle pour se décaler ")]
    public float angleMax;
    public float StraffPowa;
    public float FakeGravityMultiplier;
    [Tooltip("les wheels pour l'animation ")]
    public Animator[] wheelAnimation;
    [HideInInspector]public bool grounded; // pour savoir sur sol
    #endregion

    #region local
    //Local variable
    float TimeCible; // le temps viser pour savoir si le joueur est en l'air
    float originalVitesseMax;
    float OriginalFakeGravityMultiplier;

    Vector3 DirectionForMoto; //
    Vector3 LastPostionOnCircuit;
   [HideInInspector] public Vector3 FakeGravity;

    Quaternion LastRotation;
   
    bool OnceForFloor; // pour détecter si en l'air
    bool staffing=false;
    bool Deraping = false;
    bool GNEU=false;
    bool reseting = false;

    Rigidbody Rb; // stock le rigidbody
    GestionGeneral GG;//stock les script
    ConstantForce LantiGravite;
    Quaternion PreviousRotation;

    #endregion

    void Start()
    {
        originalVitesseMax = vitesseMax;
        OriginalFakeGravityMultiplier = FakeGravityMultiplier;
        Rb = GetComponent<Rigidbody>();// récupére le rigidbody
        GetGestion(out GG, this.gameObject);// récupére les autres script
        //FirstVitesseRotation = ;//set old votesse rotation
        LantiGravite = GetComponent<ConstantForce>();
        FakeGravity = -Vector3.up * Physics.gravity.magnitude;
    }
    public void SetByNormal() 
    {
        
        grounded = false; // lui dis qu'il n'est pas au sol
        RaycastHit hit;// stock les infos du raycast

        
        if (Physics.Raycast(detecteur.position, -transform.up, out hit, GroundRayLength, whatIsGround) ) // si le raycast detect le sol
        {
            reseting = false;
            grounded = true;//passe le bool en true
            GNEU = false;
            if (OriginalFakeGravityMultiplier!= FakeGravityMultiplier)
            {
                FakeGravityMultiplier = OriginalFakeGravityMultiplier;
            }
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            Rb.angularVelocity = Vector3.zero;
            LastPostionOnCircuit = ResetPosition.position;
            LastRotation = ResetPosition.rotation;
            OnceForFloor = false; // set le once a false
            FakeGravity = -transform.up;
            LantiGravite.force = FakeGravity * Physics.gravity.magnitude ;
            //Physics.gravity = -transform.up * 9.81f;  //-hit.normal * 9.81f;   //-transform.up * 9.81f;  //-hit.normal* Physics.gravity.magnitude; //-transform.up * Physics.gravity.magnitude;
            //FreezeRotation();// freeze la rotation selon resultat
        }
        if (!grounded)// si n'est pas ground
        {
            if (!reseting)
            {
                LantiGravite.force = Physics.gravity *FakeGravityMultiplier;
            }
            

            // PourForceConstante = Vector3.zero;
            if (!OnceForFloor)// et que le Once est false
            {
                
                TimeCible = Time.time+TempsPourEtreEnLair; //set le temps pour check
                OnceForFloor = true;// le passe a true
                //FreezeRotation(); // freeze la rotation
            }
            if (OnceForFloor && TimeCible<= Time.time )//si le temps est dépassé
            {
                if (!GNEU)
                {
                    Invoke("ResetLastPosition", TempsAvantRespawn);
                    GNEU = true;
                }
               
                //Physics.gravity = new Vector3(0, -9.81f, 0);
                
            }
            
        }
        //FreezeRotation();
        // LantiGravite.force = PourForceConstante;
        //transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation; // set la rotation selon le sol
        //LastPostionOnCircuit = DetecteurArriere.transform.position + new Vector3(0, Mathf.Abs(DetecteurArriere.transform.localPosition.y), 0);
    }
    private void LateUpdate()
    {
        FreezeRotation();
    }

    public void avance(float Direction) // fait avancer le véhicule
    {
        
            
                
                
            float DirectionMoteur = 0; // enregistre la direction du moteur
            float DirectionVoulue = 0; // enregistre la direction voulue
            float pourcentage = Mathf.Abs( VitesseMoto) / vitesseMax;// détermine le pourcentage de vitesse de la moto
            if (VitesseMoto != 0)// si la vitesse n'est pas nul
            {
                DirectionMoteur = VitesseMoto / Mathf.Abs(VitesseMoto);// Direction du moteur
            }
            if (Direction != 0) // si le joueur appuie
            {
                DirectionVoulue = Direction / Mathf.Abs(Direction);//détermine la direction voulue
            }
            if (DirectionVoulue>0 && DirectionMoteur == DirectionVoulue && ActuelVitesseRotation == 0 && !staffing && !GG.GB.boosting && !GG.GB.Surchauffing && !GG.GB.Recharge)
            {
            GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.Avance);
            }
            if (DirectionVoulue < 0 && DirectionMoteur==DirectionVoulue && ActuelVitesseRotation == 0 && !staffing && !GG.GB.boosting && !GG.GB.Surchauffing &&!GG.GB.Recharge)
            {
            GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.Recule);
            }
            if (VitesseMoto==0)
            {
            
                if (DirectionVoulue!=0)
                {
                GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.Stationnaire);
                }
           
            }

        if (DirectionMoteur != DirectionVoulue && DirectionMoteur != 0)// si la direction moteur n(est pas la direction voulue et la direction moteur n'est pas 0
            {
                Freine(DirectionVoulue, Direction, pourcentage);//Lance le frein
            }
            else if (DirectionMoteur == DirectionVoulue || DirectionMoteur == 0) //sinon
            {
                if (grounded){//si sur le sol
                    if (VitesseMoto<vitesseMax)
                    {
                         VitesseMoto += AccélérationMoto * Direction * Time.deltaTime * AccelerationSelonVitesseMax.Evaluate(pourcentage);// calcule la vitesse
                    }
                
                checkVitesseMoto();// check la vitesse de la moto
                }
                else if (!grounded) 
                 {
                    Freine(DirectionVoulue, Direction, pourcentage);
                 }
            }
        DirectionForMoto += transform.forward;//set la direction
        DeclenchementParticuleSelonVitesse();
        foreach (Animator item in wheelAnimation)
        {
            item.speed = VitesseMoto;
        }
        transform.Translate(DirectionForMoto.normalized * VitesseMoto, Space.World);//fais le déplacement
        DirectionForMoto = Vector3.zero;// remet la direction 
        



    }
    public void tourne(float X) //fait tourner la moto
    {
        
        if (grounded && Mathf.Abs(VitesseMoto) > VitesseMinimumPourTourner)//si je suis sur le sol et que la vitesse est suffisante
        {
            
            if (Mathf.Abs(X) > ValeurJoysticPourRotation)// si la pressions est suffisante
            {
                ActuelVitesseRotation += X * VitesseDeProgressionDeLaRotation  * Time.deltaTime; // augmente la vitesse de rotation 
            }

            checkRotationMoto(VitesseDeRotationMax);
        }
        if (Mathf.Abs(X) < ValeurJoysticPourRotation || Mathf.Abs(VitesseMoto) < VitesseMinimumPourTourner)// si le joueur ne touche pas assez au joystick
        {
            ActuelVitesseRotation = 0;//reset la valeur de rotation
        }


        transform.rotation *= Quaternion.Euler(0, ActuelVitesseRotation, 0);// tourne la moto

        if (ActuelVitesseRotation !=0 && !staffing && !GG.GB.boosting && !GG.GB.Surchauffing && !GG.GB.Recharge)
        {
            GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.Tourne);
        }
        PreviousRotation = transform.rotation;
    }

    public void RotateMotoInWorld() 
    {
        float pourcentage = 0;
        float direction = 0;
        float angleZ = 0;
        if (ActuelVitesseRotation !=0)
        {
            pourcentage = Mathf.Abs(ActuelVitesseRotation) / VitesseDeRotationMax;
            direction = Mathf.Abs(ActuelVitesseRotation) / ActuelVitesseRotation;
            angleZ = AngleMotoLorsDeRotation * -direction * pourcentage;
        }
        else 
        {
            angleZ = Mathf.LerpAngle(ModeleMoto.localRotation.eulerAngles.z,0, VitesseDeProgressionDeLaRotation*Time.deltaTime);
        }
       
        ModeleMoto.localRotation = Quaternion.Euler(0, 0, angleZ);
    
    }

    public void straff(float X) // fait straffer
    {
        staffing = false;
        if (grounded && Mathf.Abs(VitesseMoto) > 0.01f) // si la moto est au sol est à une vitesse sup a 0.01f
        {
            float angle = X * angleMax;// decale selon angle max
            DirectionForMoto = Quaternion.AngleAxis(angle, transform.up) * transform.forward*StraffPowa;//calcul la rotation
            GG.FeedBackVisu.GestionStraff(true,true);
            if (X>0.3f|| X < -0.3f)
            {
                staffing = true;
                GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.Straff);
            }
            else 
            {
                staffing = false;
            }
           
            //actualRotatevalue += ValuePourcentageForRotate * Time.deltaTime * Mathf.Abs(X);// l'applique la rotation
        }
    }

    #region Derapage
    public void TourneDerapage(float DirectionRotation, float X, out bool ISitLosingSpeed) 
    {
        float directionJoystick = 0;
        if (X!=0)// si le joueur touche au joystick
        {
            directionJoystick = Mathf.Abs(X)/X;//divise par son opposé pour avoir le bon signe
        }//détermine la direction du joystick
        
        
        if (Mathf.Abs(X) > ValeurJoysticPourRotation &&  (directionJoystick == DirectionRotation ) && Mathf.Abs(VitesseMoto)>VitesseMinimumPourTourner && grounded)
        {
            Deraping = true;
            ActuelVitesseRotation += VitesseDeProgressionDuDerapage * DirectionRotation * Time.deltaTime;//ajoute la rotation
            checkRotationMoto(VitesseDeDerapageMax);//check si la rotation est encore bonne
            GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.Derape);
            ISitLosingSpeed = true;// indique que la moto perd de la vitesse
           
        }
        else if (Mathf.Abs(X) > ValeurJoysticPourRotation && (directionJoystick != DirectionRotation) && Mathf.Abs(VitesseMoto) > VitesseMinimumPourTourner && grounded)
        {
            ISitLosingSpeed = false;//indique que la moto ne perd pas de vitesse
            checkRotationMoto(VitesseDeRotationMax);//check si la rotation est encore bonne
            tourne(X);
        }
        else 
        {
            ISitLosingSpeed = false;//indique que la moto ne perd pas de vitesse
            checkRotationMoto(VitesseDeRotationMax);//check si la rotation est encore bonne
            ActuelVitesseRotation = 0;
        }
        GG.FeedBackVisu.GestionSmoke(ISitLosingSpeed);
        GG.FeedBackVisu.GestionWheelTrail(ISitLosingSpeed);
        GG.FeedBackVisu.GestionParticleRoue(ISitLosingSpeed);
        if (ActuelVitesseRotation!=0)
        {
            transform.rotation *= Quaternion.Euler(0, ActuelVitesseRotation, 0);// tourne la moto
        }
    }//tourne la moto pour le dérapage

    public void derapage(bool state, float X ,bool MustloseSpeed) //fais le dérapage
    {
        if (!state && X==0)
        {
             Deraping = false;

        }
        if (state && MustloseSpeed)// vérifie que je demande a déraper et que je dois perdre de la vitesse
        {
            
            float direction = Mathf.Abs(VitesseMoto) / VitesseMoto;// trouve la direction de la moto
            float pourcentage = Mathf.Abs(VitesseMoto) / vitesseMax;// détermine le pourcentage de vitesse de la moto
            if (Mathf.Abs(VitesseMoto) > VitesseMinimumPourDeraper && Mathf.Abs(X) > ValeurJoysticPourRotation)//  
            {
                VitesseMoto += -direction * ForceRalentissementDerapage  * RalentissementDerapage.Evaluate(pourcentage) * Time.deltaTime;//le calcul qui enléve
            }
            
        }
        
          



    }
    #endregion
    public void Freine(float directionFrein, float Input , float pourcentage) // fait freiner le véhicule
    {

        if (!GG.GB.Surchauffing && VitesseMoto!=0) //si le joueur ne surchauffe pas
        {
            if (Input == 0 || !grounded) //si le joueur n'appuie pas ou n'est plus sur le sol 
            {
                if (Input == 0 && grounded)
                {
                    VitesseMoto = Mathf.Lerp(VitesseMoto, 0, ForceRalentissement * Time.deltaTime * Ralentissement.Evaluate(pourcentage));// calcul la vitesse de la moto
                }
                else if (!grounded)
                {
                    VitesseMoto = Mathf.Lerp(VitesseMoto, 0, ForceRalentissementEnLair * Time.deltaTime * Ralentissement.Evaluate(pourcentage));// calcul la vitesse de la moto
                }
                
                if (!staffing && !GG.GB.boosting && !GG.GB.Surchauffing && !GG.GB.Recharge && !Deraping)
                {
                    GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.Ralenti);
                }
               
            }
            else if (grounded)// sinon si le joueur est sur le sol
            {

                VitesseMoto = Mathf.Lerp(VitesseMoto, directionFrein, PuissanceFrainage * Time.deltaTime * FreinageSelonVitesse.Evaluate(pourcentage));// calcul la vitesse de la moto
                if (!staffing && !GG.GB.boosting && !GG.GB.Surchauffing && !GG.GB.Recharge  )
                {
                    GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.Freine);
                }
               
            }
        }
       
        
        //changer cette partie
        checkVitesseMoto();//check la vitesse
    }
    public void ResetLastPosition() 
    {

        CancelInvoke();
        reseting = true;
        this.gameObject.transform.position = LastPostionOnCircuit;
        this.gameObject.transform.rotation = LastRotation;
        this.gameObject.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        LantiGravite.force = -transform.up * Physics.gravity.magnitude * FakeGravityMultiplier;
        //Physics.gravity = -transform.up * 9.81f * 3; //transform.TransformPoint(-transform.up * 9.81f * 3) //;
        VitesseMoto = 0;
        
    }
    #region checker
    void checkVitesseMoto() // check la vitesse de la moto
    {
        if (Mathf.Abs(VitesseMoto)-vitesseMax>=0.1)//si la moto vas plus vite que la vitesse max
        {           
            VitesseMoto -= DecellerationVitesseElleve*Time.deltaTime;//baisse la vitesse moto
        }
        else if (Mathf.Abs(VitesseMoto) - vitesseMax < 0.1)//pour le clamp la value
        {
            if (VitesseMoto > vitesseMax)//si supérieure
            {
                VitesseMoto = vitesseMax; // clamp vitesse
            }
            else if (VitesseMoto < -vitesseMax)//si inférieur a -vitesse max
            {
                VitesseMoto = -vitesseMax; // clamp vitesse
            }
            
            if (VitesseMoto > -SnapVitesse && VitesseMoto < SnapVitesse)//clamp pour arret
            {
                VitesseMoto = 0;//met la vitesse a 0
            }
        }
        
    
    }

    void checkRotationMoto(float max) 
    {

        GG.CSF.setRotationSpeedMax(max);
        if (ActuelVitesseRotation > max)
        {
            ActuelVitesseRotation = max;
        }
        if (ActuelVitesseRotation < -max)
        {
            ActuelVitesseRotation = -max;
        }
    }  // check si je dépasse pas la vitesse de rotation max
    #endregion
    void FreezeRotation()// freeze la rotation selon grounded ou non
    {
        
        
        if (grounded)//si sur le soll
        {
            Rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ ; // freeze Y et Z 
            //Rb.angularVelocity = new Vector3(Rb.angularVelocity.x, 0, Rb.angularVelocity.z);
            
        }
        else
        {
            Rb.constraints = RigidbodyConstraints.FreezeRotation;//freeze tout 
        }

    }//tentative

    void LorsqueCollision(Collision InfoDeCollision) 
    {
        GG.GUI.setTextCouleur();//cahnge la couleur du text
        GG.GUI.addPenality();
        if (GG.GB.Surchauffing)
        {
            
            GG.GB.LostBoost(GG.GB.PerteParCollisionInSurchauffe);//fait perdre du boost
        }
        else 
        {
           
            if (VitesseMoto>vitesseMax)
            {
                VitesseMoto = vitesseMax;//set la moto à la vitesse max
            }
            else 
            {
                VitesseMoto += -PerteParCollision * Mathf.Abs(VitesseMoto)/VitesseMoto;//fais perdre selon value
            }
        }
        GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.TraverseObstacle);
        
    }

    void LorsqueCollision(Collider InfoDeCollision)
    {
        GG.GUI.setTextCouleur();//cahnge la couleur du text
        GG.GUI.addPenality();
        if (GG.GB.Surchauffing)
        {

            GG.GB.LostBoost(GG.GB.PerteParCollisionInSurchauffe);//fait perdre du boost
        }
        else
        {

            if (VitesseMoto > vitesseMax)
            {
                VitesseMoto = vitesseMax;//set la moto à la vitesse max
            }
            else
            {
                VitesseMoto += -PerteParCollision * Mathf.Abs(VitesseMoto) / VitesseMoto;//fais perdre selon value
            }
        }
        GG.EtatEtFeedback.changementDetat(GestionEtatEtFeedback.MotoActualState.TraverseObstacle);
        
    }
    void DeclenchementParticuleSelonVitesse() 
    {
        if (VitesseMoto>=2)
        {
            GG.FeedBackVisu.GestionWindTrail(true);
        }
        else 
        {
            GG.FeedBackVisu.GestionWindTrail(false);
        }
        if (VitesseMoto >0)
        {
            GG.FeedBackVisu.GestionRedLight(true);
        }
        else 
        {
            GG.FeedBackVisu.GestionRedLight(false);
        }
        if (VitesseMoto>0.5f)
        {
            GG.FeedBackVisu.GestionParticleCam(true);
        }
        else 
        {
            GG.FeedBackVisu.GestionParticleCam(false);
        }
        /*if (Mathf.Abs(VitesseMoto) > vitesseMax * 0.5f)
        {
            ParticleCam.SetActive(true);
        }
        else
        {

            ParticleCam.SetActive(false);
        }*/
    }


    private void OnTriggerEnter(Collider other)
    {
       
            if (other.transform.CompareTag("Obstacle"))
            {

                LorsqueCollision(other);


            }
        
    }


    private void OnCollisionEnter(Collision collision)//si le joueur touche qqchose
    {
        if ( collision.transform.gameObject.layer == 8) // si le layer est sol //utile? sers à mettre le vhéhicule correctement
        {
           
            if (!grounded  ) //si n'est pas sur le sol
            {
                
                
                Vector3 H = collision.GetContact(0).point;
               RaycastHit Info;
               Physics.Raycast(detecteur.position, -transform.up, out Info, GroundRayLength, whatIsGround);
                float angleX = transform.rotation.eulerAngles.x;
                float angleY = transform.rotation.eulerAngles.y;
                float angleZ = transform.rotation.eulerAngles.z;
                transform.rotation = Quaternion.FromToRotation(transform.up, Info.normal)* Quaternion.Euler(0, angleY, 0);
                //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, angleY, transform.rotation.eulerAngles.z);//reset rot
                //transform.rotation = Quaternion.Euler(0, angleY, 0);
            }
            Rb.velocity = Vector3.zero;//met la velocity a 0

        }
         if (collision.transform.CompareTag("Obstacle"))
        {
            
            LorsqueCollision(collision);


        }
    }
    
   
}
