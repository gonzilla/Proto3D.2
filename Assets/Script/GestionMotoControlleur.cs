using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionMotoControlleur : PersonnalMethod
{
    //Public variable
    public float VitesseMoto;
    public float AccélérationMoto,vitesseMax;
    public AnimationCurve AccelerationSelonVitesseMax;
    public float PuissanceFrainage;
    public AnimationCurve FreinageSelonVitesse;
    public float angleMaxInAir;
    public AnimationCurve Ralentissement;
    public float ForceRalentissement;


    public float VitesseRotation;

    public float DecellerationVitesseElleve;

    public float TempsPourEtreEnLair;

    public Transform detecteur;
    public float GroundRayLength;
    public LayerMask whatIsGround;

    public float angleMax;
    //Local variable
    float TimeCible;
    //float AngleX;
    Vector3 DirectionForMoto;
    bool grounded;
    bool OnceForFloor;
    Rigidbody Rb;
    GestionGeneral GG;
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        GetGestion(out GG, this.gameObject);
    }
    public void SetByNormal() 
    {

        grounded = false;
        RaycastHit hit;
       
       
        if (Physics.Raycast(detecteur.position, -transform.up, out hit, GroundRayLength, whatIsGround))
        {
            //print("touche terrain");
            grounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            Debug.DrawRay(detecteur.position, -transform.up * GroundRayLength, Color.magenta);
            OnceForFloor = false;
            FreezeRotation();
        }
        if (!grounded)
        {
            if (!OnceForFloor)
            {
                TimeCible = Time.time+TempsPourEtreEnLair;
                OnceForFloor = true;
                
            }
            if (OnceForFloor && TimeCible>= Time.time )
            {
                //AngleX = UnityEditor.TransformUtils.GetInspectorRotation(this.gameObject.transform).x;
                FreezeRotation();
            }
            //print(UnityEditor.TransformUtils.GetInspectorRotation(this.gameObject.transform).x);
        }
        


    }
    

    public void avance(float Direction) 
    {
        
            
                
                
            float DirectionMoteur = 0;
            float DirectionVoulue = 0;
            float pourcentage = Mathf.Abs( VitesseMoto) / vitesseMax;
            if (VitesseMoto != 0)
            {
                DirectionMoteur = VitesseMoto / Mathf.Abs(VitesseMoto);
            }
            if (Direction != 0)
            {
                DirectionVoulue = Direction / Mathf.Abs(Direction);
            }

            if (DirectionMoteur != DirectionVoulue && DirectionMoteur != 0)
            {
                Freine(DirectionVoulue, Direction, pourcentage);
            }
            else if (DirectionMoteur == DirectionVoulue || DirectionMoteur == 0)
            {
                if (grounded){
                if (VitesseMoto<vitesseMax)
                {
                    VitesseMoto += AccélérationMoto * Direction * Time.deltaTime * AccelerationSelonVitesseMax.Evaluate(pourcentage);
                }
                
                checkVitesseMoto();
                }
            }
        /*if (grounded)
        {
            transform.Translate(transform.forward * VitesseMoto, Space.World);
        }    
        else if (!grounded) 
        {
            transform.Translate(Vector3.zero * VitesseMoto, Space.World);
        
        }*/

        transform.Translate(transform.forward * VitesseMoto, Space.World);
        //transform.Translate(DirectionForMoto * VitesseMoto, Space.World);


    }
    public void tourne(float X) 
    {

        if (grounded)//&& Mathf.Abs(VitesseMoto)>0.01f 
        {
            float Rotation = X * VitesseRotation * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0, Rotation, 0);
            /*float angleToGo = X * angleMax;
            DirectionForMoto = transform.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * angleToGo), 0, Mathf.Cos(Mathf.Deg2Rad * angleToGo)).normalized ;
            Debug.DrawRay(transform.position, DirectionForMoto * 3, Color.red);*/
        }
        
       
        //float Z = Input.GetAxis("VerticalManette");
        //float angleToGo = X * angleMax;
        
        //transform.rotation
        //Vector3 DirectionRay = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angleToGo), 0, Mathf.Cos(Mathf.Deg2Rad * angleToGo));
        //Debug.DrawRay(transform.position, DirectionRay * 10, Color.red);
    }

    public void Freine(float directionFrein, float Input , float pourcentage) 
    {
        
        if ((Input == 0 || !grounded)&& !GG.GB.Surchauffing)
        {
            // un faible ralentissement
            VitesseMoto = Mathf.Lerp(VitesseMoto, 0, ForceRalentissement * Time.deltaTime * Ralentissement.Evaluate(pourcentage));
            //VitesseMoto += directionFrein*Mathf.Abs(Input)*ForceRalentissement*Input*Time.deltaTime;
        }
        else if (grounded && !GG.GB.Surchauffing)
        {

            VitesseMoto = Mathf.Lerp(VitesseMoto, directionFrein, PuissanceFrainage * Time.deltaTime * FreinageSelonVitesse.Evaluate(pourcentage));
            // Gros Freinage
            //VitesseMoto =
            //VitesseMoto += directionFrein * PuissanceFrainage * Time.deltaTime * FreinageSelonVitesse.Evaluate();
        }

        checkVitesseMoto();
    }

    void checkVitesseMoto() 
    {
        if (Mathf.Abs(VitesseMoto)-vitesseMax>=0.1)
        {
           
            VitesseMoto -= DecellerationVitesseElleve*Time.deltaTime;
        }
        else if (Mathf.Abs(VitesseMoto) - vitesseMax < 0.1)
        {
            if (VitesseMoto > vitesseMax)
            {
                VitesseMoto = vitesseMax;
            }
            else if (VitesseMoto < -vitesseMax)
            {
                VitesseMoto = -vitesseMax;
            }
            float limite = 0.001f;
            if (VitesseMoto > -limite && VitesseMoto < limite)
            {
                VitesseMoto = 0;
            }
        }
        
    
    }

    void FreezeRotation()
    {
        
        
        if (grounded)
        {
            Rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            Rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

    }//tentative

    private void OnCollisionEnter(Collision collision)
    {
        if ( collision.transform.gameObject.layer == 8)
        {
            Rb.velocity = Vector3.zero;
            if (!grounded)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
            }
            
        }
    }
    private void OnCollisionExit(Collision collision)
    {

        /*if (collision.transform.gameObject.layer == 8)
        {
            FreezeRotation();

        }*/
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float X = Input.GetAxis("HorizontalManette");
        float angleToGo = X * angleMax;
        Vector3 coordonnéespehere = transform.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * angleToGo), 0, Mathf.Cos(Mathf.Deg2Rad * angleToGo)).normalized * 10;
        Gizmos.DrawWireSphere(coordonnéespehere,0.2f);
    }
}
